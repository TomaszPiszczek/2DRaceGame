using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
  public void PlayGame(){
    SceneManager.LoadScene("Track_01");
  }
  public void QuitGame(){
    Application.Quit();
  }
}
