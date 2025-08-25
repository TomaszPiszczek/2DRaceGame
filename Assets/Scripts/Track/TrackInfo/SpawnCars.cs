using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            int playerSelectedCarID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");

            foreach (CarData cardata in carDatas)
            {
                if (cardata.CarUniqueID == playerSelectedCarID)
                {
                    GameObject playerCar = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    playerCar.GetComponent<CarInputHandler>().playerNumber =0;
                    
                    // Find the corresponding Car data from JSON and apply stats
                    Car selectedCar = GetCarByID(playerSelectedCarID);
                    if (selectedCar != null)
                    {
                        CarStatsApplier statsApplier = playerCar.GetComponent<CarStatsApplier>();
                        if (statsApplier == null)
                        {
                            statsApplier = playerCar.AddComponent<CarStatsApplier>();
                        }
                        statsApplier.SetCarData(selectedCar);
                    }

                    CarAIHandler aiHandler = playerCar.GetComponent<CarAIHandler>();
                    if (aiHandler != null)
                    {
                        Debug.Log("AI Handler initialized for player " + (i + 1));
                        aiHandler.aiMode = CarAIHandler.AIMode.followWaypoints;

                        StartCoroutine(DelayedInitialization(aiHandler));
                    }
                    else
                    {
                        Debug.LogError("AI Handler is missing for player " + (i + 1));
                    }

                    break;
                }
            }
        }
    }

    private IEnumerator DelayedInitialization(CarAIHandler aiHandler)
    {
        yield return new WaitForSeconds(0.1f);
        aiHandler.InitializeAI();
    }
    
    private Car GetCarByID(int carID)
    {
        // Get all cars from the shop list and find the one matching the ID
        List<Car> allCars = new List<Car>();
        allCars.AddRange(CarShopList.getATierCars());
        allCars.AddRange(CarShopList.getBTierCars());
        allCars.AddRange(CarShopList.getCTierCars());
        allCars.AddRange(CarShopList.getDTierCars());
        allCars.AddRange(CarShopList.getETierCars());
        
        // For now, use the car index as ID (this might need adjustment based on your ID system)
        if (carID >= 0 && carID < allCars.Count)
        {
            return allCars[carID];
        }
        
        Debug.LogWarning($"Car with ID {carID} not found, using default car");
        return allCars.Count > 0 ? allCars[0] : null;
    }
}
