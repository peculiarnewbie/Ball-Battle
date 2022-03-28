using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    bool isPickedUp;
    public bool isbeingPassed;

    Rigidbody rb;

    public Transform targetTransform;
    float speed = 1.5f;
    bool isMoving;

    MatchManager matchManager;

    MeshRenderer rend;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();
        matchManager = MatchManager.Instance;
        isMoving = false;
        isbeingPassed = false;

        SpawnBall();
    }

    private void FixedUpdate() {
        if(isMoving) MoveToTarget();
    }

    private void MoveToTarget(){
        if(isbeingPassed){
            Vector3 targetPosition = targetTransform.position - transform.position;
            if(targetPosition.magnitude <= 1f) isbeingPassed = false;
            targetPosition = targetPosition.normalized;

            rb.MovePosition(transform.position + targetPosition * Time.deltaTime * speed * matchManager.gameSpeedMultiplier);
        }
        else{
            float range = (targetTransform.position - transform.position).magnitude;
            Vector3 targetPosition = Vector3.Lerp(transform.position, targetTransform.position, speed* Mathf.Pow(range, 1f/3f)/10);
        
            rb.MovePosition(targetPosition);
        }
        
    }

    private void OnTriggerEnter(Collider other){
        if(matchManager.isBallHeld) return;
        if(other.CompareTag("Soldier")){
            Attackers attacker = other.gameObject.GetComponent<Attackers>();
            if(attacker == null) return;
            attacker.isHoldingBall = true;
            targetTransform = other.transform;
            matchManager.ChangeBallPickup(true);
            isMoving = true;
        }
    }

    public void SpawnBall(){
        Bounds spawnField;
        if(matchManager.isPlayerAttacking) spawnField = matchManager.playerField.bounds;
        else spawnField = matchManager.enemyField.bounds;
        float x = Random.Range(spawnField.min.x + 1f, spawnField.max.x - 1f);
        float z = Random.Range(spawnField.min.z + 1f, spawnField.max.z - 1f);
        Vector3 position = new Vector3(x, rb.position.y, z);
        rb.MovePosition(position);

        rend.enabled = true;
    }


}
