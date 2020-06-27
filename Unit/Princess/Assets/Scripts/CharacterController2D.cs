using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 1)] [SerializeField] private float m_AirSpeed = .5f;			    // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(2, 25)] [SerializeField] private float m_sleepTimeWait = 25f;		// Amount of seconds to wait until start the second idle or sleep time
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded = false;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;


	
	[System.Serializable] public class BoolEvent : UnityEvent<bool> { }


	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;
	public UnityEvent OnFalling;
	public BoolEvent OnSleepingEvent;
	public BoolEvent OnCrouchEvent;
	
	
	private bool m_wasCrouching = false;
	private bool m_falling = false;

	[SerializeField] private float m_waitingToSleep = 0f;
	[SerializeField] private bool m_isSleeping = false;


	private float m_timeToCheckLanding = 0.01f;


	private float m_prevPosY = -9999f;
	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		
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
		if (m_timeToCheckLanding > 0)
			m_timeToCheckLanding -= Time.fixedDeltaTime;

		if (!m_falling && m_prevPosY - transform.position.y > 0.05f){
			m_falling = true;
			m_Grounded = false;
			m_waitingToSleep = 0f;
			OnFalling.Invoke();
		}
		m_prevPosY = transform.position.y;



		if (!m_Grounded && m_timeToCheckLanding <= 0f){
			
			//bool wasGrounded = m_Grounded;
			//m_Grounded = false;

			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			foreach(Collider2D col in colliders)
			{
				if (col.gameObject != gameObject)
				{
					m_Grounded = true;
					m_falling = false;
				}
			}

			if (m_Grounded){
				
				m_waitingToSleep = 0f;
				OnLandEvent.Invoke();
			} 
		}

	}

	
	void Update()
	{
		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01)
			m_waitingToSleep = 0f;


		m_waitingToSleep += Time.deltaTime;

		if (m_waitingToSleep > m_sleepTimeWait && !m_isSleeping){
			m_isSleeping = true;
			OnSleepingEvent.Invoke(true);

		}else if (m_waitingToSleep < m_sleepTimeWait && m_isSleeping) {
			m_isSleeping = false;
			OnSleepingEvent.Invoke(false);
		}

	}





	public void Move(float move, bool crouch, bool jump)
	{
        
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				m_waitingToSleep = 0f;
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					m_waitingToSleep = 0f;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;
				if (m_AirControl && !m_Grounded){
					move *= m_AirSpeed;
				}

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} 
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					m_waitingToSleep = 0f;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}

			if (Mathf.Abs(move) > .01f )
				m_waitingToSleep = 0f;
		}

        // If the player should jump...
        if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			m_timeToCheckLanding = .2f;
			m_waitingToSleep = 0f;
        }
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}



	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	private void OnDrawGizmosSelected() {
		if (m_GroundCheck != null)
			Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);

		if (m_CeilingCheck != null)
			Gizmos.DrawWireSphere(m_CeilingCheck.position, k_CeilingRadius);
		
	}
}
