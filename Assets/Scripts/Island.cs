using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public float speed = 1f;
    //Vector3 origin;
    float y_origin;
    //float time = 0.01f;

    // Start is called before the first frame update
    void Awake()
    {
        //origin = transform.position;
        y_origin = transform.position.y;
        transform.position = new Vector3(Random.Range(-360f, 360f), y_origin, 800f);
        if (Mathf.PerlinNoise(transform.position.x, transform.position.y) < 0.5f) transform.position = new Vector3(Random.Range(-360f, 360f), y_origin, 800f);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + Random.Range(-10f, 10f), 0f);
        if (Random.Range(0f, 1f) > 0.5f) transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.forward * speed;

        if (transform.position.z < -20f)
        {
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
#else
            Destroy(gameObject);
#endif
        }
    }
}
