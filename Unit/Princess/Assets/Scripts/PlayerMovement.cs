using UnityEngine;

public class PlayerMovement : MonoBehaviour, CharacterGeneralController
{
    [SerializeField] private CharacterController2D m_controller;
    [SerializeField] private Animator m_animator;

    [Range(0.01f, 30f)] [SerializeField] private float runSpeed = 10f;
    [Range(0.01f, 1f)] [SerializeField] private float accelerationPower = .1f;
    [Range(0.01f, 1f)] [SerializeField] private float breakPower = .1f;

    private float m_horizontalMove = 0f;
    private bool m_jump = false;
    private bool m_crunch = false;
    private float m_currentSpeed = 0f;
    private float m_currentMaxSpeed = 0f;

    
    private void Awake()
    {
        m_currentMaxSpeed = runSpeed;
        m_currentSpeed = 0f;
    }


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
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_currentMaxSpeed, accelerationPower);
        }else if (m_horizontalMove < -0.01){
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, -1 * m_currentMaxSpeed, accelerationPower);
        }else if (Mathf.Abs(m_currentSpeed) < 0.03){
            m_currentSpeed = 0f;
        }else{
            m_currentSpeed = Mathf.Lerp(m_currentSpeed, 0f, breakPower);
        }
        
        m_controller.Move(m_currentSpeed * Time.fixedDeltaTime, m_crunch, m_jump);
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

    public void ReduceSpeed(){
        m_currentMaxSpeed = 0.5f * runSpeed;
        if (m_currentSpeed > m_currentMaxSpeed){
            m_currentSpeed = m_currentMaxSpeed;
        }
    }

    public void RestoreSpeed(){
        m_currentMaxSpeed = runSpeed;
    }

    public void ResetVelocity(){}
}
