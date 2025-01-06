using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System.Security;

public class GameController : MonoBehaviour
{
    [Header("Game Components")]
    public BalloonController balloonController;
    public CoinController coinController;
    public IslandsController islandsController;
    public CloudController cloudController;
    public MouseFollower mouseFollower;

    [Header("UI")]
    public AnimationCurve curve;
    public CanvasGroup cg;
    public CanvasGroup gameOverCG;
    public TMP_Text fpsCounter;
    public GameObject restartButton;
    public GameObject enterButton;
    public GameObject showLeaderboardButton;
    public GameObject backButton;
    public GameObject info;
    public GameObject nameInput;
    public GameObject leaderboard;
    public TMP_Text leaderboardText;
    public TMP_Text infoText;
    public TMP_InputField nameInputField;

    bool isPlaying = false;
    bool isGameOver = false;
    float t = 0f;

    int score;
    float distance;

    List<string> badWords = new List<string>()
    {
        "ass",
        "fuc",
        "fuk",
        "fuq",
        "fux",
        "fck",
        "coc",
        "cok",
        "coq",
        "kox",
        "koc",
        "kok",
        "koq",
        "cac",
        "cak",
        "caq",
        "kac",
        "kak",
        "kaq",
        "dic",
        "dik",
        "diq",
        "dix",
        "dck",
        "pns",
        "psy",
        "fag",
        "fgt",
        "ngr",
        "nig",
        "cnt",
        "knt",
        "sht",
        "dsh",
        "twt",
        "bch",
        "cum",
        "clt",
        "kum",
        "klt",
        "suc",
        "suk",
        "suq",
        "sck",
        "lic",
        "lik",
        "liq",
        "lck",
        "jiz",
        "jzz",
        "gay",
        "gey",
        "gei",
        "gai",
        "vag",
        "vgn",
        "sjv",
        "fap",
        "prn",
        "jew",
        "joo",
        "gvr",
        "pus",
        "pis",
        "pss",
        "snm",
        "tit",
        "fku",
        "fcu",
        "fqu",
        "hor",
        "slt",
        "jap",
        "wop",
        "kik",
        "kyk",
        "kyc",
        "kyq",
        "dyk",
        "dyq",
        "dyc",
        "kkk",
        "jyz",
        "prk",
        "prc",
        "prq",
        "mic",
        "mik",
        "miq",
        "myc",
        "myk",
        "myq",
        "guc",
        "guk",
        "guq",
        "giz",
        "gzz",
        "sex",
        "sxx",
        "sxi",
        "sxe",
        "sxy",
        "xxx",
        "wac",
        "wak",
        "wck",
        "waq",
        "pot",
        "thc",
        "vaj",
        "vjn",
        "nut",
        "std",
        "lsd",
        "poo",
        "azn",
        "pcp",
        "dmn",
        "orl",
        "anl",
        "ans",
        "muf",
        "mff",
        "phk",
        "phc",
        "phq",
        "xtc",
        "tok",
        "toc",
        "toq",
        "mlf",
        "rac",
        "rak",
        "raq",
        "rck",
        "sac",
        "sak",
        "saq",
        "pms",
        "nad",
        "ndz",
        "nds",
        "wtf",
        "sol",
        "sob",
        "fob",
        "sfu"
    };

    string host = "https://api.simpleboards.dev/";
    string key = "d2a060fb-498e-4e99-9b1f-5e7d8d60574d";
    string id = "4dcc6399-5376-486a-631d-08dd2dffaae1";

    [Serializable]
    struct CreateEntry
    {
        public string LeaderboardId;
        public string PlayerId;
        public string PlayerDisplayName;
        public string Score;
        public string Metadata;
    }

    [Serializable]
    struct Entry
    {
        public string Id;
        public string LeaderboardId;
        public string PlayerId;
        public string PlayerDisplayName;
        public string Score;
        public string Metadata;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //client = new HttpClient();
        //client.BaseAddress = new System.Uri("https://api.simpleboards.dev/");
        //client.DefaultRequestHeaders.Add("x-api-key", "d2a060fb-498e-4e99-9b1f-5e7d8d60574d");
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
        islandsController.SetSpeed(2f);
        cloudController.SetPlaying(true);
        cloudController.SetSpeed(2f);
        balloonController.SetPlaying(true);
        balloonController.SetSpeed(2f);
        balloonController.SetChance(1f);
        coinController.SetPlaying(true);
        coinController.SetSpeed(2f);
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
        ShowGameOverScreen();
        score = mouseFollower.GetScore();
        distance = mouseFollower.GetDistance();
        string s = score.ToString();
        string scoreString = ">score:..........." + s.PadLeft(4, '.');
        string d = $"{distance:F2}";
        print(d.Length);
        string distanceString = ">distance:..." + d.PadLeft(7, '.') + "km";
        infoText.text = $"<b>game over</b>\r\n{scoreString}\r\n{distanceString}\r\n\r\n>submit to leaderboard\r\n\r\n\r\n\r\nLOADING...\r\n\r\n<b>click to restart</b>";
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
        StartCoroutine(LoadLeaderboard());
    }

