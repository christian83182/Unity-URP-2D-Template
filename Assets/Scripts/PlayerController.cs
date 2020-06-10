using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//"public" variables are visible in the inspector
	//"private" variables are hidden from anything outside of this file.

	public float runSpeed;                      //The speed the player should run at.
	public float jumpForce;                     //The force with which the player should jump. 
	public LayerMask groundLayers;              //A LayerMask object of the layers which should be considered ground. 
	public Transform groundCheck;               //A transform object who's position will be used to check if the player is grounded. 

	private Rigidbody2D rigidBody;              //The GameObject's rigidbody component.
	private Vector3 velocity = Vector3.zero;    //Used to smooth the character's motion. 


	//Called once, when the component is created.
	private void Start() {
		this.rigidBody = GetComponent<Rigidbody2D>();
	}

	//Called everytime a new frame is rendered.
	private void Update() {
		//The OverlapCircleAll() method will return a list of colliders within some radius.
		//The position of the 'GroundCheck' object is used, and the 'GroundLayers' mask is used so only certain layers are considered:
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayers);
		//If any colliders are found then the player is touching the ground, so store this in a boolean variable:
		bool isGrounded = colliders.Length > 0;

		//If the jump button is pressed and the player is also currently grounded then jump:
		if (Input.GetButtonDown("Jump") && isGrounded) {
			//Do this by applying a vertical force to the object through its RigidBody (use 'JumpForce' to scale the force amount):  
			rigidBody.AddForce(Vector2.up * jumpForce);
		}

		//Input.GetAxis("Horizontal") will return 1.0 when the 'D' key is pressed, and -1.0 when the 'A' key is pressed:
		float horizontalInput = Input.GetAxis("Horizontal");
		//Compute how much the player should be moved by multiplying the movement direction by the run speed:
		float horizontalMovement = horizontalInput * runSpeed;

		Vector3 targetVelocity = new Vector2(horizontalMovement, rigidBody.velocity.y);
		rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, 0.001f);
	}
}
