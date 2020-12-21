using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIAController : MonoBehaviour
{
    public float maxHealth = 10;
    public Animator animator;
    public Material damageMaterial;


    private CharacterGeneralController m_characterController;


    private float m_currentHealth = 0f;
    private SpriteRenderer m_render;    
    private Material m_defaultMaterial;

    public bool IsDead = false;

    void Awake(){

        m_currentHealth = maxHealth;
        m_characterController = this.gameObject.GetComponent<CharacterGeneralController>();

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

        if (m_currentHealth <= 0f)
            return false;

        m_currentHealth -= amount;
        if (m_characterController != null){
            m_characterController.ResetVelocity();
        }


        if (m_currentHealth <= 0f){
            m_currentHealth = 0f;
                
            if (animator != null)
                animator.SetBool("isDead", true);
            Debug.Log("TODO: bat dead");

            return true;
        }
        else if (damageMaterial != null) {
            StartCoroutine("ShowDamage");
        }

        
        return false;
    }

    public void Dead(){
        GameObject.Destroy(this.gameObject);
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
