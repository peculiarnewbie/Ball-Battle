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
    public Energy enemyEnergy;
    public Energy playerEnergy;
    public List<Attackers> attackers;
    public List<Defenders> defenders;

    bool isPlacing;
    bool isPlaying;

    public bool isBallHeld;

    public Rigidbody prefab;
    public Rigidbody soldierRB;

    public Transform playerTarget;
    public Transform enemyTarget;
    public Transform ballTransform;

    // Start is called before the first frame update
    void Start()
    {
        isPlacing = false;

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

    private void StartPlacing(Vector2 screenPosition, float time){
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane);
        
        var ray = RayPosition();
        if(!ray.wasHit) return;
        isPlacing = true;
        soldierRB = Instantiate(prefab, ray.position, Quaternion.identity);
        
    }

    private void PositionSoldier(){
        // Debug.Log(inputManager.GetPosition());
        var ray = RayPosition();
        if(!ray.wasHit) return;

        Vector3 targetPosition = ray.position - soldierRB.position;
        targetPosition = soldierRB.position + targetPosition * Time.deltaTime * 10;
        soldierRB.MovePosition(new Vector3(targetPosition.x, soldierRB.position.y, targetPosition.z));
            
    }

    private void PlaceSoldier(Vector2 screenPositoin, float time){
        Debug.Log("letgo");
        isPlacing = false;
        soldierRB.GetComponent<Soldiers>().SoldierPlaced();
    }

    private (bool wasHit, Vector3 position) RayPosition(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputManager.GetPosition());
        if(Physics.Raycast(ray, out hit, 100.0f)){
            if(hit.collider.CompareTag("Player Field")){
                return (true, hit.point);
            }
        }
        return (false, Vector3.zero);
    }

    private void UpdateTime(){
        matchTime -= Time.deltaTime;
        if(matchTime < 0){

        }
    }

    private void ResetMatch(){

    }
}
