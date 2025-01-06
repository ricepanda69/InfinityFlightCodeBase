using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class MouseFollower : MonoBehaviour
{
    public float offset = 3.229432f * 0.25f;
    public float moveSpeed = 0.05f;
    public TMP_Text scoreText;
    public TMP_Text distanceText;

    public Image[] hearts;
    public GameObject whiteSmoke;
    public GameObject graySmoke;
    public GameObject fire;

    public MeshRenderer planeBody;
    public MeshRenderer propeller;
    public GameController gameController;

    public AnimationCurve shakeCurve;
    public AnimationCurve crashCurve;

    float distance = 0f;
    int score = 0;
    bool isHit = false;
    float hitTime = 0f;
    int iFrame = 0;
    int health = 3;

    bool isPlaying = false;

    Vector3 previousPosition;
    Camera mainCamera;
    float speed = 2f;

    public void SetPlaying(bool playing)
    {
        isPlaying = playing;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        speed += Time.deltaTime / 100f;
        if (speed > 4f) speed = 4f;
        distance += speed;
        distanceText.text = $"{(distance / 1000f):F2}km";

        float time = Time.time;
        float t = (Mathf.PerlinNoise(time, time) - 0.5f) * Mathf.PI;
        float r = Mathf.PerlinNoise(time, time) - 0.5f;

        Vector2 mPos = Mouse.current.position.value;
        Vector3 pos = new Vector3(mPos.x, mPos.y, 20);
        Vector3 newPos = Vector3.Lerp(previousPosition, pos, moveSpeed);
        Vector3 origin = mainCamera.ScreenToWorldPoint(newPos);
        previousPosition = newPos;

        float x = origin.x + (r * Mathf.Cos(t));
        float y = origin.y + (r * Mathf.Sin(t));

        transform.position = new Vector3(x, y - offset, 0);
        transform.rotation = Quaternion.Euler(0, 0, 10f * Mathf.Sin(t));

        if (isHit)
        {
            hitTime += Time.deltaTime;
            if (hitTime > 2.5f) isHit = false;
            else if (iFrame % 8 == 0)
            {
                iFrame = 0;
                planeBody.enabled = !planeBody.enabled;
                propeller.enabled = !propeller.enabled;
            }
            iFrame++;
        }
        else
        {
            if (!planeBody.enabled) planeBody.enabled = true;
            if (!propeller.enabled) propeller.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaying) return;

        if (other.name.Contains("Coin"))
        {
            score += 1;
            scoreText.text = score.ToString();
            other.enabled = false;
        }
        else
        {
            if (!isHit)
            {
                if (health == 0)
                {
                    gameController.OnGameOver();
                    StartCoroutine(CrashPlane());
                }
                else
                {
                    StartCoroutine(ApplyCameraShake());
                    isHit = true;
                    hitTime = 0f;
                    iFrame = 0;
                    health -= 1;
                    hearts[health].color = new Color(0.5f, 0.5f, 0.5f);

                    if (health == 2)
                    {
                        whiteSmoke.SetActive(true);
                    }
                    if (health == 1) 
                    {
                        whiteSmoke.SetActive(false);
                        graySmoke.SetActive(true);
                    }
                    if (health == 0)
                    {
                        fire.SetActive(true);
                    }
                }
            }
        }
    }

    IEnumerator ApplyCameraShake()
    {
        Vector3 origin = mainCamera.transform.position;
        for (int i = 0; i < 30; i++)
        {
            float t = i / 30f;
            mainCamera.transform.position = origin + 2f * shakeCurve.Evaluate(t) * new Vector3(Mathf.PerlinNoise1D(t), Mathf.PerlinNoise1D(t), 0f);
            yield return new WaitForEndOfFrame();
        }
        mainCamera.transform.position = origin;
    }

    IEnumerator CrashPlane()
    {
        float x_origin = transform.position.x;
        float y_origin = transform.position.y;
        float z_origin = transform.position.z;
        float z_angle = transform.rotation.eulerAngles.z;
        for (int i = 0; i < 60; i++)
        {
            float t = i / 60f;
            float a = 90f - 90f * crashCurve.Evaluate(t);
            transform.position = new Vector3(x_origin, y_origin - 0.25f * a, z_origin);
            transform.rotation = Quaternion.Euler(2f * a, 0f, z_angle + 2f * a);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ApplyCameraShake());
    }

    public int GetScore()
    {
        return score;
    }

    public float GetDistance()
    {
        return distance / 1000f;
    }

    public void ResetStats()
    {
        speed = 2f;
        distance = 0f;
        distanceText.text = $"{(distance / 1000f):F2}km";
        score = 0;
        scoreText.text = score.ToString();
        health = 3;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        isHit = true;
        hitTime = 0f;
        iFrame = 0;
        hearts[0].color = new Color(1f, 1f, 1f);
        hearts[1].color = new Color(1f, 1f, 1f);
        hearts[2].color = new Color(1f, 1f, 1f);
        whiteSmoke.SetActive(false);
        graySmoke.SetActive(false);
        fire.SetActive(false);
    }
}
