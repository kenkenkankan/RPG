using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public float moveSpeed;
	public float rotateSpeed;
	public float jumpForce = 5f;
	public float gravity;
	public Transform cameraTransform;

	private void Update()
	{
		Movement();
		Rotate();
	}

	void Movement()
	{

		float inputWS;
		float inputAD;
		Vector3 inputWASD;
		inputAD = Input.GetAxis("Horizontal");
		inputWS = Input.GetAxis("Vertical");
		inputWASD = new Vector3(inputAD, 0, inputWS);

		Vector3 moveCameraDirection = cameraTransform.TransformDirection(inputWASD);
		moveCameraDirection.y = 0;

		controller.SimpleMove(moveCameraDirection * moveSpeed * Time.deltaTime);

	}

	void Rotate()
	{
		float inputMouseHorizontal;
		inputMouseHorizontal = Input.GetAxis("Mouse X");

		transform.eulerAngles += new Vector3(0, inputMouseHorizontal * rotateSpeed * Time.deltaTime, 0);
	}
}


//  if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
//{
//	moveDirection.y = jumpPower;
//}
//else
//{
//	moveDirection.y = movementDirectionY;
//}

//if (!characterController.isGrounded)
//{
//	moveDirection.y -= gravity * Time.deltaTime;
//}

//if (Input.GetKey(KeyCode.R) && canMove)
//{
//	characterController.height = crouchHeight;
//	walkSpeed = crouchSpeed;
//	runSpeed = crouchSpeed;

//}
//else
//{
//	characterController.height = defaultHeight;
//	walkSpeed = 6f;
//	runSpeed = 12f;
//}

//characterController.Move(moveDirection * Time.deltaTime);

//if (canMove)
//{
//rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
//rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
//playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
//transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	public Camera playerCamera;
	public float walkSpeed = 6f;
	public float runSpeed = 12f;
	public float jumpPower = 7f;
	public float gravity = 10f;
	public float lookSpeed = 2f;
	public float lookXLimit = 45f;
	public float defaultHeight = 2f;
	public float crouchHeight = 1f;
	public float crouchSpeed = 3f;

	private Vector3 moveDirection = Vector3.zero;
	private float rotationX = 0;
	private CharacterController characterController;

	private bool canMove = true;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);

		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
		float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
		float movementDirectionY = moveDirection.y;
		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
		{
			moveDirection.y = jumpPower;
		}
		else
		{
			moveDirection.y = movementDirectionY;
		}

		if (!characterController.isGrounded)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.R) && canMove)
		{
			characterController.height = crouchHeight;
			walkSpeed = crouchSpeed;
			runSpeed = crouchSpeed;

		}
		else
		{
			characterController.height = defaultHeight;
			walkSpeed = 6f;
			runSpeed = 12f;
		}

		characterController.Move(moveDirection * Time.deltaTime);

		if (canMove)
		{
			rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	public Camera playerCamera;
	public float walkSpeed = 6f;
	public float runSpeed = 12f;
	public float jumpPower = 7f;
	public float gravity = 10f;
	public float lookSpeed = 2f;
	public float lookXLimit = 45f;
	public float defaultHeight = 2f;
	public float crouchHeight = 1f;
	public float crouchSpeed = 3f;

	private Vector3 moveDirection = Vector3.zero;
	private float rotationX = 0;
	private CharacterController characterController;

	private bool canMove = true;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Movement();
		Rotate();
		Jump();
	}

	void Movement()
	{
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);

		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
		float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		characterController.Move(moveDirection * Time.deltaTime);

		if (canMove)
		{
			rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
		}
	}

	void Rotate()
	{
		float inputMouseHorizontal = Input.GetAxis("Mouse X");
		transform.eulerAngles += new Vector3(0, inputMouseHorizontal * lookSpeed * Time.deltaTime, 0);
	}

	void Jump()
	{
		float movementDirectionY = moveDirection.y;


		if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
		{
			moveDirection.y = jumpPower;
		}
		else
		{
			moveDirection.y = movementDirectionY;
		}

		if (!characterController.isGrounded)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}
		if (Input.GetButtonDown("Jump"))
			Debug.Log("Space");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	public Camera playerCamera;
	public float walkSpeed = 6f;
	public float runSpeed = 12f;
	public float jumpPower = 7f;
	public float gravity = 10f;
	public float lookSpeed = 2f;
	public float lookXLimit = 45f;
	public float defaultHeight = 2f;
	public float crouchHeight = 1f;
	public float crouchSpeed = 3f;

	private Vector3 moveDirection = Vector3.zero;
	private float rotationX = 0;
	private CharacterController characterController;

	private bool canMove = true;

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.visible = false;
	}

	void Update()
	{
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);

		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
		float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
		float movementDirectionY = moveDirection.y;
		moveDirection = (forward * curSpeedX) + (right * curSpeedY);

		if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
		{
			moveDirection.y = jumpPower;
		}
		else
		{
			moveDirection.y = movementDirectionY;
		}

		if (!characterController.isGrounded)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.R) && canMove)
		{
			characterController.height = crouchHeight;
			walkSpeed = crouchSpeed;
			runSpeed = crouchSpeed;

		}
		else
		{
			characterController.height = defaultHeight;
			walkSpeed = 6f;
			runSpeed = 12f;
		}

		characterController.Move(moveDirection * Time.deltaTime);

		if (canMove)
		{
			rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
		}
	}
}