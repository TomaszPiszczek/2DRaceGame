using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [Header("Scene Configuration")]
    public Object sceneToLoad;
    
    public void LoadScene()
    {
        if (sceneToLoad != null)
        {
            SceneManager.LoadScene(sceneToLoad.name);
        }
        else
        {
            Debug.LogError("Scene to load not assigned!");
        }
    }
    
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}