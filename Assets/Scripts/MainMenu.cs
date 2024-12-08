
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame() //Created by Bryan
    {
        SceneManager.LoadSceneAsync(1);
    }
}
