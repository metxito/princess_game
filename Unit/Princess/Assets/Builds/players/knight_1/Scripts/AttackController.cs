using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private ParticleSystem AttackEffect;

    public float factorAttackPower = 1f;
    private float attackPower = 0f;
    private float force = 0f;
    private int counting = 2;



    
    public void Attack(float attackPower, float force, bool lookRigth){
        this.attackPower = attackPower;
        this.force = force;

        if (!lookRigth){
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        attackCollider.enabled = true;

    }


    private void FixedUpdate() {
        counting--;
        if (counting <= 0){
            attackCollider.enabled = false;
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {

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

    
    public void Finish()
    {
        GameObject.Destroy(this.gameObject);
    }
}
