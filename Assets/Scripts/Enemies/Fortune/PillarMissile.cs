using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMissile : MonoBehaviour
{
    private bool faceTarget;
    private Vector3 targetPosition;
    [SerializeField] private Transform player;
    private bool isMoving;

    void Update()
    {
        if (!faceTarget)
            return;

        if (isMoving)
            return;

        transform.up = (player.position - transform.position).normalized;
    }

    public void FaceTarget(Vector3 target)
    {
        faceTarget = true;
        targetPosition = target;
    }

    public void Move(Vector3 target, float moveSpeed)
    {
        StartCoroutine(MissileMovement(target, moveSpeed));
    }

    private IEnumerator MissileMovement(Vector3 target, float moveSpeed)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
