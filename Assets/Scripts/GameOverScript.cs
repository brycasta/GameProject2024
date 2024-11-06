using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public GameObject gameOverUI;  // Reference to the actual UI GameObject

    
    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

}
