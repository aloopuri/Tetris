
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Scoring system used is the Original BPS Scoring System
// https://tetris.wiki/Scoring
public class Score : MonoBehaviour
{
    public TextMeshProUGUI currentScore;

    private int[] scoreValues = new int[] {40, 100, 300, 1200};

    void Start() {
        currentScore.SetText("0");
        
    }

    // clear lines at board.clearlines()
    public void UpdateScore(int lines) {
        // string temp = currentScore.text;
        int temp = int.Parse(currentScore.text);
        temp += scoreValues[lines-1];
        currentScore.SetText(temp.ToString());
    }
}
