using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackers : Soldiers
{
    public override void AssignStartValue(){
        slowSpeed = 0.75f;
        fastSpeed = 1.5f;
        activationTime = 2.5f;
        isSpeedy = true;
        isAttacker = true;
    }

    public override void ActivateSoldier(){
        if (matchManager.isBallHeld) targetTransform = goalTarget;
        else targetTransform = ballTransform;
        activated = true;
        isMoving = true;
    }

    public override void DeactivateSoldier(){
        activated = false;
        isMoving = false;
        StartCoroutine(WaitforActivation(activationTime));
    }

    public override void BallPickupSwitch(bool value){
        if(value) targetTransform = goalTarget;
        else targetTransform = ballTransform;
    }

    private void CheckIfReachedEnd(){
        float endLine;
        bool isAtEnd = false;

        if(isPlayers){
            endLine = matchManager.enemyField.bounds.max.z - 2f;
            if(transform.position.z > endLine) isAtEnd = true;;
        } 
        else{
            endLine = matchManager.playerField.bounds.min.z + 2f;
            if(transform.position.z < endLine) isAtEnd = true;
        } 

        if(isAtEnd){
            if(isHoldingBall){
                if(!matchManager.ballController.isbeingPassed) matchManager.AttackerScored();
                else{
                    matchManager.ballController.isMoving = false;
                    matchManager.isBallHeld = false;
                    isHoldingBall = false;
                    RemoveSoldier();
                }
            }
            else RemoveSoldier();
        }
    }

    private void Update() {
        if(activated) CheckIfReachedEnd();
    }

    private void FixedUpdate() {
        if(!isMoving) return;
        if(isHoldingBall) MoveToTarget(slowSpeed, targetTransform.position);
        else MoveToTarget(fastSpeed, targetTransform.position);
    }
    

}
