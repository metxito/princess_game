using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight1AttackController : MonoBehaviour
{
    [SerializeField] private GameObject attackPoint;
    [SerializeField] [Range(0, 30)] private float attackPower = 1f;
    [SerializeField] [Range(0, 4)] private float force = 0f;
    [SerializeField] private AttackController attack1Object;
    [SerializeField] private AttackController attack2Object;
    [SerializeField] private AttackController attack3Object;
    [SerializeField] private Animator animator;

    private CharacterController2D cc;

    public void RestartAttack(){
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
    }
    private void Awake() {
        cc = this.GetComponent<CharacterController2D>();
    }
    // Start is called before the first frame update
    private void Update() {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("IsJumping") && !animator.GetBool("IsFalling"))
        {
            if (!animator.GetBool("Attack1")){
                animator.SetBool("Attack1", true);
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack3", false);
                animator.SetTrigger("Attack");
                
            }else if (!animator.GetBool("Attack2")){
                animator.SetBool("Attack1", true);
                animator.SetBool("Attack2", true);
                animator.SetBool("Attack3", false);
            }else{
                animator.SetBool("Attack1", true);
                animator.SetBool("Attack2", true);
                animator.SetBool("Attack3", true);
            }
            
            
        }
    }

    private void Attack1(){
        GameObject go = GameObject.Instantiate(attack1Object.gameObject);
        go.transform.position = attackPoint.transform.position;
        AttackController ac = go.GetComponent<AttackController>();
        ac.Attack(attackPower, force, cc.isFacingRight());
    }
    private void Attack2(){
        GameObject go = GameObject.Instantiate(attack2Object.gameObject);
        go.transform.position = attackPoint.transform.position;
        AttackController ac = go.GetComponent<AttackController>();
        ac.Attack(attackPower, force, cc.isFacingRight());
    }
    private void Attack3(){
        GameObject go = GameObject.Instantiate(attack3Object.gameObject);
        go.transform.position = attackPoint.transform.position;
        AttackController ac = go.GetComponent<AttackController>();
        ac.Attack(attackPower, force, cc.isFacingRight());
    }

}
