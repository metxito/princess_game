using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1.5f)] private float timeScale = 1f;
    private void Awake() {
        Time.timeScale = timeScale;
    }

    private void FixedUpdate() {
        Time.timeScale = timeScale;
    }
}
