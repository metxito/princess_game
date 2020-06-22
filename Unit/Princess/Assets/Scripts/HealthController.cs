using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float MaxHealth = 10;
    public Animator animator;
    public Material material;


    private float m_currentHealth = 0f;
    

    void Awake()
    {
        m_currentHealth = MaxHealth;
    }

    public void setDamageMaterial(float val){
        Debug.Log(val);
        material.SetFloat("_Damage", val);
    }


    public bool Damage(float amount){
        

        if (m_currentHealth <= 0)
            return false;

        m_currentHealth -= amount;

        if (m_currentHealth <= 0){
            m_currentHealth = 0;
            
            if (animator != null)
                animator.SetBool("isDead", true);

            return true;
        }
        else {

            if (animator != null){
                animator.SetTrigger("getDamage");
            }
            
        }

        return false;
    }

}
