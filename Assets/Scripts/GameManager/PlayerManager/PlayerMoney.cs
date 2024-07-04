using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerMoney : MonoBehaviour
{
    public TMP_Text moneyText;
    

    public void Start()
    {
         if (Player.Instance != null)
        {
            moneyText.text = "Money : " + Player.Instance.money;
        }
         else
         {
            Debug.LogError("PLAYER IS NULL");
         }
    }


    


   public void UpdateMoney(int amount)
    {
        Player.Instance.money += amount;
        moneyText.text = "Money: " + Player.Instance.money;
    }

    public void addMoney(){
        Player.Instance.money += 4000;
        UpdateMoney(0);
    }

    
}
