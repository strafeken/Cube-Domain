using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private RigidbodyCharacterController rigidbodyController;

    private PlayerControls controls;
    
    private Vector2 walkInput;

    [SerializeField] private MouseLook mouseLook;
    private Vector2 mouseInput;

    [SerializeField] private SwordAnimation sword;
    [SerializeField] private float mouseScrollY;

    void Awake()
    {
        rigidbodyController = GetComponent<RigidbodyCharacterController>();

        controls = new PlayerControls();

        PlayerControls.GroundMovementActions groundMovement = controls.GroundMovement;
        groundMovement.Movement.performed += ctx => walkInput = ctx.ReadValue<Vector2>();

        groundMovement.Jump.performed += rigidbodyController.JumpOnGround;

        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        PlayerControls.SwordAttackActions swordAttack = controls.SwordAttack;
        swordAttack.Swing.performed += _ => sword.OnLMBClicked();
        swordAttack.ChangeMode.performed += ctx => mouseScrollY = ctx.ReadValue<float>();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        rigidbodyController.ReceiveInput(walkInput);
        mouseLook.ReceiveInput(mouseInput);
        sword.ReceiveInput(mouseScrollY);
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
