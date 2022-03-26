using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    bool isPickedUp;

    Rigidbody rb;

    public Transform targetTransform;
    float speed = 0.1f;
    bool isMoving;

    MatchManager matchManager;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        matchManager = MatchManager.Instance;
        isMoving = false;
    }

    private void FixedUpdate() {
        if(isMoving) MoveToTarget();
    }

    private void MoveToTarget(){
        float range = (targetTransform.position - transform.position).magnitude;
        Vector3 targetPosition = Vector3.Lerp(transform.position, targetTransform.position, speed* Mathf.Pow(range, 1f/3f));
        // targetPosition = targetPosition.normalized;

        rb.MovePosition(targetPosition);
        // rb.MovePosition(transform.position + targetPosition * Time.deltaTime * speed * matchManager.gameSpeedMultiplier);
    }

    private void OnTriggerEnter(Collider other){
        if(matchManager.isBallHeld) return;
        if(other.CompareTag("Soldier")){
            Attackers attacker = other.gameObject.GetComponent<Attackers>();
            if(attacker == null) return;
            targetTransform = other.transform;
            matchManager.ChangeBallPickup(true);
            isMoving = true;
        }
    }


}
