using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    float speed = 2160f;

    // Update is called once per frame
    void Update()
    {
        float angle = Time.time * speed;
        if (angle > 360f) angle -= 360f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
