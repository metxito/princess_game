using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour, CharacterGeneralController
{
    [SerializeField] private Animator m_animator;
    [Range(0.01f, 50f)] [SerializeField] private float m_rangeIn = 10f;
    [Range(0.1f, 50f)] [SerializeField] private float m_rangeOut = 20f;
    [Range(0.1f, 30f)] [SerializeField] private float m_velocity = 2f;
    [Range(0.01f, 1f)] [SerializeField] private float m_accelerationPower = .1f;
    [Range(0.1f, 5f)] [SerializeField] private float m_noise = 0.1f;

    private float m_noiseLvl1 = 0f;
    private float m_noiseLvl2 = 0f;
    private float m_noiseLvl3 = 0f;
    private float m_noiseLvl4 = 0f;
    

    //bool m_FacingRight = false;
    private CharacterController2D m_player;
    private float m_currentVelocity = 0f;
    private Vector2 m_direction = Vector2.zero;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>



    void Awake()
    {
        m_noiseLvl1 = m_noise * -1f;
        m_noiseLvl2 = m_noise * -0.5f;
        m_noiseLvl3 = m_noise * 0.5f;
        m_noiseLvl4 = m_noise;

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        if (m_player == null){
            Debug.LogError("It was not possible to find the Player");
        }



    }


    

    void FixedUpdate() {
        
        if (m_direction.x == 0 && m_direction.y == 0)
        {
            if (Vector2.Distance(transform.position, m_player.transform.position) <= m_rangeIn)
            {
                m_animator.SetBool("attack", true);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, m_player.transform.position) > m_rangeOut)
            {
                m_direction = Vector2.zero;
                m_animator.SetBool("attack", false);
            }
            else {
                Move();
            }
        }
    }

    private void Move(){
        if (m_direction.magnitude > 0.01f){
            m_currentVelocity = Mathf.Lerp(m_currentVelocity, m_velocity, m_accelerationPower);
        } else if (m_direction.magnitude < -0.01f){
            m_currentVelocity = Mathf.Lerp(m_currentVelocity, -1 * m_velocity, m_accelerationPower);
        } else {
            m_currentVelocity = 0f;   
        }


        transform.Translate(new Vector3(m_direction.x, m_direction.y, 0f) * m_currentVelocity * Time.fixedDeltaTime);
        float r = Random.Range(m_noiseLvl1, m_noiseLvl4);
        if (m_noiseLvl2 <= r && r <= m_noiseLvl3){
            transform.Translate(new Vector3(0f, 1f, 0f) * r );
        }
        FlipCharacter();
    }

    private void FlipCharacter(){
        if (m_direction.x > 0 && transform.localScale.x < 0){
            Vector3 s = transform.localScale;
            s.x = 1;
            transform.localScale = s;
        }
        else if (m_direction.x < 0 && transform.localScale.x > 0){
            Vector3 s = transform.localScale;
            s.x = -1;
            transform.localScale = s;
        }
    }


    public void SetNewDirection()
    {
        Vector2 vplayer = new Vector2(m_player.transform.position.x, m_player.transform.position.y);
        Vector2 vthis = new Vector2(transform.position.x, transform.position.y);
        m_direction = vplayer - vthis;
        m_direction = m_direction.normalized;
    }


    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, m_rangeIn);
        Gizmos.color = new Color(1, 0, 1, 0.75F);
        Gizmos.DrawWireSphere(transform.position, m_rangeOut);
    }

    public void ResetVelocity(){
        m_currentVelocity = 0f;
        Debug.Log("asdasd");
    }
}
