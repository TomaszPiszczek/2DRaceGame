using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Tworzymy przykładowego gracza na start
            player = new Player(50000, new Garage(10, new List<Car>(), new List<CarPart>()));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddRaceReward(int position)
    {
        int reward = 0;
        switch (position)
        {
            case 1: reward = 30000; break;
            case 2: reward = 10000; break;
            case 3: reward = 5000; break;
            default: reward = 0; break;
        }

        player.money += reward;
        Debug.Log($"Player now has {player.money} money after finishing {position}.");
    }

    public bool BuyCar(Car car)
    {
        if (player.money >= car.Price)
        {
            player.money -= car.Price;
            player.garage.addCar(car);
            Debug.Log($"Bought car {car.Name}. Money left: {player.money}");
            return true;
        }
        else
        {
            Debug.Log("Not enough money to buy car!");
            return false;
        }
    }

    public bool SellCar(Car car, int sellPrice)
    {
        if (player.garage.cars.Contains(car))
        {
            player.garage.cars.Remove(car);
            player.money += sellPrice;
            Debug.Log($"Sold car {car.Name}. Money now: {player.money}");
            return true;
        }
        else
        {
            Debug.Log("Car not in garage!");
            return false;
        }
    }
}