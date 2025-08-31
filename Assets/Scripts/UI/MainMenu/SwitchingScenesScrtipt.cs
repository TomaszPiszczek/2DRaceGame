using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwitchingScenesScrtipt : MonoBehaviour
{
    
    public GameObject MenuNavigationCanvas;
    public GameObject UICarShop;
    public GameObject CarShopTemplate;
    public GameObject ChooseTierPanel;
 

    public void switchToCarShop()
    {
        if(MenuNavigationCanvas.activeInHierarchy == true)
        {
            MenuNavigationCanvas.SetActive(false);
            UICarShop.SetActive(true);
            CarShopTemplate.SetActive(false);
            ChooseTierPanel.SetActive(true);

        }else{
            MenuNavigationCanvas.SetActive(true);
            UICarShop.SetActive(false);
        }
    }
}
