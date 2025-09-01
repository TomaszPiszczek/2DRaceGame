using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Configuration")]
    public int startingMoney = 10000;
    public int defaultGarageSize = 10;
    
    
    private Player currentPlayer;
    private string saveFilePath;
    private List<MoneyDisplay> moneyDisplays = new List<MoneyDisplay>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Application.persistentDataPath + "/playerdata.json";
            LoadOrCreatePlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    private void LoadOrCreatePlayer()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(json);
                
                Garage garage = new Garage(saveData.garageSize, saveData.ownedCars, new List<CarPart>());
                currentPlayer = new Player(saveData.money, garage);
                currentPlayer.currentCarIndex = saveData.currentCarIndex;
                
                Debug.Log($"Player data loaded. Money: {currentPlayer.money}, Cars: {currentPlayer.garage.cars.Count}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load player data: {e.Message}");
                CreateNewPlayer();
            }
        }
        else
        {
            CreateNewPlayer();
        }
    }
    
    private void CreateNewPlayer()
    {
        Garage garage = new Garage(defaultGarageSize, new List<Car>(), new List<CarPart>());
        currentPlayer = new Player(startingMoney, garage);
        currentPlayer.currentCarIndex = -1;
        
        Debug.Log($"New player created with {startingMoney} money");
        SavePlayer();
    }
    
    public void SavePlayer()
    {
        try
        {
            PlayerSaveData saveData = new PlayerSaveData
            {
                money = currentPlayer.money,
                garageSize = currentPlayer.garage.size,
                ownedCars = currentPlayer.garage.cars,
                currentCarIndex = currentPlayer.currentCarIndex
            };
            
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Player data saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }
    
    public bool CanAffordCar(Car car)
    {
        return currentPlayer.money >= car.Price;
    }
    
    public bool OwnsCarAlready(Car car)
    {
        return currentPlayer.garage.cars.Exists(ownedCar => ownedCar.Name == car.Name);
    }
    
    public bool BuyCar(Car car)
    {
        if (!CanAffordCar(car))
        {
            Debug.Log($"Not enough money to buy {car.Name}. Need: {car.Price}, Have: {currentPlayer.money}");
            return false;
        }
        
        if (OwnsCarAlready(car))
        {
            Debug.Log($"Already own {car.Name}");
            return false;
        }
        
        currentPlayer.money -= car.Price;
        currentPlayer.garage.addCar(car);
        
        if (currentPlayer.currentCarIndex == -1)
        {
            currentPlayer.currentCarIndex = 0;
        }
        
        Debug.Log($"Purchased {car.Name} for {car.Price}. Remaining money: {currentPlayer.money}");
        
        UpdateMoneyDisplay();
        SavePlayer();
        return true;
    }
    
    public void AddMoney(int amount)
    {
        currentPlayer.money += amount;
        Debug.Log($"Added {amount} money. Total: {currentPlayer.money}");
        UpdateMoneyDisplay();
        SavePlayer();
    }
    
    public void SetCurrentCar(int carIndex)
    {
        if (carIndex >= 0 && carIndex < currentPlayer.garage.cars.Count)
        {
            currentPlayer.currentCarIndex = carIndex;
            Debug.Log($"Current car set to: {GetCurrentCar()?.Name}");
            SavePlayer();
        }
    }
    
    public Car GetCurrentCar()
    {
        if (currentPlayer.currentCarIndex >= 0 && currentPlayer.currentCarIndex < currentPlayer.garage.cars.Count)
        {
            return currentPlayer.garage.cars[currentPlayer.currentCarIndex];
        }
        return null;
    }
    
    public List<Car> GetOwnedCars()
    {
        return currentPlayer.garage.cars;
    }
    
    public int GetMoney()
    {
        return currentPlayer.money;
    }
    
    public void RegisterMoneyDisplay(MoneyDisplay display)
    {
        Debug.Log($"RegisterMoneyDisplay called with display: {display}");
        if (!moneyDisplays.Contains(display))
        {
            moneyDisplays.Add(display);
            Debug.Log($"MoneyDisplay registered. Total displays: {moneyDisplays.Count}");
            display.UpdateDisplay();
        }
    }
    
    public void UnregisterMoneyDisplay(MoneyDisplay display)
    {
        moneyDisplays.Remove(display);
    }
    
    public void UpdateMoneyDisplay()
    {
        Debug.Log($"UpdateMoneyDisplay called. Registered displays: {moneyDisplays.Count}");
        moneyDisplays.RemoveAll(display => display == null);
        foreach (MoneyDisplay display in moneyDisplays)
        {
            display.UpdateDisplay();
        }
    }
    
    public void ResetPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        CreateNewPlayer();
        UpdateMoneyDisplay();
        Debug.Log("Player data reset");
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public int money;
    public int garageSize;
    public List<Car> ownedCars;
    public int currentCarIndex;
}