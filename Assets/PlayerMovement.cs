using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
	public float speed = 5f;
	public float jumpHeight = 1f;
	public float gravity = -9.81f;
	public float sprintFactor = 3;

	private float jumpVelocity;
	private float verticalVelocity = 0;
	private InputAction moveAction;
	private InputAction jumpAction;
	private InputAction sprintAction;
	private CharacterController controller;
	private InputAction lookAction;
	public Transform cameraTransform;

	public float mouseSensitivity = 0.1f;
	private float xRotation = 0f;
	public Vector3 Position => transform.position;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		jumpVelocity = Mathf.Sqrt(2f * -gravity * jumpHeight);
		moveAction = InputSystem.actions.FindAction("Move");
		jumpAction = InputSystem.actions.FindAction("Jump");
		lookAction = InputSystem.actions.FindAction("Look");
		sprintAction = InputSystem.actions.FindAction("Sprint");
		controller = GetComponent<CharacterController>();
    }

	// Update is called once per frame
	void Update()
	{
		if (Time.timeScale == 0f) return;

		Vector2 moveValue = moveAction.ReadValue<Vector2>();
		bool jumpValue = jumpAction.IsPressed();
		bool sprintValue = sprintAction.IsPressed();
		Vector2 lookValue = lookAction.ReadValue<Vector2>();

		float actualSpeed = sprintValue ? speed * sprintFactor : speed;

		Vector3 movement = transform.right * moveValue.x * actualSpeed + transform.forward * moveValue.y * actualSpeed;

		float mouseX = lookValue.x * mouseSensitivity;
		float mouseY = lookValue.y * mouseSensitivity;

		// Rotate player left/right
		transform.Rotate(Vector3.up * mouseX);

		// Rotate camera up/down
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		if (controller.isGrounded && jumpValue)
		{
			verticalVelocity = 2f;
		}
		else if (controller.isGrounded && verticalVelocity < 0)
		{
			verticalVelocity = -2f;
		}
		verticalVelocity += gravity * Time.deltaTime;
		movement += new Vector3(0, verticalVelocity, 0);

		controller.Move(movement * Time.deltaTime);
    }
}