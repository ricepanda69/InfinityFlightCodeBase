using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public GameObject[] balloons;
    float time = 0f;
    float spawnTime = 0f;
    float speed = 2f;
    float spawnChance = 1f;
    bool isPlaying = false;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetPlaying(bool playing)
    {
        isPlaying = playing;
    }

    public void SetChance(float c)
    {
        spawnChance = c;
    }

    public void Clear()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            try
            {
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            }
            catch (System.Exception e)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        float s = speed;
        time += Time.deltaTime;
        spawnTime += Time.deltaTime;
        speed += Time.deltaTime / 100f;
        if (speed > 4f) speed = 4f;

        if (spawnTime > 20f)
        {
            spawnTime = 0f;
            spawnChance -= 0.005f;
            if (spawnChance < 0.975f) spawnChance = 0.975f;
        }

        if (Random.Range(0f, 1f) > spawnChance && time > 0.125f)
        {
            int i = Random.Range(0, balloons.Length);
            GameObject balloon = Instantiate(balloons[i], transform);
            balloon.AddComponent<Balloon>().SetSpeed(speed);
            time = 0f;
        }

        if (s != 4f && transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Balloon>().SetSpeed(speed);
            }
        }
    }
}
