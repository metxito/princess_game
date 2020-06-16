using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBackground : MonoBehaviour
{
    public Transform background_01;
    public Transform background_02;

    public float difference_x = 0.5f;
    public float difference_y = 0.3f;

    private Vector3 prevPosition = Vector3.zero;

    private void Awake()
	{
        prevPosition = this.transform.position;
	}

    void FixedUpdate()
    {
        Vector3 difference = this.transform.position - prevPosition;
        Vector3 translation01 = new Vector3(difference.x * difference_x / 2, difference.y * difference_y / 2, difference.z);
        Vector3 translation02 = new Vector3(difference.x * difference_x, difference.y * difference_y, difference.z);

        background_01.Translate(translation01);
        background_02.Translate(translation02);

        


        prevPosition = this.transform.position;
    }
}
