using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Scoring system used is the Original BPS Scoring System
// https://tetris.wiki/Scoring
public class Hud : MonoBehaviour
{
    public TextMeshProUGUI currentScore;

    private int[] scoreValues = new int[] {40, 100, 300, 1200};

    void Start() {
        currentScore.SetText("0");
        
    }

    public void AddLineClearedSCore(int linesCleared) {
        if (linesCleared <= 0) {
            return;
        }
        int val = scoreValues[linesCleared - 1];
        UpdateScore(val);
    }

    // public void AddHardDropScore(int rows) {
    //     UpdateScore(rows + 1);
    // }

    public void UpdateScore(int value) {
        int temp = int.Parse(currentScore.text);
        temp += value;
        currentScore.SetText(temp.ToString());
    }
}
