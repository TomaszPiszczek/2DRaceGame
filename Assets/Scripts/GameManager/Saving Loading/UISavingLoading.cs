using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


//CHANGE THE VIEW OF SAVE LOAD CARRER IF CARRER EXISTS SHOW LOAD BUTTON IF NOT SHOW NEW CARRER BUTTON
public class UISavingLoading : MonoBehaviour
{
    public TMP_Text carrerSlot1;
    public TMP_Text carrerSlot2;
    public TMP_Text carrerSlot3;

 


    public void  SaveLoadGameSlot1() => actionToPerform(1);
    public void  SaveLoadGameSlot2() => actionToPerform(2);
    public void  SaveLoadGameSlot3() => actionToPerform(3);


    private void Awake()
    {
            if(SaveManager.dataExists(1)){
                carrerSlot1.text ="Load game";
            }else
            {
                carrerSlot1.text ="New Carrer"; 
            }

            if(SaveManager.dataExists(2)){
                Debug.Log(SaveManager.dataExists(2));
                carrerSlot2.text ="Load game";
            }else
            {
                carrerSlot2.text ="New Carrer";
            }

            if(SaveManager.dataExists(3)){
                carrerSlot3.text ="Load game";
            }else
            {
                carrerSlot3.text ="New Carrer";
            }
        
    }

    private void actionToPerform(int slot)
    {

        PlayerPrefs.SetInt("gameSaveSlot", slot);
        PlayerPrefs.Save();
        if(SaveManager.dataExists(slot))
        {
            Debug.Log("load game");
            SaveManager.LoadPlayer(slot);
        }else
        {
            Debug.Log("new player");
            SaveManager.newPlayerSave(slot); 
            
        }

        SceneManager.LoadScene("MainMenu");

    }




   
}
