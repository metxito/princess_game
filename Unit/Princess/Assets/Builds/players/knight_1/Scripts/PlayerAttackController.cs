using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private EdgeCollider2D attackCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private float force = 3f;
    [SerializeField] private float attackPower = 1f;
    [SerializeField] private ParticleSystem AttackEffect;


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsFalling") && !animator.GetBool("IsJumping"))
        {
            animator.SetTrigger("Attack");
        }

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (attackCollider.enabled){
            if (col.tag == "Enemy"){

                Vector3 colPosition = col.transform.position;
                Vector3 q = colPosition - this.transform.position;
                
                colPosition.z -= 2;
                q.y = (q.y < 0f ? 0f : q.y);
                
                col.transform.Translate(q.normalized * force);
                HealthIAController hc = col.GetComponent<HealthIAController>();
                if (hc != null){
                    hc.Damage(attackPower);
                    GameController.main.FreezeShort();
                }

                ParticleSystem ae = GameObject.Instantiate(AttackEffect, colPosition, col.transform.rotation);
                GameObject.Destroy(ae.gameObject, .51f);

            }
        }
    }
}
