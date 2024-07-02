using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChoosePart : MonoBehaviour
{
    
    public GameObject choosePart;
    public GameObject EngineParts;
 

    public void switchToParts()
    {
        if(choosePart.activeInHierarchy == true)
        {
            choosePart.SetActive(false);
            EngineParts.SetActive(true);


        }else{
            choosePart.SetActive(true);
            EngineParts.SetActive(false);
            
        }
    }

    public void goBack()
    {
        choosePart.SetActive(true);
        EngineParts.SetActive(false);
    }
}
