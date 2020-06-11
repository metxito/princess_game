using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controler;
    public Animator animator;

    public float runSpeed = 10f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crunch = false;
    private float prev_y = -9999f;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
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
        prev_y = transform.position.y;
        controler.Move(horizontalMove * Time.fixedDeltaTime, crunch, jump);
        jump = false;
    }

    public void OnLanding()
    {
        animator.SetBool("Falling", false);
        animator.SetBool("IsJumping", false);
    }

    public void OutCrunched(bool crunched)
    {
        animator.SetBool("IsCrunched", crunched);
    }

    public void OnFalling(){
        animator.SetBool("Falling", true);
        animator.SetBool("IsJumping", false);
    }
}
