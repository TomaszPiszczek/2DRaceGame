using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToSaveScene : MonoBehaviour
{
    public GameObject SaveGameCanvas;
    public GameObject StartingPageCanvas;




    public void switchScenes()
    {
        if(StartingPageCanvas.activeInHierarchy == true)
        {
            StartingPageCanvas.SetActive(false);
            SaveGameCanvas.SetActive(true);
        }else{
           StartingPageCanvas.SetActive(true);
            SaveGameCanvas.SetActive(false);
        }
    }


}
