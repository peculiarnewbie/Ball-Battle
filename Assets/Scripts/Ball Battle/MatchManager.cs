using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviourSingleton<MatchManager>
{
    public delegate void BallPickupEvent(bool ballPickUp);
    public event BallPickupEvent OnBallPickup;


    private InputManager inputManager;
    private Camera mainCamera;

    public float gameSpeedMultiplier = 1f;

    int match = 1;
    float matchTime = 140f;
    bool isPlayerAttacking = true;

    public Energy enemyEnergy;
    public Energy playerEnergy;

    public List<Soldiers> attackers;
    public List<Soldiers> defenders;

    bool isPlacing;
    bool isPlaying;

    public bool isBallHeld;

    public ObjectPool soldierPool;
    private GameObject soldierToPlace;
    private Soldiers soldierToPlaceScript;
    private Rigidbody soldierToPlaceRB;

    public Transform playerGoalTarget;
    public Transform enemyGoalTarget;
    public Transform ballTransform;
    Collider playerField;
    Collider enemyField;

    // Start is called before the first frame update
    void Start()
    {
        isPlacing = false;
        isPlaying = false;
        isBallHeld = false;

        playerField = GameObject.FindGameObjectWithTag("Player Field").GetComponent<Collider>();
        enemyField = GameObject.FindGameObjectWithTag("Enemy Field").GetComponent<Collider>();

        mainCamera = Camera.main;
        inputManager = InputManager.Instance;
        inputManager.OnStartTouch += StartPlacing;
        inputManager.OnEndTouch += PlaceSoldier;
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlacing) PositionSoldier();
        if(isPlaying) UpdateTime();

    }

    public void ChangeBallPickup(bool value){
        isBallHeld = value;
        OnBallPickup(value);
    }

    private void UpdateTime(){
        matchTime -= Time.deltaTime;
        if(matchTime < 0){

        }
    }

    private void ResetMatch(){
        isPlayerAttacking = !isPlayerAttacking;
    }

    // Functions for placing soldiers, might refactor to another class

    private void StartPlacing(Vector2 screenPosition, float time){
        var ray = RayPosition(true);
        if(!ray.wasHit) return;
        isPlacing = true;
        // soldierRB = Instantiate(prefab, ray.position, Quaternion.identity);
        
    }

    private void GetSoldier(bool isPlayer, Vector3 hit){
        // xnor
        if(isPlayer == isPlayerAttacking){
            soldierToPlace = soldierPool.GetPooledObject(true);
            soldierToPlaceScript = soldierToPlace.GetComponent<Soldiers>();
            attackers.Add(soldierToPlaceScript);
            soldierToPlaceScript.assignedIndex = attackers.Count - 1;
        } 
        else{
            soldierToPlace = soldierPool.GetPooledObject(false);
            soldierToPlaceScript = soldierToPlace.GetComponent<Soldiers>();
            defenders.Add(soldierToPlaceScript);
            soldierToPlaceScript.assignedIndex = defenders.Count - 1;
        } 

        soldierToPlaceScript.isPlayers = isPlayer;

        soldierToPlace.SetActive(true);
        
        soldierToPlaceRB = soldierToPlace.GetComponent<Rigidbody>();
        soldierToPlaceRB.MovePosition(new Vector3(hit.x, soldierToPlaceRB.position.y, hit.z));
        soldierToPlaceRB.isKinematic = true;

        Debug.Log(isPlayer.ToString() + ' ' + isPlayerAttacking.ToString());

        
    }

    private void PositionSoldier(){
        // Debug.Log(inputManager.GetPosition());
        var ray = RayPosition(false);
        if(!ray.wasHit) return;

        Bounds fieldBound;
        if(soldierToPlaceScript.isPlayers) fieldBound = playerField.bounds;
        else fieldBound = enemyField.bounds;

        // Vector3 targetPosition = ray.position - soldierToPlaceRB.position;
        // targetPosition = soldierToPlaceRB.position + targetPosition * Time.deltaTime * 10;
        // soldierToPlaceRB.MovePosition(new Vector3(targetPosition.x, soldierToPlaceRB.position.y, targetPosition.z));

        float range = (ray.position - transform.position).magnitude;
        Vector3 targetPosition = Vector3.Lerp(soldierToPlaceRB.position, ray.position, 0.02f* Mathf.Pow(range, 1f/3f));
        // targetPosition = targetPosition.normalized;

        if(targetPosition.x > fieldBound.max.x) targetPosition.x = fieldBound.max.x;
        if(targetPosition.x < fieldBound.min.x) targetPosition.x = fieldBound.min.x;
        if(targetPosition.z > fieldBound.max.z) targetPosition.z = fieldBound.max.z;
        if(targetPosition.z < fieldBound.min.z) targetPosition.z = fieldBound.min.z;

        soldierToPlaceRB.MovePosition(new Vector3(targetPosition.x, soldierToPlaceRB.position.y, targetPosition.z));
            
    }

    private void PlaceSoldier(Vector2 screenPositoin, float time){
        
        isPlacing = false;

        soldierToPlaceRB.isKinematic = false;
        soldierToPlaceRB.velocity = new Vector3(0f, 0f, 0f);
        soldierToPlaceRB.angularVelocity = new Vector3(0f, 0f, 0f);
        soldierToPlace.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));

        soldierToPlaceScript.SoldierPlaced();
        
    }

    private (bool wasHit, Vector3 position) RayPosition(bool isFirst){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputManager.GetPosition());
        if(Physics.Raycast(ray, out hit, 100.0f)){
            if(hit.collider == playerField){
                if(isFirst) GetSoldier(true, hit.point);
                if(soldierToPlaceScript.isPlayers) return (true, hit.point);
            }
            else if(hit.collider == enemyField){
                if(isFirst) GetSoldier(false, hit.point);
                if(!soldierToPlaceScript.isPlayers) return (true, hit.point);
            }
        }
        
        if(!isFirst) return (true, ray.origin);
        
        return (false, Vector3.zero);
    }

    // heavy coupling with function in both Soldiers
    public void RemoveSoldier(int index, bool isAttacker){
        if(isAttacker) attackers.RemoveAt(index);
        else defenders.RemoveAt(index);
    }

    
}
