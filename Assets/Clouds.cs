using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    float speed = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        transform.position = new Vector3(Random.Range(-720f, 720f), 200f + Random.Range(-25f, 50f), 2000f);
        transform.localScale = Vector3.one * Random.Range(20f, 25f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.forward * speed;

        if (transform.position.z < -10f)
        {
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
#else
            Destroy(gameObject);
#endif
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
