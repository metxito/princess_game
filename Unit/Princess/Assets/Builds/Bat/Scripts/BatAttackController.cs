using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttackController : MonoBehaviour
{
    [SerializeField] private float attackPower = 1f;
        
    // when the GameObjects collider arrange for this GameObject to travel to the left of the screen
    void OnTriggerEnter2D(Collider2D col)
    {
        
        

        if (col.tag == "Player"){
            
            HealthController hc = col.GetComponent<HealthController>();
            if (hc != null){
                hc.Damage(attackPower);
            }
        }
    }


}
