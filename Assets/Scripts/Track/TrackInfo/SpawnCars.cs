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
}
