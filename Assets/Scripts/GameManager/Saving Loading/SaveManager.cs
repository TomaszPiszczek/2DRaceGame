using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;
[System.Serializable]

//SAVING PLAYER / LOADING PLAYER DATA 
public static class SaveManager
{
    public static void SavePlayer(Player player, int slot)
    {
        if (player == null)
        {
            Debug.LogError("Player is null");
            return;
        }
        if (slot < 1 || slot > 3)
        {
            Debug.LogError("Invalid slot number! Slot number should be between 1 and 3.");
            return;
        }

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player_slot" + slot + ".dat";
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, player);
            }
            Debug.Log("Data saved: " + player.money);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
    }

    public static void NewPlayerSave(int slot)
    {
        if (slot < 1 || slot > 3)
        {
            Debug.LogError("Invalid slot number! Slot number should be between 1 and 3.");
            return;
        }

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player_slot" + slot + ".dat";
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                Garage garage = new Garage(1, new List<Car>(), new List<CarPart>());
                Player player = new Player(0, garage);
                formatter.Serialize(stream, player);
            }
            Debug.Log("New Player save created");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to create new player save: " + e.Message);
        }
    }

    public static void LoadPlayer(int slot)
    {
        if (slot < 1 || slot > 3)
        {
            Debug.LogError("Invalid slot number! Slot number should be between 1 and 3.");
            return;
        }

        string path = Application.persistentDataPath + "/player_slot" + slot + ".dat";
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    Player player = formatter.Deserialize(stream) as Player;
                    Player.Instance = player;
                }
                Debug.Log("Player successfully loaded from slot: " + slot);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load player data: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    public static bool DataExists(int slot)
    {
        string path = Application.persistentDataPath + "/player_slot" + slot + ".dat";
        return File.Exists(path);
    }
}