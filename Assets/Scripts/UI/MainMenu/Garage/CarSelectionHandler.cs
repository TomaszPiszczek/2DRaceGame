using System.Collections.Generic;
using UnityEngine;

public class CarSelectionHandler : MonoBehaviour
{
    [Header("References")]
    public CarStatsDisplay statsDisplay;
    
    [Header("Car Selection")]
    public int currentCarIndex = 0;
    public int currentTier = 0; // 0 = A-Tier, 1 = B-Tier, 2 = C-Tier
    
    private List<Car> currentTierCars;
    
    void Start()
    {
        LoadCarsForCurrentTier();
        DisplayCurrentCar();
    }
    
    public void NextCar()
    {
        if (currentTierCars != null && currentTierCars.Count > 0)
        {
            currentCarIndex = (currentCarIndex + 1) % currentTierCars.Count;
            DisplayCurrentCar();
        }
    }
    
    public void PreviousCar()
    {
        if (currentTierCars != null && currentTierCars.Count > 0)
        {
            currentCarIndex = (currentCarIndex - 1 + currentTierCars.Count) % currentTierCars.Count;
            DisplayCurrentCar();
        }
    }
    
    public void SelectTier(int tier)
    {
        currentTier = tier;
        currentCarIndex = 0;
        LoadCarsForCurrentTier();
        DisplayCurrentCar();
    }
    
    public void SelectATier() => SelectTier(0);
    public void SelectBTier() => SelectTier(1);
    public void SelectCTier() => SelectTier(2);
    
    private void LoadCarsForCurrentTier()
    {
        switch (currentTier)
        {
            case 0:
                currentTierCars = CarShopList.getATierCars();
                break;
            case 1:
                currentTierCars = CarShopList.getBTierCars();
                break;
            case 2:
                currentTierCars = CarShopList.getCTierCars();
                break;
            default:
                currentTierCars = CarShopList.getATierCars();
                break;
        }
        
        // Ensure we have a valid car index
        if (currentTierCars.Count > 0 && currentCarIndex >= currentTierCars.Count)
        {
            currentCarIndex = 0;
        }
    }
    
    private void DisplayCurrentCar()
    {
        if (currentTierCars != null && currentTierCars.Count > 0 && statsDisplay != null)
        {
            Car currentCar = currentTierCars[currentCarIndex];
            statsDisplay.DisplayCarStats(currentCar);
            
            // Save the selected car ID for the spawn system
            int globalCarIndex = GetGlobalCarIndex(currentCar);
            PlayerPrefs.SetInt("P1SelectedCarID", globalCarIndex);
        }
        else if (statsDisplay != null)
        {
            statsDisplay.ClearDisplay();
        }
    }
    
    public Car GetCurrentCar()
    {
        if (currentTierCars != null && currentTierCars.Count > 0)
        {
            return currentTierCars[currentCarIndex];
        }
        return null;
    }
    
    private int GetGlobalCarIndex(Car car)
    {
        // Get all cars and find the index of the current car
        List<Car> allCars = new List<Car>();
        allCars.AddRange(CarShopList.getATierCars());
        allCars.AddRange(CarShopList.getBTierCars());
        allCars.AddRange(CarShopList.getCTierCars());
        allCars.AddRange(CarShopList.getDTierCars());
        allCars.AddRange(CarShopList.getETierCars());
        
        for (int i = 0; i < allCars.Count; i++)
        {
            if (allCars[i].Name == car.Name)
            {
                return i;
            }
        }
        
        return 0; // Default to first car if not found
    }
}