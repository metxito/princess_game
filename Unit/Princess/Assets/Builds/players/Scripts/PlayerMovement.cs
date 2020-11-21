using UnityEngine;

public class PlayerMovement : MonoBehaviour, CharacterGeneralController
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private CharacterController2D playerController;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(0.01f, 30f)] private float maxSpeed = 10f;
    [SerializeField] [Range(0.01f,  1f)] private float accelerationPower = .175f;
    [SerializeField] [Range(0.01f,  1f)] private float breakPower = .3f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crunch = false;
    [SerializeField] private float currentMaxSpeed = 0f;
    [SerializeField] private float currentSpeed = 0f;
    

    
    private void Awake()
    {
        currentMaxSpeed = maxSpeed;
        currentSpeed = 0f;
    }


    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove * 10));

        if (Input.GetButtonDown("Jump"))
        {
            if (!animator.GetBool("IsFalling") && !animator.GetBool("IsJumping")){
                jump = true;
                animator.SetBool("IsJumping", true);
                animator.SetTrigger("Jump");
            }
        }        

        if (Input.GetButtonDown("Crunch"))
        {
            crunch = true;
        }
        if (Input.GetButtonUp("Crunch"))
        {
            crunch = false;
        }

    }

    void FixedUpdate()
    {
        if (horizontalMove > 0.01){
            currentSpeed = Mathf.Lerp(currentSpeed, currentMaxSpeed, accelerationPower);
        }else if (horizontalMove < -0.01){
            currentSpeed = Mathf.Lerp(currentSpeed, -1 * currentMaxSpeed, accelerationPower);
        }else if (Mathf.Abs(currentSpeed) < 0.03){
            currentSpeed = 0f;
        }else{
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, breakPower);
        }
        
        playerController.Move(currentSpeed * Time.fixedDeltaTime, crunch, jump);
        jump = false;

        if (animator.GetBool("IsFalling")){
            float fallingDistance = rigidBody.velocity.y * Time.fixedDeltaTime;
            if (fallingDistance < 0f){
                fallingDistance = animator.GetFloat("FallingDistance") - fallingDistance;
                animator.SetFloat("FallingDistance", fallingDistance);
            }
        }
        
    }

    public void OnLanding()
    {
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsJumping", false);
    }

    public void OutCrunched(bool crunched)
    {
        animator.SetBool("IsCrunched", crunched);
    }

    public void OnFalling(){
        if (!animator.GetBool("IsFalling")){
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
            animator.SetFloat("FallingDistance", 0f);
            animator.SetTrigger("Fallout");
        }
    }

    public void OnSleeping(bool sleep){
        animator.SetBool("ToSleep", sleep);
    }

    public void ReduceSpeed(){
        currentMaxSpeed = 0.1f * maxSpeed;
        currentSpeed = 0;
    }

    public void RestoreSpeed(){
        currentMaxSpeed = maxSpeed;
    }    

    public void ResetVelocity(){}
}
