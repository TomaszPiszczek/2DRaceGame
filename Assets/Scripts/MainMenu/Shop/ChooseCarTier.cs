using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCarTier : MonoBehaviour
{
    public GameObject ChooseTierPanel;
    public GameObject CarShopTemplate;
    public PageInitialization pageInitialization;


    public GameObject ATier;
    public GameObject BTier;
    public GameObject CTier;
    public GameObject DTier;
    public GameObject ETier;




    public void  ATierAction() => ActivateTier(0);
    public void  BTierAction() => ActivateTier(1);
    public void  CTierAction() => ActivateTier(2);
    public void  DTierAction() => ActivateTier(3);
    public void  ETierAction() => ActivateTier(4);


   private void ActivateTier(int tier)
    {
        CarShopTemplate.SetActive(true);
        ChooseTierPanel.SetActive(false);

        pageInitialization.LoadCarsForTier(tier);


    }


}
