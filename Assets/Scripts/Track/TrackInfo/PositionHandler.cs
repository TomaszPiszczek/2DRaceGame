using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    
    void Start()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();
        
        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

        foreach(CarLapCounter lapCounter in carLapCounters)
        {
            lapCounter.OnPassCheckpoint += OnPassCheckpoint;
            lapCounter.OnLapCompleted += OnLapCompleted;
            lapCounter.OnRaceFinished += OnRaceFinished;
        }
        
        if(leaderboardUIHandler != null)
        {
            leaderboardUIHandler.UpdateList(carLapCounters);
        }
    }
    
    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        CalculatePositions();
        
        if(leaderboardUIHandler != null)
        {
            leaderboardUIHandler.UpdateList(carLapCounters);
        }
    }
    
    void OnLapCompleted(CarLapCounter carLapCounter)
    {
        CalculatePositions();
        Debug.Log($"{carLapCounter.gameObject.name} completed lap {carLapCounter.GetLapsCompleted()}");
        
        if(leaderboardUIHandler != null)
        {
            leaderboardUIHandler.UpdateList(carLapCounters);
        }
    }
    
    void OnRaceFinished(CarLapCounter carLapCounter)
    {
        CalculatePositions();
        Debug.Log($"{carLapCounter.gameObject.name} finished the race!");
        
        if (carLapCounter.isPlayerCar)
        {
            RaceManager raceManager = FindObjectOfType<RaceManager>();
            if (raceManager != null)
            {
                raceManager.OnPlayerFinishedRace(carLapCounter.carPostion, carLapCounter.totalRaceTime);
            }
        }
        
        if(leaderboardUIHandler != null)
        {
            leaderboardUIHandler.UpdateList(carLapCounters);
        }
    }
    
    private void CalculatePositions()
    {
        carLapCounters = carLapCounters
            .OrderByDescending(s => s.GetLapsCompleted())
            .ThenByDescending(s => s.GetNumberOfCheckpointPassed())
            .ThenBy(s => s.getTimeAtLastCheckpoint())
            .ToList();

        for (int i = 0; i < carLapCounters.Count; i++)
        {
            carLapCounters[i].SetCarPosition(i + 1);
        }
    }
    
    public List<CarLapCounter> GetFinalResults()
    {
        return carLapCounters
            .Where(car => car.isRaceFinished)
            .OrderBy(car => car.totalRaceTime)
            .ToList();
    }
}
