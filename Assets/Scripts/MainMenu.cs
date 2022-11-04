using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Value of the game board scene
    public readonly int tetrisGameScene = 1;

    public void StartGame() {
        SceneManager.LoadScene(tetrisGameScene);
    }
}
