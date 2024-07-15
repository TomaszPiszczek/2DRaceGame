using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarLapCounter : MonoBehaviour
{

    public TextMeshProUGUI carPostionText;
    int passedCheckPointNumber =0;
    float timeAtLastPassedCheckPoint;

    int numberOfPassedCheckpoints =0;


    int lapsCompleted =0;
    const int lapsToComplete =3;

    int carPostion = 0;

    public event Action<CarLapCounter> OnPassCheckpoint;

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
        void OnTriggerEnter2D(Collider2D colider) {
        if (colider.CompareTag("CheckPoint"))
        {
            CheckPoint checkPoint = colider.GetComponent<CheckPoint>();

            if(passedCheckPointNumber +1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber  = checkPoint.checkPointNumber;

                numberOfPassedCheckpoints++;

                timeAtLastPassedCheckPoint = Time.time;


                if(checkPoint.isFinsihLine)
                {
                    passedCheckPointNumber=0;
                    lapsCompleted++;
                }
                if(lapsCompleted > lapsToComplete){
                    Debug.Log("FISNISH");
                   
                }

                OnPassCheckpoint?.Invoke(this);

               if(lapsCompleted > lapsToComplete)
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
}
