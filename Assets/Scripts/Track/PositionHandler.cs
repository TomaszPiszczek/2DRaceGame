using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{

    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    void Start()
    {


        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        foreach(CarLapCounter lapCounter in carLapCounters)
        {
            lapCounter.OnPassCheckpoint+= OnPassCheckpoint;
        }
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
       carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointPassed()).ThenBy(s => s.getTimeAtLastCheckpoint() ).ToList();

       int carPostion = carLapCounters.IndexOf(carLapCounter) +1;

       carLapCounter.SetCarPosition(carPostion);

       Debug.Log("CAR POSITION" + carPostion + " TIME " + carLapCounters[carPostion-1].getTimeAtLastCheckpoint());
    }

    
}
