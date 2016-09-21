using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    int MIN = -15;
    int MAX = 15;
    int SCORE = 0;
    float TIMER = 0.0f;
    bool PLAYING = true;
    public GameObject FirstTaget;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (PLAYING)
        {
            TIMER += Time.deltaTime;
            DisplayTime();
            if (SCORE >= 20)
            {
                EndGame();
            }
        }
    }

    string FormatTimer()
    {
        if (TIMER <= 60.0f)
        {
            return (Mathf.Round(TIMER * 100) / 100).ToString();
        }
        else
        {
            int displayTimeMin = (int)(TIMER / 60.0f);
            float displayTimeSec = Mathf.Round((TIMER % 60) * 10 / 10);
            if (displayTimeSec < 10)
            {
                return displayTimeMin.ToString() + ":0" + displayTimeSec.ToString();
            }
            else
            {
                return displayTimeMin.ToString() + ":" + displayTimeSec.ToString();
            }
        }
    }

    void DisplayTime()
    {
        GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Text>().text = "Time: "
        + FormatTimer()
        + System.Environment.NewLine
        + "Targets hit: " + SCORE + "/20";
    }

    public void NewTarget()
    {
        SCORE++;
        float xVal = Random.Range(MIN, MAX);
        float yVal = Random.Range(2.0f, 5.0f);
        float zVal = Random.Range(MIN, MAX);
        Vector3 TargetPos = new Vector3(xVal, yVal, zVal);
        Instantiate(FirstTaget, TargetPos, FirstTaget.transform.rotation);
    }

    void EndGame()
    {
        PLAYING = false;
        if (!PlayerPrefs.HasKey("BestTime") || PlayerPrefs.GetFloat("BestTime") < TIMER)
        {
            PlayerPrefs.SetFloat("BestTime", TIMER);
            GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Text>().text = "Time: "
            + FormatTimer()
            + System.Environment.NewLine
            + "New High Score!";
        }
    }


}
