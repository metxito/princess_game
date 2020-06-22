using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public EdgeCollider2D attackCollider;
    public Animator animator;
    public float force = 3f;
    public float attackPower = 1f;
    public ParticleSystem AttackEffect;


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("Falling") && !animator.GetBool("IsJumping"))
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
                q.y = q.y / 3;
                
                col.transform.Translate(q.normalized * force);
                HealthController hc = col.GetComponent<HealthController>();
                if (hc != null)
                    hc.Damage(attackPower);

                ParticleSystem ae = GameObject.Instantiate(AttackEffect, colPosition, col.transform.rotation);
                GameObject.Destroy(ae, 1f);

            }
        }
    }
}
