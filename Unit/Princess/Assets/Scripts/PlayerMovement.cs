using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController2D m_controller;
    [SerializeField] private Animator m_animator;

    [Range(0.01f, 30f)] [SerializeField] private float runSpeed = 10f;
    [Range(0.01f, 1f)] [SerializeField] private float accelerationPower = .1f;
    [Range(0.01f, 1f)] [SerializeField] private float breakPower = .1f;

    private float m_horizontalMove = 0f;
    private bool m_jump = false;
    private bool m_crunch = false;
    private float m_currentVelocity = 0f;

    void Update()
    {
        m_horizontalMove = Input.GetAxisRaw("Horizontal");
        m_animator.SetFloat("Speed", Mathf.Abs(m_horizontalMove * 10));

        if (Input.GetButtonDown("Jump"))
        {
            if (!m_animator.GetBool("Falling")){
                m_jump = true;
                m_animator.SetBool("IsJumping", true);
            }
        }        

        if (Input.GetButtonDown("Crunch"))
        {
            m_crunch = true;
        }
        if (Input.GetButtonUp("Crunch"))
        {
            m_crunch = false;
        }

    }

    void FixedUpdate()
    {
        if (m_horizontalMove > 0.01){
            m_currentVelocity = Mathf.Lerp(m_currentVelocity, runSpeed, accelerationPower);
        }else if (m_horizontalMove < -0.01){
            m_currentVelocity = Mathf.Lerp(m_currentVelocity, -1 * runSpeed, accelerationPower);
        }else if (Mathf.Abs(m_currentVelocity) < 0.03){
            m_currentVelocity = 0f;
        }else{
            m_currentVelocity = Mathf.Lerp(m_currentVelocity, 0f, breakPower);
        }
        
        m_controller.Move(m_currentVelocity * Time.fixedDeltaTime, m_crunch, m_jump);
        m_jump = false;
    }

    public void OnLanding()
    {
        m_animator.SetBool("Falling", false);
        m_animator.SetBool("IsJumping", false);
    }

    public void OutCrunched(bool crunched)
    {
        m_animator.SetBool("IsCrunched", crunched);
    }

    public void OnFalling(){
        if (!m_animator.GetBool("Falling")){
            m_animator.SetBool("Falling", true);
            m_animator.SetBool("IsJumping", false);
        }
    }

    public void OnSleeping(bool sleep){
        m_animator.SetBool("ToSleep", sleep);
    }

   
}
