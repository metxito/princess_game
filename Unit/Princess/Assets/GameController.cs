using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController main;
    private float timeSlow = 0f;

    [SerializeField] private float slowTime = 0.25f;

    private void Awake() {
        main = this;
    }

    private void FixedUpdate() {
        if (timeSlow > 0f){
            timeSlow -= Time.fixedDeltaTime;
            if (timeSlow <= 0f)
                Time.timeScale = 1;
        }
    }

    public void FreezeShort()
    {
        Time.timeScale = slowTime;
        timeSlow = .2f;
    }

    public void FreezeMedium()
    {
        Time.timeScale = slowTime;
        timeSlow = .4f;
    }

    public void FreezeLong()
    {
        Time.timeScale = slowTime;
        timeSlow = .6f;
    }
}
