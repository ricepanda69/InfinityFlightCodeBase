using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Coin : MonoBehaviour
{
    public AnimationCurve curve;
    bool isCollected = false;
    float speed = 0.25f;

    void Awake()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = 540f * Time.time;
        if (angle > 360f) angle -= 360f;
        transform.rotation = Quaternion.Euler(-90f, angle, 0f);
        transform.position -= Vector3.forward * speed;

        if (isCollected)
        {

        }

        if (transform.position.z < -10f)
        {
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
#else
            Destroy(gameObject);
#endif
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isCollected = true;
        if (other.transform.name == "floatplane")
        {
            //print("coin get");
        }
        else
        {

        }
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
