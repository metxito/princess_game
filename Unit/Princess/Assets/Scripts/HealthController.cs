using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float MaxHealth = 10;
    public Animator animator;
    public Material damageMaterial;


    private float m_currentHealth = 0f;
    private SpriteRenderer m_render;    
    private Material m_defaultMaterial;

    void Awake(){

        m_currentHealth = MaxHealth;

        m_render = (SpriteRenderer)this.GetComponent<SpriteRenderer>();
        if (m_render == null){
            Debug.LogError("It was not possible to find the Sprite Renderer in this objects");
        }else{
            m_defaultMaterial = m_render.material;
            if (m_defaultMaterial == null){
                Debug.LogError("The Sprite Renderer does not have a material");
            }
        }
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
        else if (damageMaterial != null) {
            StartCoroutine("ShowDamage");
        }

        return false;
    }


    IEnumerator ShowDamage() 
    {
        for (int i=0; i<3; i++) 
        {
            if (i%2 == 0){
                m_render.material = damageMaterial;
            }
            else {
                m_render.material = m_defaultMaterial;
            }
            yield return new WaitForSeconds(.075f);;
        }
        m_render.material = m_defaultMaterial;
    }


}
