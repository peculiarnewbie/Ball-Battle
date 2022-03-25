using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    bool isPickedUp;

    Rigidbody rb;

    public Transform targetTransform;
    float speed;
    bool isMoving;

    MatchManager matchManager;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        matchManager = MatchManager.Instance;
        isMoving = false;
    }

    private void Update() {
        if(isMoving) MoveToTarget();
    }

    private void MoveToTarget(){
        Vector3 targetPosition = targetTransform.position - transform.position;
        targetPosition = targetPosition.normalized;

        rb.MovePosition(transform.position + targetPosition * Time.deltaTime * speed * matchManager.gameSpeedMultiplier);
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("wha");
        if(matchManager.isBallHeld) return;
        if(other.collider.CompareTag("Soldier")){
            Attackers attacker = other.gameObject.GetComponent<Attackers>();
            if(attacker != null) return;
            targetTransform = transform;
            matchManager.ChangeBallPickup(true);
            isMoving = true;
        }
    }


}
