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
        ccontroler.Move(horizontalMove * Time.fixedDeltaTime, crunch, jump);
        jump = false;

    }

    public void OnLanding()
    {
        Debug.Log("atterrizo");
        animator.SetBool("IsJumping", false);
    }
}
