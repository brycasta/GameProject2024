
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame() //Created by Bryan
    {
        SceneManager.LoadSceneAsync(1);
    }

  
  //Fanial W.
  public void Settings(){

    SceneManager.LoadSceneAsync(3);
  }
}
