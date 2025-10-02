using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{ 
    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private bool isGrounded;

    private CharacterController controller;
    private Transform cameraMainTransform;
    private Vector3 moveDirection;
    private Vector3 velocity;

    private float gravityValue = -9.81f;
    private float moveX;
    private float moveZ;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!controller) // Check for controller reference
        {
            Debug.LogError("Did not find CharacterController!");
        }

        cameraMainTransform = Camera.main.transform;

        if (!cameraMainTransform) // Check for camera reference
        {
            Debug.LogError("Did not find Camera!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Jump();
    }

    // Move is the function controls user to move with WASD
    private void Move()
    {
        // isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        isGrounded = controller.isGrounded;

        // Avoid flickering isGrounded on and off
        if (isGrounded && velocity.y < 0)
        {
            // if it sets to zero, it flickers
            // I don't know why this fix would work ...
            velocity.y = -1f;
        }

        // Regular movement with WASD 
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        
        // Add camera movement (mouse movement sensor) to WASD 
        moveDirection = cameraMainTransform.right * moveX + cameraMainTransform.forward * moveZ;
        moveDirection.y = 0f;

        // move only on WASD
        controller.Move(moveDirection * Time.deltaTime * playerSpeed);

        // Rotate the character to face forward (camera sees back)
        Rotate();

    }

    // Jump is the function controls user to jump with space 
    private void Jump() 
    {
        
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityValue);
        }

        // move on jump
        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Rotate is the function controls user to rotate with camera moves
    private void Rotate()
    {
        Vector3 direction = new Vector3(moveX, 0f, moveZ).normalized;

        if (direction.magnitude >= 0.1f) 
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
