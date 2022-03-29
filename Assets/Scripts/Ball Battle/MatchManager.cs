using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviourSingleton<MatchManager>
{
    public delegate void BallPickupEvent(bool ballPickUp);
    public event BallPickupEvent OnBallPickup;
    public delegate void AttackerCaughtEvent();
    public event AttackerCaughtEvent OnAttackerCaught;
    public delegate void ScoreEvent();
    public event ScoreEvent OnScore;
    public delegate void MatchStartEvent();
    public event MatchStartEvent OnMatchStart;


    private InputManager inputManager;
    private Camera mainCamera;
    [SerializeField] CameraController camController;

    public float gameSpeedMultiplier = 1f;

    int match = 0;
    public float matchTime = 140f;
    public bool isPlayerAttacking = true;

    public Energy enemyEnergy;
    public Energy playerEnergy;

    public List<Soldiers> attackers;
    public List<Soldiers> defenders;

    bool isPlacing;
    bool isPlayerPlacing;
    bool isEnemyPlacing;
    bool isPlaying;

    public int enemyScore;
    public int playerScore;

    public bool isBallHeld;

    public ObjectPool soldierPool;
    private GameObject soldierToPlace;
    private Soldiers soldierToPlaceScript;
    private Rigidbody soldierToPlaceRB;

    public Transform ballTransform;
    public BallController ballController;

    public Transform playerGoalTarget;
    public Transform enemyGoalTarget;
    public Collider playerField;
    public Collider enemyField;

    void Start()
    {
        playerField = GameObject.FindGameObjectWithTag("Player Field").GetComponent<Collider>();
        enemyField = GameObject.FindGameObjectWithTag("Enemy Field").GetComponent<Collider>();
        ballController = ballTransform.GetComponent<BallController>();

        mainCamera = Camera.main;
        inputManager = InputManager.Instance;
        inputManager.OnStartTouch += StartPlacing;
        inputManager.OnEndTouch += PlaceSoldier;

        StartMatch(); /* Move if match not immediate */
    }

    private void StartMatch(){
        match++;
        if(match%2 == 1) isPlayerAttacking = true;
        else isPlayerAttacking = false;

        if(OnMatchStart != null) OnMatchStart();

        matchTime = 140f;

        enemyEnergy.actualEnergy = 0.0f;
        playerEnergy.actualEnergy = 0.0f;

        isPlacing = false;
        isPlaying = true;
        isBallHeld = false;
        isPlayerPlacing = false;
        isEnemyPlacing = false;
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    void Update()
    {
        if(isPlacing) PositionSoldier();
        if(isPlaying) UpdateTime();

    }

    private void UpdateTime(){
        matchTime -= Time.deltaTime;
        if(matchTime < 0){
            isPlaying = false;
            Scored(false);
        }
    }

    public void Scored(bool isAttacker){
        Debug.Log("wha");
        if(isPlayerAttacking == isAttacker) playerScore += 1;
        else enemyScore += 1;
        if(OnScore != null) OnScore();
        StartCoroutine(MatchTransition());

    }

    public IEnumerator MatchTransition(){

        yield return new WaitForEndOfFrame();
        // Explosion & Score Effects

        RemoveAllSoldiers();

        // Start Match

        StartMatch();

        yield return null;
    }

    public void ChangeBallPickup(bool value){
        isBallHeld = value;
        if(OnBallPickup != null) OnBallPickup(value);
    }

    public void AttackerCaught(){
        if(OnAttackerCaught != null) OnAttackerCaught();
        float closestRange = 100f;
        int closestIndex = -1;
        int i = 0;
        foreach(Soldiers soldier in attackers){
            if(!soldier.activated || soldier.isHoldingBall) {
                if(soldier.isHoldingBall){
                    soldier.DeactivateSoldier();
                    soldier.isHoldingBall = false;
                }
                i++;
                continue;
            };
            float range = Vector3.Distance(ballTransform.position, soldier.transform.position);
            if(range < closestRange){
                closestRange = range;
                closestIndex = i;
            }
        }

        if(closestIndex == -1){
            isBallHeld = false;
        }
        else{
            Soldiers target = attackers[closestIndex];
            target.isHoldingBall = true;
            ballController.targetTransform = target.transform;
            ballController.isbeingPassed = true;
        }
    }

    /* Further down are functions for placing soldiers
    might refactor to another class */

    private void StartPlacing(Vector2 screenPosition, float time){
        var ray = RayPosition(true);
        if(!ray.wasHit) return;
        isPlacing = true;
    }

    private void PositionSoldier(){
        var ray = RayPosition(false);
        if(!ray.wasHit) return;

        Bounds fieldBound;
        if(soldierToPlaceScript.isPlayers) fieldBound = playerField.bounds;
        else fieldBound = enemyField.bounds;

        float range = (ray.position - transform.position).magnitude;
        Vector3 targetPosition = Vector3.Lerp(soldierToPlaceRB.position, ray.position, 0.02f* Mathf.Pow(range, 1f/3f));
        // targetPosition = targetPosition.normalized;

        if(targetPosition.x > fieldBound.max.x) targetPosition.x = fieldBound.max.x;
        if(targetPosition.x < fieldBound.min.x) targetPosition.x = fieldBound.min.x;
        if(targetPosition.z > fieldBound.max.z) targetPosition.z = fieldBound.max.z;
        if(targetPosition.z < fieldBound.min.z) targetPosition.z = fieldBound.min.z;

        soldierToPlaceRB.MovePosition(new Vector3(targetPosition.x, soldierToPlaceRB.position.y, targetPosition.z));  
    }

    private (bool wasHit, Vector3 position) RayPosition(bool isFirst){
        // if(!isFirst && !isPlacing) return (false, Vector3.zero);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputManager.GetPosition());
        if(Physics.Raycast(ray, out hit, 100.0f)){
            if(hit.collider == playerField){
                if(isFirst) GetSoldier(true, hit.point);
                if(soldierToPlaceScript?.isPlayers == true) return (true, hit.point);
            }
            else if(hit.collider == enemyField){
                if(isFirst) GetSoldier(false, hit.point);
                if(soldierToPlaceScript?.isPlayers == false) return (true, hit.point);
            }
        }
        
        if(!isFirst) return (true, ray.origin);
        
        return (false, Vector3.zero);
    }

    private void GetSoldier(bool isPlayer, Vector3 hit){
        if(isPlayer){
            if(isPlayerPlacing) return;
        }
        else if(isEnemyPlacing) return;

        /* xnor */
        if(isPlayer == isPlayerAttacking) soldierToPlace = soldierPool.GetPooledObject(true);
        else soldierToPlace = soldierPool.GetPooledObject(false);

        soldierToPlaceScript = soldierToPlace.GetComponent<Soldiers>();
        soldierToPlaceScript.isPlayers = isPlayer;
        soldierToPlace.SetActive(true);
        
        soldierToPlaceRB = soldierToPlace.GetComponent<Rigidbody>();
        soldierToPlaceRB.MovePosition(new Vector3(hit.x, soldierToPlaceRB.position.y, hit.z));
        soldierToPlaceRB.isKinematic = true;
    }

    private void PlaceSoldier(Vector2 screenPositoin, float time){

        if(!isPlacing) return;
        
        isPlacing = false;

        if(soldierToPlaceScript.isPlayers){
            if(isPlayerPlacing) return;
        }
        else if(isEnemyPlacing) return;

        soldierToPlaceRB.isKinematic = false;
        soldierToPlaceRB.velocity = new Vector3(0f, 0f, 0f);
        soldierToPlaceRB.angularVelocity = new Vector3(0f, 0f, 0f);
        soldierToPlace.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));

        StartCoroutine(WaitForEnergy(soldierToPlaceScript));
         
    }

    private bool CheckEnergy(bool isAttacker, bool isPlayer){
        int cost;
        if(isAttacker) cost = 2;
        else cost = 3;

        if(isPlayer) return playerEnergy.UseEnergy(cost);
        else return enemyEnergy.UseEnergy(cost);
    }

    private IEnumerator WaitForEnergy(Soldiers soldier){
        if(soldier.isPlayers) isPlayerPlacing = true;
        else isEnemyPlacing = true;
        AddSoldierToList(soldier.isAttacker, soldier);
        
        while(!CheckEnergy(soldier.isAttacker, soldier.isPlayers)){
            yield return null; /* waits a frame each frame until enough energy */
        }
        soldier.SoldierPlaced();
        

        if(soldier.isPlayers) isPlayerPlacing = false;
        else isEnemyPlacing = false;
        
        yield return null;
    }

    public void AddSoldierToList(bool isAttacker, Soldiers soldier){
        if(isAttacker){
            attackers.Add(soldier);
            soldierToPlaceScript.assignedIndex = attackers.Count - 1;
        } 
        else {
            defenders.Add(soldier);
            soldierToPlaceScript.assignedIndex = defenders.Count - 1;
        }
    }

    /* heavy coupling with function in both Soldiers */
    public void RemoveSoldier(Soldiers soldier, bool isAttacker){
        if(isAttacker) attackers.Remove(soldier);
        else defenders.Remove(soldier);
    }

    private void RemoveAllSoldiers(){
        if(attackers.Count != 0){
            foreach(Soldiers attacker in attackers){
                attacker.gameObject.SetActive(false);
            }
            attackers.Clear();
        }  

        if(defenders.Count != 0){
            foreach(Soldiers defender in defenders){
                defender.gameObject.SetActive(false);
            }
            defenders.Clear();
        }
    }
}
