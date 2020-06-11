using UnityEngine;

public class WormController  : MonoBehaviour
{
    
    public Animator animator = null;
    public float velocity = 1f;


    private HealthController player;
    private Vector2 direction = Vector2.zero;
    private bool attack = false;
    private bool fliped = false;
    private float timeToChangeDirection = 0f;

    void Start()
    {
        player = (GameObject.FindGameObjectWithTag("Player")).GetComponent<HealthController>();
        
    }


    private void Update() {
        transform.Translate(direction * velocity * Time.deltaTime);
        if (animator != null){
            animator.SetFloat("velocity", Mathf.Abs(direction.x * velocity));
            if (!fliped && direction.x < 0){
                flip();
            }
            else if (fliped && direction.x > 0)
            {
                flip();
            }
        }
    }

    private void flip(){
        // Switch the way the player is labelled as facing.
		fliped = !fliped;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }


    void FixedUpdate()
    {
        timeToChangeDirection -= Time.deltaTime;
        if (timeToChangeDirection <= 0 && attack && player != null){
            Vector2 posGusano = new Vector2(transform.position.x, transform.position.y);
            Vector2 posPlayer = new Vector2(player.transform.position.x, player.transform.position.y);
            direction = posPlayer - posGusano;
            direction.y = 0;
            direction = direction.normalized;
            timeToChangeDirection = 1f;
        }
    }

    public void setAttack ()
    {
        this.attack = true;
    } 


}
