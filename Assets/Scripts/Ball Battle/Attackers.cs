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
    }

    public override void ActivateSoldier(){
        targetTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        activated = true;
        isMoving = true;
    }

    public override void BallPickupSwitch(bool value){
        if(value) targetTransform = goalTarget;
        else targetTransform = ballTransform;
    }

    private void FixedUpdate() {
        if(!isMoving) return;
        if(isSpeedy) MoveToTarget(fastSpeed);
        else MoveToTarget(slowSpeed);
    }

    

}
