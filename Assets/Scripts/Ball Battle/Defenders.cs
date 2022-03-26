using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defenders : Soldiers
{
    public override void AssignStartValue(){
        slowSpeed = 1f;
        fastSpeed = 2f;
        activationTime = 4f;
    }

    public override void ActivateSoldier(){
        activated = true;
    }

    public override void BallPickupSwitch(bool value){
        if(value) targetTransform = ballTransform;
        else targetTransform = goalTarget;
    }
}