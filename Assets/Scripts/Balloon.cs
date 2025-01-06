using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public int type;

    float sign;
    float t;
    float o;
    float speed = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        sign = 1f;
        if (Random.Range(0f, 1f) > 0.5f) sign = -1f;

        transform.position = new Vector3(Random.Range(-15f, 15f), Random.Range(-8f, 4f), 800f);

        if (name.Contains("stripes")) type = 0;
        else type = 1;

        if (type == 0)
        {
            // up-down
            t = transform.position.x;
            o = transform.position.y;
        }
        else
        {
            // left-right
            t = transform.position.y;
            o = transform.position.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == 0)
        {
            // up-down
            float y = sign * 6f * Mathf.Sin((2f * Time.time) - (Mathf.PI * 0.5f) + o) - 2f;
            transform.position = new Vector3(t, y, transform.position.z);
        }
        else
        {
            // left-right
            float x = sign * 15f * Mathf.Cos((0.5f * Time.time) - (Mathf.PI * 0.5f) + o);
            transform.position = new Vector3(x, t, transform.position.z);
        }
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

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetType(int t)
    {
        type = t;
    }
}
