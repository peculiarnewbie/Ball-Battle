using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Soldiers : MonoBehaviour
{
    Rigidbody rb;
    public Transform targetTransform;
    public Transform goalTarget;
    public Transform ballTransform;
    [SerializeField] protected float slowSpeed;
    [SerializeField] protected float fastSpeed;
    float rotationSpeed = 3f;
    [SerializeField] protected float activationTime;
    protected IEnumerator activationCoroutine;
    [SerializeField] protected bool isMoving;
    public bool activated;
    protected Renderer rend;
    protected bool isSpeedy;
    protected MatchManager matchManager;

    public bool isPlayers;
    public bool isAttacker;
    public int assignedIndex;
    public bool isHoldingBall;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        SetSoldierColor();
        matchManager = MatchManager.Instance;

        goalTarget = matchManager.playerGoalTarget;
        ballTransform = matchManager.ballTransform;
    }

    private void OnEnable() {
        activated = false;
        isMoving = false;

        AssignStartValue();
        
        activationCoroutine = WaitforActivation(activationTime);

        matchManager.OnBallPickup += BallPickupSwitch;
    }

    private void OnDisable() {
        matchManager.OnBallPickup -= BallPickupSwitch;
    }

    protected void MoveToTarget(float speed, Vector3 targetTransformPosition){
        float speedMultiplier = matchManager.gameSpeedMultiplier;
        //Moving rigidbody
        Vector3 targetPosition = targetTransformPosition - transform.position;
        targetPosition = targetPosition.normalized;

        rb.MovePosition(transform.position + targetPosition * Time.deltaTime * speed * speedMultiplier);
        
        //Rotating rigidbody
        Vector3 localTarget = transform.InverseTransformPoint(targetTransformPosition);

        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * rotationSpeed * speedMultiplier);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void SetSoldierColor(){
        if(isPlayers){
            rend.material.color = Color.blue;
        }
        else rend.material.color = Color.red;
    }

    public void SoldierPlaced(){
        if(isPlayers){
            goalTarget = matchManager.playerGoalTarget;
        }
        else{
            goalTarget = matchManager.enemyGoalTarget;
        }
        StartCoroutine(activationCoroutine);
    }

    public IEnumerator WaitforActivation(float waitTime){
        
        yield return new WaitForSeconds(waitTime);
        ActivateSoldier();
    }

    public void RemoveSoldier(){
        matchManager.RemoveSoldier(this, isAttacker);
        this.gameObject.SetActive(false);
    }

    public abstract void AssignStartValue();

    public abstract void ActivateSoldier();

    public abstract void DeactivateSoldier();

    public abstract void BallPickupSwitch(bool value);


}
