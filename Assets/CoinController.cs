using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public GameObject coinPrefab;
    float time = 0f;
    float speed = 1f;
    bool isPlaying = false;

    public void SetPlaying(bool playing)
    {
        isPlaying = playing;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void Clear()
    {
        int n = transform.childCount;
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        float s = speed;
        time += Time.deltaTime;
        speed += Time.deltaTime / 100f;
        if (speed > 2f) speed = 2f;

        if (Random.Range(0f, 1f) > 0.995f && time > 1.5f)
        {
            time = 0f;
            float sign = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
            float x0 = Random.Range(0f, 10f);
            int n = (2 * Random.Range(1, 3)) + 1;
            float m = Random.Range(0f, 1f) > 0.5f ? 1f : 0f;
            for (int i = 0; i < n; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform);
                float x = (sign * x0) + (m * -sign * 4f * i);
                if (x < -10f) x = -10f;
                if (x > 10f) x = 10f;
                float z = 800f + (25f * i);
                float y = -sign * 2f * i;
                if (y < -4f) y = -4f;
                if (y > 4f) y = 4f;
                coin.transform.position = new Vector3(x, y, z);
                coin.GetComponent<Coin>().SetSpeed(speed);
                coin.SetActive(true);
            }
        }

        if (s != 2f && transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Coin>().SetSpeed(speed);
            }
        }
    }
}