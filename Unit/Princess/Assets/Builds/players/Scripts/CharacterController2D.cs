using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rigidBody2D;
	[SerializeField] [Range(500, 2000)] private float jumpForce = 800f;			//Amount of force added when the player jumps.
	[SerializeField] [Range(0, 1)] private float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] [Range(0, 1)] private float aitSpeed = .5f;			    // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] [Range(2, 25)] private float waitTimeToSleep = 25f;		// Amount of seconds to wait until start the second idle or sleep time
	[SerializeField] [Range(0, .3f)] private float movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;								// A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D crouchDisableCollider;					// A collider that will be disabled when crouching
	[SerializeField] private float waitingToSleep = 0f;
	[SerializeField] private bool isSleeping = false;
	[System.Serializable] public class BoolEvent : UnityEvent<bool> { }
	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;
	public UnityEvent OnFalling;
	public BoolEvent OnSleepingEvent;
	public BoolEvent OnCrouchEvent;


	const float groundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	private bool grounded = false;            // Whether or not the player is grounded.
	const float ceilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	
	private bool facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 currentVelocity = Vector3.zero;
	private bool wasCrouching = false;
	private bool falling = false;
	private float timeToCheckLanding = 0.01f;
	private float prevPosY = -9999f;





	private void Awake()
	{
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		if (OnFalling == null)
			OnFalling = new UnityEvent();

		if (OnSleepingEvent == null)
			OnSleepingEvent = new BoolEvent();
	}
	

	private void FixedUpdate()
	{
		if (timeToCheckLanding > 0)
			timeToCheckLanding -= Time.fixedDeltaTime;

		if (!falling && prevPosY - transform.position.y > 0.05f){
			falling = true;
			grounded = false;
			waitingToSleep = 0f;
			OnFalling.Invoke();
		}
		prevPosY = transform.position.y;


		if (!grounded && timeToCheckLanding <= 0f){
			
			Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
			foreach(Collider2D col in colliders)
			{
				if (col.gameObject != gameObject)
				{
					grounded = true;
					falling = false;
				}
			}

			if (grounded){
				
				waitingToSleep = 0f;
				OnLandEvent.Invoke();
			} 
		}
	}

	
	void Update()
	{
		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01)
			waitingToSleep = 0f;


		waitingToSleep += Time.deltaTime;

		if (waitingToSleep > waitTimeToSleep && !isSleeping){
			isSleeping = true;
			OnSleepingEvent.Invoke(true);

		}else if (waitingToSleep < waitTimeToSleep && isSleeping) {
			isSleeping = false;
			OnSleepingEvent.Invoke(false);
		}
	}


	


	public void Move(float move, bool crouch, bool jump)
	{
        
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
			{
				waitingToSleep = 0f;
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (grounded || airControl)
		{

			// If crouching
			if (crouch)
			{
				if (!wasCrouching)
				{
					wasCrouching = true;
					waitingToSleep = 0f;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= crouchSpeed;
				if (airControl && !grounded){
					move *= aitSpeed;
				}

				// Disable one of the colliders when crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
			} 
			else
			{
				// Enable the collider when not crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = true;

				if (wasCrouching)
				{
					wasCrouching = false;
					waitingToSleep = 0f;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, rigidBody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref currentVelocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
			{
				// ... flip the player.
				Flip();
			}

			if (Mathf.Abs(move) > .01f )
				waitingToSleep = 0f;
		}

        // If the player should jump...
        if (grounded && jump)
		{
			// Add a vertical force to the player.
			grounded = false;
			rigidBody2D.AddForce(new Vector2(0f, jumpForce));
			timeToCheckLanding = .2f;
			waitingToSleep = 0f;
        }
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}



	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	private void OnDrawGizmosSelected() {
		if (groundCheck != null)
			Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);

		if (ceilingCheck != null)
			Gizmos.DrawWireSphere(ceilingCheck.position, ceilingRadius);
		
	}
}
