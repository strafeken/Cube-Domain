using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeHeartMovement : MonoBehaviour
{
    [SerializeField] private Transform topLeftHeart;
    [SerializeField] private Transform topRightHeart;
    [SerializeField] private Transform bottomLeftHeart;
    [SerializeField] private Transform bottomRightHeart;

    [SerializeField] private Transform topPosition;
    [SerializeField] private Transform bottomPosition;

    [SerializeField] private Transform front;
    [SerializeField] private Transform back;
    [SerializeField] private Transform center;


    private float rotationSpeed = 100f;
    private float jumpingRotationSpeed = 240f;

    private BigSlime slime;

    void Awake()
    {
        slime = GetComponentInParent<BigSlime>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch(slime.GetCurrentState())
        {
            case BigSlime.State.IDLE:
                topPosition.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
                bottomPosition.Rotate(-Vector3.up * (rotationSpeed * Time.deltaTime));
                break;
            case BigSlime.State.CHASE:
                break;
            case BigSlime.State.JUMPING:
                center.Rotate(Vector3.right * (jumpingRotationSpeed * Time.deltaTime));
                break;
            case BigSlime.State.SHOOT:
                break;
            case BigSlime.State.ATTACK:
                break;
            case BigSlime.State.DEAD:
                break;
        }
    }
}
