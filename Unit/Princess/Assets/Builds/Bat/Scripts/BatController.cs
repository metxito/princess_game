using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour, CharacterGeneralController
{
    [SerializeField] private Animator animator;
    [Range(0.01f, 50f)] [SerializeField] private float rangeAttack = 6.5f;
    [Range(0.1f, 50f)] [SerializeField] private float rangeSleep = 7.5f;
    [Range(0.1f, 30f)] [SerializeField] private float maxVelocity = 3f;
    [Range(0.01f, 1f)] [SerializeField] private float acceleration = .05f;
    [Range(0.1f, 5f)] [SerializeField] private float noiseMovement = 0.2f;



    private float noiseMovementLvl1 = 0f;
    private float noiseMovementLvl2 = 0f;
    private float noiseMovementLvl3 = 0f;
    private float noiseMovementLvl4 = 0f;
    

    //bool m_FacingRight = false;
    private CharacterController2D playerController;
    private float currentVelocity = 0f;
    private Vector2 directionMovement = Vector2.zero;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>



    void Awake()
    {
        noiseMovementLvl1 = noiseMovement * -1f;
        noiseMovementLvl2 = noiseMovement * -0.5f;
        noiseMovementLvl3 = noiseMovement * 0.5f;
        noiseMovementLvl4 = noiseMovement;

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        if (playerController == null){
            Debug.LogError("It was not possible to find the Player");
        }



    }


    

    void FixedUpdate() {
        
        if (directionMovement.x == 0 && directionMovement.y == 0)
        {
            if (Vector2.Distance(transform.position, playerController.transform.position) <= rangeAttack)
            {
                animator.SetBool("attack", true);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, playerController.transform.position) > rangeSleep)
            {
                directionMovement = Vector2.zero;
                animator.SetBool("attack", false);
            }
            else {
                Move();
            }
        }
    }

    private void Move(){
        if (directionMovement.magnitude > 0.01f){
            currentVelocity = Mathf.Lerp(currentVelocity, maxVelocity, acceleration);
        } else if (directionMovement.magnitude < -0.01f){
            currentVelocity = Mathf.Lerp(currentVelocity, -1 * maxVelocity, acceleration);
        } else {
            currentVelocity = 0f;   
        }


        transform.Translate(new Vector3(directionMovement.x, directionMovement.y, 0f) * currentVelocity * Time.fixedDeltaTime);
        float r = Random.Range(noiseMovementLvl1, noiseMovementLvl4);
        if (noiseMovementLvl2 <= r && r <= noiseMovementLvl3){
            transform.Translate(new Vector3(0f, 1f, 0f) * r );
        }
        FlipCharacter();
    }

    private void FlipCharacter(){
        if (directionMovement.x > 0 && transform.localScale.x < 0){
            Vector3 s = transform.localScale;
            s.x = 1;
            transform.localScale = s;
        }
        else if (directionMovement.x < 0 && transform.localScale.x > 0){
            Vector3 s = transform.localScale;
            s.x = -1;
            transform.localScale = s;
        }
    }


    public void SetNewDirection()
    {
        Vector2 vplayer = new Vector2(playerController.transform.position.x, playerController.transform.position.y);
        Vector2 vthis = new Vector2(transform.position.x, transform.position.y);
        directionMovement = vplayer - vthis;
        directionMovement = directionMovement.normalized;
    }


    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
        Gizmos.color = new Color(1, 0, 1, 0.75F);
        Gizmos.DrawWireSphere(transform.position, rangeSleep);
    }

    public void ResetVelocity(){
        currentVelocity = 0f;
        Debug.Log("asdasd");
    }
}
