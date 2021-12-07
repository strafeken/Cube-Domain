using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionController : MonoBehaviour
{
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        controller.detectCollisions = false;
    }
}
