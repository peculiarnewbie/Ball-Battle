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
    protected GameObject rangeIndicator;
    public GameObject holdingIndicator;
    protected bool isSpeedy;
    protected MatchManager matchManager;
    protected bool isBeingPlaced = true;
    public SoldierAnimations animationKeys;

    public bool isPlayers;
    public bool isAttacker;
    public int assignedIndex;
    public bool isHoldingBall;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        rangeIndicator = GetComponentInChildren<RangeIndicator>().gameObject;
        holdingIndicator = GetComponentInChildren<HoldingIndicator>().gameObject;
        animationKeys = GetComponent<SoldierAnimations>();
        
        matchManager = MatchManager.Instance;
    }

    private void OnEnable() {
        Debug.Log("enabled");

        activated = false;
        isMoving = false;
        isBeingPlaced = true;

        rangeIndicator.SetActive(false);
        holdingIndicator.SetActive(false);

        AssignStartValue();
        SetSoldierColor();

        activationCoroutine = WaitforActivation(activationTime);

        goalTarget = matchManager.playerGoalTarget;
        ballTransform = matchManager.ballTransform;

        matchManager.OnBallPickup += BallPickupSwitch;
        matchManager.OnScore += StopMoving;
    }

    private void OnDisable() {
        activated = false;
        isMoving = false;
        
        matchManager.OnBallPickup -= BallPickupSwitch;
        matchManager.OnScore -= StopMoving;
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

    protected void SetSoldierColor(){
        if(isPlayers){
            if(activated) rend.material.color = Color.blue;
            else rend.material.color = new Color(0.43f, 0.43f, 0.75f);
        }
        else{
            if(activated) rend.material.color = Color.red;
            else rend.material.color = new Color(0.75f, 0.43f, 0.43f);
        }
    }

    public void SoldierPlaced(){
        if(!this.gameObject.activeInHierarchy) return;
        transform.localScale = new Vector3(2f, 2f, 2f);
        animationKeys.PlayAnimation("Place");
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

    private void StopMoving(){
        isMoving = false;
    }

    public abstract void AssignStartValue();

    public abstract void ActivateSoldier();

    public abstract void DeactivateSoldier();

    public abstract void BallPickupSwitch(bool value);


}
