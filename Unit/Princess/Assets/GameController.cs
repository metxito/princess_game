using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController main;
    [SerializeField] [Range(0.1f, 2f)] private float slowTime = 0.5f;
    [SerializeField] [Range(0.1f, 2f)] private float lerpShort = .2f;
    [SerializeField] [Range(0.1f, 2f)] private float lerpMedium = .1f;
    [SerializeField] [Range(0.1f, 2f)] private float lerpLong = .05f;
    private float lerp = 1f;

    private void Awake() {
        main = this;
    }

    private void FixedUpdate() {
        Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, lerp);
    }

    public void FreezeShort()
    {
        Time.timeScale = slowTime;
        lerp = lerpShort;
    }

    public void FreezeMedium()
    {
        Time.timeScale = slowTime;
        lerp = lerpMedium;
    }

    public void FreezeLong()
    {
        Time.timeScale = slowTime;
        lerp = lerpLong;
    }
}
