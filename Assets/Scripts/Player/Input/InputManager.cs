using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private RigidbodyCharacterController rigidbodyController;

    [SerializeField] private MouseLook mouseLook;

    PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;

    Vector2 horizontalInput;

    Vector2 mouseInput;

    [SerializeField] private SwordAnimation sword;

    PlayerControls.SwordAttackActions swordAttack;

    void Awake()
    {
        rigidbodyController = GetComponent<RigidbodyCharacterController>();

        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => rigidbodyController.OnJumpPressed();

        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        swordAttack = controls.SwordAttack;
        swordAttack.Swing.performed += _ => sword.OnLMBClicked();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        rigidbodyController.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
