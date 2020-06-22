using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public Animator Animator;
    public float RangeIn = 10f;
    public float RangeOut = 20f;
    public float Velocity = 2f;
    public float Noise = 0.1f;
    private float NoiseLvl1 = 0f;
    private float NoiseLvl2 = 0f;
    private float NoiseLvl3 = 0f;
    private float NoiseLvl4 = 0f;
    

    bool m_FacingRight = false;
    private CharacterController2D m_player;

    private Vector2 direction = Vector2.zero;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        NoiseLvl1 = Noise * -1f;
        NoiseLvl2 = Noise * -0.5f;
        NoiseLvl3 = Noise * 0.5f;
        NoiseLvl4 = Noise;

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        if (m_player == null){
            Debug.LogError("It was not possible to find the Player");
        }
    }


    

    void FixedUpdate() {
        
        if (direction.x == 0 && direction.y == 0)
        {
            if (Vector2.Distance(transform.position, m_player.transform.position) <= RangeIn)
            {
                Animator.SetBool("attack", true);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, m_player.transform.position) > RangeOut)
            {
                direction = Vector2.zero;
                Animator.SetBool("attack", false);
            }
            else {
                Move();
            }
        }
    }

    private void Move(){
        transform.Translate(new Vector3(direction.x, direction.y, 0f) * Velocity * Time.fixedDeltaTime);
        float r = Random.Range(NoiseLvl1, NoiseLvl4);
        if (NoiseLvl2 <= r && r <= NoiseLvl3){
            transform.Translate(new Vector3(0f, 1f, 0f) * r );
        }
        FlipCharacter();
    }

    private void FlipCharacter(){
        if (direction.x > 0 && transform.localScale.x < 0){
            Vector3 s = transform.localScale;
            s.x = 1;
            transform.localScale = s;
        }
        else if (direction.x < 0 && transform.localScale.x > 0){
            Vector3 s = transform.localScale;
            s.x = -1;
            transform.localScale = s;
        }
    }


    public void set_new_direction()
    {
        Vector2 vplayer = new Vector2(m_player.transform.position.x, m_player.transform.position.y);
        Vector2 vthis = new Vector2(transform.position.x, transform.position.y);
        direction = vplayer - vthis;
        direction = direction.normalized;
    }
}
