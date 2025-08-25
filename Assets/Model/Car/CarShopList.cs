using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CarShopList
{
    private static List<Car> cars;

    static CarShopList()
    {
        loadCarsFromJson();
    }

    private static void loadCarsFromJson()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Database/CarShop/cars");
        if (jsonTextAsset != null)
        {
            cars = JsonUtility.FromJson<CarsWrapper>(jsonTextAsset.text).cars;

           
        }
        else
        {
            Debug.LogError("Failed to load cars.json file!");
            cars = new List<Car>();
        }
    }
    public static List<Car> GetCarsByTier(int tier)
    {
        return cars.FindAll(car => car.Tier == tier).OrderBy(car => car.Price).ToList();
    }
    public static List<Car> getATierCars()
    {
        return cars.FindAll(car => car.Tier == 0).OrderBy(car => car.Price).ToList();
    }

    public static List<Car> getBTierCars()
    {
        return cars.FindAll(car => car.Tier == 1).OrderBy(car => car.Price).ToList();
    }

    public static List<Car> getCTierCars()
    {
        return cars.FindAll(car => car.Tier == 2).OrderBy(car => car.Price).ToList();
    }
     public static List<Car> getDTierCars()
    {
        return cars.FindAll(car => car.Tier == 3).OrderBy(car => car.Price).ToList();
    }
     public static List<Car> getETierCars()
    {
        return cars.FindAll(car => car.Tier == 4).OrderBy(car => car.Price).ToList();
    }
}

[System.Serializable]
public class CarsWrapper
{
    public List<Car> cars;
}
