using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defenders : Soldiers
{
    float radius = 17.5f;
    bool isChasing = false;
    bool isGoingBack = false;
    Vector3 origin;
    public float chaseTreshold = 2f;

    private void Update() {
        if(!activated) return;
        if(matchManager.isBallHeld) isChasing = CheckForBallHolder();
        
    }

    private void FixedUpdate() {
        rangeIndicator.transform.position = origin;
        if(isBeingPlaced) return;
        if(isChasing && !matchManager.ballController.isbeingPassed) ChaseSoldier();
        else if(!CheckIfOnOrigin()) MoveToTarget(fastSpeed, origin);
    }

    public override void AssignStartValue(){
        slowSpeed = 1f;
        fastSpeed = 2f;
        activationTime = 4f;
        isSpeedy = false;
        isAttacker = false;
        

        matchManager.OnAttackerCaught += AttackerCaught;
    }

    public override void ActivateSoldier(){
        rangeIndicator.SetActive(true);
        activated = true;
        isMoving = true;
        animationKeys.PlayAnimation("Activate");
        SetSoldierColor();
        isBeingPlaced = false;
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public override void DeactivateSoldier(){
        rangeIndicator.SetActive(false);
        isChasing = false;
        activated = false;
        SetSoldierColor();
        StartCoroutine(WaitforActivation(activationTime));
    }

    public override void BallPickupSwitch(bool value){
        // if(value) targetTransform = ballTransform;
        // else targetTransform = goalTarget;
    }

    private void AttackerCaught(){
        
    }

    private bool CheckForBallHolder(){
        Collider[] hitColliders = Physics.OverlapSphere(origin, radius);
        foreach(Collider hitCollider in hitColliders){
            if(hitCollider.CompareTag("Soldier")){
                Soldiers soldier = hitCollider.GetComponent<Soldiers>();
                if(soldier.isHoldingBall){
                    targetTransform = soldier.transform;
                    return true;
                } 
            }
        }
        return false;
    }

    private bool CheckIfOnOrigin(){
        float distance = Vector3.Distance(transform.position, origin);
        if(distance <= 1f) return true;
        else return false;
    }

    private void GoBack(){
        MoveToTarget(fastSpeed, origin);
        float distance = Vector3.Distance(transform.position, origin);
        if(distance <= 1f) isGoingBack = false;
    }

    private void ChaseSoldier(){
        MoveToTarget(slowSpeed, targetTransform.position);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        
        if(distance <= chaseTreshold){
            matchManager.AttackerCaught();
            isGoingBack = true;
            DeactivateSoldier();
        } 
    }
}