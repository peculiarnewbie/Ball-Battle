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

    private void FixedUpdate() {
        if(!isMoving) return;
        if(isHoldingBall) MoveToTarget(slowSpeed, targetTransform.position);
        else MoveToTarget(fastSpeed, targetTransform.position);
    }
    

}
