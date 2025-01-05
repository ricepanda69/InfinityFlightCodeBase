using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandsController : MonoBehaviour
{
    public float speed = 1f;
    public GameObject[] islands;

    float time = 0f;
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
        if (isPlaying) speed += Time.deltaTime / 100f;
        if (speed > 2f) speed = 2f;

        time += Time.deltaTime;

        if (Random.Range(0f, 1f) > 0.965f && transform.childCount < 30 && time > 0.15f)
        {
            GameObject island = Instantiate(islands[Random.Range(0, islands.Length)], transform);
            island.AddComponent<Island>().SetSpeed(speed);
            time = 0f;
        }

        if (s != 2f)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Island>().SetSpeed(speed);
            }
        }
    }
}
