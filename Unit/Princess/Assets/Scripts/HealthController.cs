using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float MaxHealth = 10;
    public ParticleSystem BloodPS;
    private float CurrentHealth = 0f;


    void Awake()
    {
        CurrentHealth = MaxHealth;
    }



    public bool Damage(float amount){
        

        if (CurrentHealth <= 0)
            return false;

        CurrentHealth -= amount;
        BloodPS.Play();

        if (CurrentHealth <= 0){
            CurrentHealth = 0;
            return true;
        }

        return false;
    }

}
