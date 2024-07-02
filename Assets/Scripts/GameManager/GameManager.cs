using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int gameSaveSlot;

   /* private void Awake()
    {
        
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/



  private void Start()
    {
        Debug.Log("Loading player from game manager");
        LoadGame(PlayerPrefs.GetInt("gameSaveSlot"));

        Debug.Log(Player.Instance.money + "MONEY");

        InvokeRepeating("PeriodicSaveCheck", 1f, 1f); 
    }

    private void PeriodicSaveCheck()
    {
        SaveGame();
    }
    public void SaveGame()
    {
        gameSaveSlot = PlayerPrefs.GetInt("gameSaveSlot", 1);
        SaveManager.SavePlayer(Player.Instance, gameSaveSlot);
    }
    public void newPlayerSave(int slot)
    {
        SaveManager.newPlayerSave(slot);
    }

    public void LoadGame(int slot)
    {

         SaveManager.LoadPlayer(slot);

      
    }
    private void OnApplicationQuit()
        {
            Debug.Log("Quitting and saving game" + Player.Instance.money);
            SaveGame();
        }

   
}