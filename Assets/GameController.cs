using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public BalloonController balloonController;
    public CoinController coinController;
    public IslandsController islandsController;
    public CloudController cloudController;
    public MouseFollower mouseFollower;

    public AnimationCurve curve;
    public CanvasGroup cg;
    public CanvasGroup gameOverCG;

    public TMP_Text fpsCounter;

    bool isPlaying = false;
    bool isGameOver = false;
    float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            t += Time.deltaTime;
            if (t > 1f) isPlaying = false;
            if (cg.alpha > 0f) cg.alpha = 1f - curve.Evaluate(t);
            if (gameOverCG.alpha > 0f) gameOverCG.alpha = 1f - curve.Evaluate(t);
        }

        if (isGameOver)
        {
            t += Time.deltaTime;
            if (t > 1f)
            {
                isGameOver = false;
                gameOverCG.interactable = true;
                gameOverCG.blocksRaycasts = true;
            }
            gameOverCG.alpha = curve.Evaluate(t);
        }
        fpsCounter.text = $"FPS:{1f / Time.deltaTime}";
    }

    public void OnClick()
    {
        t = 0f;
        islandsController.SetPlaying(true);
        islandsController.SetSpeed(1f);
        cloudController.SetPlaying(true);
        cloudController.SetSpeed(1f);
        balloonController.SetPlaying(true);
        balloonController.SetSpeed(1f);
        balloonController.SetChance(1f);
        coinController.SetPlaying(true);
        coinController.SetSpeed(1f);
        mouseFollower.SetPlaying(true);
        mouseFollower.ResetStats();
        cg.interactable = false;
        cg.blocksRaycasts = false;
        gameOverCG.interactable = false;
        gameOverCG.blocksRaycasts = false;
        isPlaying = true;
    }

    public void OnGameOver()
    {
        t = 0f;
        isPlaying = false;
        isGameOver = true;
        islandsController.SetPlaying(false);
        cloudController.SetPlaying(false);
        balloonController.SetPlaying(false);
        balloonController.Clear();
        coinController.SetPlaying(false);
        coinController.Clear();
        mouseFollower.SetPlaying(false);
    }
}
