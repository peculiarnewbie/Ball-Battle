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
    private IEnumerator activationCoroutine;
    [SerializeField] protected bool isMoving;
    [SerializeField] protected bool activated;
    public bool isPlayers;
    protected Renderer rend;
    protected bool isSpeedy;
    protected MatchManager matchManager;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        SetSoldierColor();
        matchManager = MatchManager.Instance;

        goalTarget = matchManager.playerTarget;
        ballTransform = matchManager.ballTransform;

        activated = false;
        isMoving = false;

        AssignStartValue();
        
        activationCoroutine = WaitforActivation(activationTime);

        matchManager.OnBallPickup += BallPickupSwitch;
        
    }

    protected void MoveToTarget(float speed){
        float speedMultiplier = matchManager.gameSpeedMultiplier;
        //Moving rigidbody
        Vector3 targetPosition = targetTransform.position - transform.position;
        targetPosition = targetPosition.normalized;

        rb.MovePosition(transform.position + targetPosition * Time.deltaTime * speed * speedMultiplier);
        
        //Rotating rigidbody
        Vector3 localTarget = transform.InverseTransformPoint(targetTransform.position);

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
        StartCoroutine(activationCoroutine);
    }

    public IEnumerator WaitforActivation(float waitTime){
        
        yield return new WaitForSeconds(waitTime);
        ActivateSoldier();
    }

    public abstract void AssignStartValue();

    public abstract void ActivateSoldier();

    public abstract void BallPickupSwitch(bool value);


}