    public void ShowLeaderboard()
    {
        restartButton.SetActive(false);
        enterButton.SetActive(false);
        info.SetActive(false);
        nameInput.SetActive(false);
        showLeaderboardButton.SetActive(false);
        backButton.SetActive(true);
        leaderboard.SetActive(true);
        string s = score.ToString();
        string d = $"{distance:F2}";
        string scoreString = ">score:..........." + s.PadLeft(4, '.');
        string distanceString = ">distance:..." + d.PadLeft(7, '.') + "km";
        infoText.text = $"<b>game over</b>\r\n{scoreString}\r\n{distanceString}\r\n\r\n>submit to leaderboard\r\n\r\n\r\n\r\nLOADING...\r\n\r\n<b>click to restart</b>";
    }

    public void ShowGameOverScreen()
    {
        restartButton.SetActive(true);
        enterButton.SetActive(true);
        info.SetActive(true);
        nameInput.SetActive(true);
        showLeaderboardButton.SetActive(true);
        backButton.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void OnInputFieldEndEdit()
    {
        if (nameInputField.text.Length > 3)
        {
            nameInputField.text = nameInputField.text.Substring(0, 3);
        }
    }

    public void Submit()
    {
        StartCoroutine(SubmitName());
    }

    IEnumerator SubmitName()
    {
        string name = nameInputField.text;
        string s = score.ToString();
        string d = $"{distance:F2}";
        string scoreString = ">score:..........." + s.PadLeft(4, '.');
        string distanceString = ">distance:..." + d.PadLeft(7, '.') + "km";
        if (badWords.Contains(name) || name.Length < 3)
        {
            nameInputField.text = "";
            infoText.text = $"<b>game over</b>\r\n{scoreString}\r\n{distanceString}\r\n\r\n>submit to leaderboard\r\n\r\ninvalid name\r\n\r\nLOADING...\r\n\r\n<b>click to restart</b>";
            yield return null;
        }
        string playerId = CreateMD5(SystemInfo.deviceName + SystemInfo.deviceModel + SystemInfo.deviceType + SystemInfo.graphicsDeviceType + SystemInfo.graphicsDeviceID);
        var newEntry = new CreateEntry
        {
            LeaderboardId = "4dcc6399-5376-486a-631d-08dd2dffaae1",
            PlayerId = UnityEngine.Random.Range(0, 9999).ToString(),//playerId,
            PlayerDisplayName = name,
            Score = s,
            Metadata = $"distance:{d}"
        };
        //var createEntryResponse = await client.PostAsJsonAsync("api/entries", newEntry);
        //createEntryResponse.EnsureSuccessStatusCode();
        //var createdEntry = await createEntryResponse.Content.ReadFromJsonAsync<Entry>();
        //Console.WriteLine($"Entry Created! {createdEntry}");
        using (UnityWebRequest www = UnityWebRequest.Post(host + "api/entries", JsonUtility.ToJson(newEntry), "application/json"))
        {
            //print(JsonUtility.ToJson(newEntry));
            www.SetRequestHeader("x-api-key", key);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Entry submitted successfully.");
            }
        }
        infoText.text = $"<b>game over</b>\r\n{scoreString}\r\n{distanceString}\r\n\r\n>submit to leaderboard\r\n\r\nsubmitted successfully\r\n\r\nLOADING...\r\n\r\n<b>click to restart</b>";
        StartCoroutine(LoadLeaderboard());
    }

    IEnumerator LoadLeaderboard()
    {
        showLeaderboardButton.SetActive(false);
        //var r = await client.GetAsync("api/leaderboards/4dcc6399-5376-486a-631d-08dd2dffaae1/entries");
        //print(r.Content);
        //var entries = await client.GetFromJsonAsync<IEnumerable<Entry>>("api/leaderboards/4dcc6399-5376-486a-631d-08dd2dffaae1/entries");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(host + $"api/leaderboards/{id}/entries"))
        {
            //$"api/leaderboards/{id}/entries"
            webRequest.SetRequestHeader("x-api-key", key);
            yield return webRequest.SendWebRequest();

            List<string> leaderboardEntries = new List<string>();
            print(webRequest.downloadHandler.text);
            try
            {
                List<Entry> entries = JsonConvert.DeserializeObject<List<Entry>>(webRequest.downloadHandler.text);

                foreach (var entry in entries)
                {
                    string d = entry.Metadata.Split(':')[1];
                    string s = entry.Score;
                    leaderboardEntries.Add($">{entry.PlayerDisplayName}   {s.PadLeft(4, ' ')} {d.PadLeft(7, ' ')}km");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError("Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Success.");
                        leaderboardText.text = "name  score  distance\n" + string.Join('\n', leaderboardEntries);
                        break;
                }
                showLeaderboardButton.SetActive(true);
            }
        }
    }

    string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string prior to .NET 5
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}