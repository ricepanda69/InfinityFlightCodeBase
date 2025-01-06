using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public GameObject[] clouds;
    float time = 0f;
    float speed = 2f;

    bool isPlaying = false;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    public void SetPlaying(bool playing)
    {
        isPlaying = playing;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    // Update is called once per frame
    void Update()
    {
        float s = speed;

        time += Time.deltaTime;
        if (isPlaying) speed += Time.deltaTime / 100f;
        if (speed > 4f) speed = 4f;

        if (Random.Range(0f, 1f) > 0.99f && transform.childCount < 5 && time > 2f)
        {
            GameObject cloud = Instantiate(clouds[Random.Range(0, clouds.Length)], transform);
            cloud.GetComponent<Clouds>().SetSpeed(speed);
            time = 0f;
        }

        if (s != 4f)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Clouds>().SetSpeed(speed);
            }
        }
    }
}
