using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreDispalyController : MonoBehaviour {
    public GameController GameController;

    public Text ScoreText;
    public Text TimeleftText;
    public Text LevelText;

    public string ScoreFormat;
    public string TimeLeftFormat;
    public string LevelTextFormat;

    void Update() {
        UpdateScoreboardText();
    }

    public void UpdateScoreboardText() {
        int seconds = (int)GameController.TimeLeft;
        int milliseconds = (int)((GameController.TimeLeft % 1) * 100);
        ScoreText.text = String.Format(ScoreFormat, GameController.Score);
        TimeleftText.text = String.Format(TimeLeftFormat, seconds, milliseconds);
        LevelText.text = String.Format(LevelTextFormat, GameController.Level);
    }
}


// .ToString() + " pioints";
    