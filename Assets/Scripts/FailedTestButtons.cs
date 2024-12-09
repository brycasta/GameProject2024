using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailedTestButtons : MonoBehaviour
{

    public void RestartGame()
    {
        // Reset the last checkpoint
        Checkpoint.lastCheckpointPosition = Vector3.zero;
        SceneManager.LoadSceneAsync(1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
