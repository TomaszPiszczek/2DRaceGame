using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarLapCounter : MonoBehaviour
{
    public TextMeshProUGUI carPostionText;
    public TextMeshProUGUI lapCounterText;
    
    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint;
    float raceStartTime;
    
    int numberOfPassedCheckpoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 3;
    public int carPostion = 0;
    
    public float[] lapTimes = new float[3];
    public float totalRaceTime = 0f;
    public bool isRaceFinished = false;
    public bool isPlayerCar = false;

    public event Action<CarLapCounter> OnPassCheckpoint;
    public event Action<CarLapCounter> OnLapCompleted;
    public event Action<CarLapCounter> OnRaceFinished;

    IEnumerator ShowPostionCO(float delayUntilGHidePostion)
    {
        carPostionText.text = carPostion.ToString();
        carPostionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayUntilGHidePostion);

        carPostionText.gameObject.SetActive(false);
    }

    public void SetCarPosition(int postion)
    {
        carPostion = postion;
    }

    public int GetNumberOfCheckpointPassed()
    {
        return numberOfPassedCheckpoints;
    }

    public float getTimeAtLastCheckpoint()
    {
        return timeAtLastPassedCheckPoint;
    }
        private void Start()
    {
        raceStartTime = Time.time;
        UpdateLapDisplay();
    }
    
    void OnTriggerEnter2D(Collider2D colider) 
    {
        if (colider.CompareTag("CheckPoint"))
        {
            CheckPoint checkPoint = colider.GetComponent<CheckPoint>();

            if(passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassedCheckPoint = Time.time;

                if(checkPoint.isFinsihLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;
                    
                    float lapTime = Time.time - (lapsCompleted == 1 ? raceStartTime : timeAtLastPassedCheckPoint - lapTimes[lapsCompleted - 2]);
                    if (lapsCompleted <= 3)
                    {
                        lapTimes[lapsCompleted - 1] = lapTime;
                    }
                    
                    UpdateLapDisplay();
                    OnLapCompleted?.Invoke(this);
                    
                    Debug.Log($"{gameObject.name} completed lap {lapsCompleted} in {lapTime:F2}s");
                }

                if(lapsCompleted >= lapsToComplete && !isRaceFinished)
                {
                    isRaceFinished = true;
                    totalRaceTime = Time.time - raceStartTime;
                    Debug.Log($"{gameObject.name} FINISHED RACE! Total time: {totalRaceTime:F2}s");
                    OnRaceFinished?.Invoke(this);
                }

                OnPassCheckpoint?.Invoke(this);

                if(isRaceFinished)
                {
                    StartCoroutine(ShowPostionCO(100));
                }
                else
                {
                    StartCoroutine(ShowPostionCO(1.5f));
                }
            }
        }
    }
    
    private void UpdateLapDisplay()
    {
        if (lapCounterText != null)
        {
            lapCounterText.text = $"Lap: {lapsCompleted}/{lapsToComplete}";
        }
    }
    
    public int GetLapsCompleted()
    {
        return lapsCompleted;
    }
    
    public float GetBestLapTime()
    {
        float best = float.MaxValue;
        for (int i = 0; i < lapsCompleted && i < lapTimes.Length; i++)
        {
            if (lapTimes[i] < best) best = lapTimes[i];
        }
        return best == float.MaxValue ? 0f : best;
    }
}
