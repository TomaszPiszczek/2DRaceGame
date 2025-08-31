using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    public GameObject raceFinishedUI;
    public TMP_Text finalPositionText;
    public TMP_Text rewardText;
    
    SetLeaderboardItemInfo[] setLeaderboardItemInfo;

    void Start()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        setLeaderboardItemInfo = new SetLeaderboardItemInfo[carLapCounterArray.Length];

        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();
            setLeaderboardItemInfo[i].SetPositionText($"{i+1}.");
        }
        
        if (raceFinishedUI != null)
        {
            raceFinishedUI.SetActive(false);
        }
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count && i < setLeaderboardItemInfo.Length; i++)
        {
            if (setLeaderboardItemInfo[i] != null)
            {
                setLeaderboardItemInfo[i].SetPositionText($"{i+1}.");
                setLeaderboardItemInfo[i].SetDriverNameText(lapCounters[i].gameObject.name);
                
                string lapInfo = $"Lap {lapCounters[i].GetLapsCompleted()}/3";
                if (lapCounters[i].isRaceFinished)
                {
                    lapInfo = $"FINISHED - {lapCounters[i].totalRaceTime:F2}s";
                }
                
                TMP_Text timeText = setLeaderboardItemInfo[i].GetComponent<TMP_Text>();
                if (timeText != null)
                {
                    timeText.text = lapInfo;
                }
            }
        }
    }
    
    public void ShowRaceFinishedUI(int playerPosition, int reward)
    {
        if (raceFinishedUI != null)
        {
            raceFinishedUI.SetActive(true);
            
            if (finalPositionText != null)
            {
                string positionSuffix = GetPositionSuffix(playerPosition);
                finalPositionText.text = $"{playerPosition}{positionSuffix} Place!";
            }
            
            if (rewardText != null)
            {
                rewardText.text = $"Reward: ${reward:N0}";
            }
        }
    }
    
    private string GetPositionSuffix(int position)
    {
        return position switch
        {
            1 => "st",
            2 => "nd", 
            3 => "rd",
            _ => "th"
        };
    }
}
