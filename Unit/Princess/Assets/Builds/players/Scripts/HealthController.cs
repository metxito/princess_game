using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private LifeBarScript lifeBar;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private Animator animator;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private CharacterGeneralController characterController;


    private float currentHealth = 0f;
    [SerializeField] private SpriteRenderer render;    
    private Material defaultMaterial;

    void Awake(){

        currentHealth = maxHealth;
        characterController = this.gameObject.GetComponent<CharacterGeneralController>();

       
        defaultMaterial = render.material;
        if (defaultMaterial == null){
            Debug.LogError("The Sprite Renderer does not have a material");
        }
    

        if (lifeBar != null)
            lifeBar.SetLife(maxHealth, maxHealth);
    }



    public bool Damage(float amount){
        
        if (currentHealth <= 0f)
            return false;

        currentHealth -= amount;
        if (characterController != null){
            characterController.ResetVelocity();
        }

        if (currentHealth <= 0f){
            currentHealth = 0f;
        }

        if (lifeBar != null)
            lifeBar.SetLife(maxHealth, currentHealth);

        if (currentHealth <= 0f){
            if (animator != null)
                Debug.Log("TODO: is dead");
                //animator.SetBool("isDead", true);
            return true;
        }
        else if (damageMaterial != null) {
            StartCoroutine("ShowDamage");
            GameController.main.FreezeShort();
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
                render.material = damageMaterial;
            }
            else {
                render.material = defaultMaterial;
            }
            yield return new WaitForSeconds(.075f);;
        }
        render.material = defaultMaterial;
    }


}
