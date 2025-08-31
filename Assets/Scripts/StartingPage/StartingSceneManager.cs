using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingSceneManager : MonoBehaviour
{
    [Header("Scene Assets")]
    public Object newCareerScene;
    public Object loadCareerScene;
    public Object quickRaceScene;
    public Object settingsScene;
    
    public void NewCareer()
    {
        Debug.Log("New Career button clicked");
        if (newCareerScene != null)
        {
            Debug.Log("Loading scene: " + newCareerScene.name);
            SceneManager.LoadScene(newCareerScene.name);
        }
        else
        {
            Debug.LogError("New Career scene not assigned!");
        }
    }
    
    public void LoadCareer()
    {
        Debug.Log("Load Career button clicked");
        if (loadCareerScene != null)
        {
            Debug.Log("Loading scene: " + loadCareerScene.name);
            SceneManager.LoadScene(loadCareerScene.name);
        }
        else
        {
            Debug.LogError("Load Career scene not assigned!");
        }
    }
    
    public void QuickRace()
    {
        Debug.Log("Quick Race button clicked");
        if (quickRaceScene != null)
        {
            Debug.Log("Loading scene: " + quickRaceScene.name);
            SceneManager.LoadScene(quickRaceScene.name);
        }
        else
        {
            Debug.LogError("Quick Race scene not assigned!");
        }
    }
    
    public void Settings()
    {
        Debug.Log("Settings button clicked");
        if (settingsScene != null)
        {
            Debug.Log("Loading scene: " + settingsScene.name);
            SceneManager.LoadScene(settingsScene.name);
        }
        else
        {
            Debug.LogError("Settings scene not assigned!");
        }
    }
}