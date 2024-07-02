using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
[System.Serializable]
public static class SaveManager 
{
  
    
    public static void SavePlayer(Player player, int slot)
    {
        if(Player.Instance == null)
        {
            Debug.LogError("Player is null");
            return;
        }
        if (slot < 1 || slot > 3)
        {
            Debug.LogError("Invalid slot number! Slot number should be between 1 and 3.");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        
        string path = Application.persistentDataPath + "/player_slot" + slot + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        PlayerData data = new PlayerData(Player.Instance);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Data saved" + data.money);
    }

    public static void newPlayerSave( int slot)
    {
        if (slot < 1 || slot > 3)
        {
            Debug.LogError("Invalid slot number! Slot number should be between 1 and 3.");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        
        string path = Application.persistentDataPath + "/player_slot" + slot + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        Garage garage = new Garage(1,new List<Car>{},new List<CarPart>{});
        Player player = new Player(0,garage);
        PlayerData playerData = new PlayerData(player);
        formatter.Serialize(stream, playerData);
        stream.Close();
        Debug.Log("New Player save created");
       
    }



    public static void LoadPlayer(int slot)
    {
        if (slot < 1 || slot > 3)
        {
            Debug.LogError("Invalid slot number! Slot number should be between 1 and 3.");
            
        }

        string path = Application.persistentDataPath + "/player_slot" + slot + ".txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            Player player = new Player(data.money,data.garage);

            Debug.Log("Player sucessfully loaded from file" + player.money);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    public static bool dataExists(int slot){
        string path = Application.persistentDataPath + "/player_slot" + slot + ".txt";

        return File.Exists(path);
    }



}
