using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeMovement : MonoBehaviour
{

    public CharacterController2D ccontroler;
    public Animator animator;

    public float runSpeed = 10f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crunch = false;
    private float prev_y = -9999f;
    //private bool landing = false;

    // Update is called once per frame
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

        //if (!animator.GetBool("Landing") && prev_y - transform.position.y > 0.5)
        //{
        //    Debug.Log("prev_y: " + prev_y.ToString() + "    position.y: " + transform.position.y.ToString() + "     dif: " + (prev_y - transform.position.y).ToString());
        //    animator.SetBool("Landing", true);
        //    animator.SetBool("IsJumping", false);
        //}
        //else 
        //    animator.SetBool("Landing", false);

        prev_y = transform.position.y;
        ccontroler.Move(horizontalMove * Time.fixedDeltaTime, crunch, jump);
        jump = false;
    }

    public void OnLanding()
    {
        //Debug.Log("landing");
        animator.SetBool("IsJumping", false);        
    }

    public void OutCrunched(bool crunched)
    {
        animator.SetBool("IsCrunched", crunched);
    }
}
