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

    public List<Attackers> attackers;
    public List<Defenders> defenders;

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

    // Start is called before the first frame update
    void Start()
    {
        isPlacing = false;
        isPlaying = false;

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
        if(isPlayer == isPlayerAttacking) soldierToPlace = soldierPool.GetPooledObject(true);
        else soldierToPlace = soldierPool.GetPooledObject(false);

        soldierToPlaceScript = soldierToPlace.GetComponent<Soldiers>();
        soldierToPlaceScript.isPlayers = isPlayer;

        soldierToPlace.SetActive(true);
        
        soldierToPlaceRB = soldierToPlace.GetComponent<Rigidbody>();
        soldierToPlaceRB.MovePosition(new Vector3(hit.x, soldierToPlaceRB.position.y, hit.z));
        
        Debug.Log(isPlayer.ToString() + ' ' + isPlayerAttacking.ToString());

        
    }

    private void PositionSoldier(){
        // Debug.Log(inputManager.GetPosition());
        var ray = RayPosition(false);
        if(!ray.wasHit) return;

        Vector3 targetPosition = ray.position - soldierToPlaceRB.position;
        targetPosition = soldierToPlaceRB.position + targetPosition * Time.deltaTime * 10;
        soldierToPlaceRB.MovePosition(new Vector3(targetPosition.x, soldierToPlaceRB.position.y, targetPosition.z));
            
    }

    private void PlaceSoldier(Vector2 screenPositoin, float time){
        
        isPlacing = false;
        soldierToPlaceScript.SoldierPlaced();
        
    }

    private (bool wasHit, Vector3 position) RayPosition(bool isFirst){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputManager.GetPosition());
        if(Physics.Raycast(ray, out hit, 100.0f)){
            if(hit.collider.CompareTag("Player Field")){
                if(isFirst) GetSoldier(true, hit.point);
                return (true, hit.point);
            }
            else if(hit.collider.CompareTag("Enemy Field")){
                if(isFirst) GetSoldier(false, hit.point);
                return (true, hit.point);
            }
        }
        return (false, Vector3.zero);
    }

    
}
