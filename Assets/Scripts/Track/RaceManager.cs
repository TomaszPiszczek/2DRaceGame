using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    [Header("Race Configuration")]
    public string trackName = "Default Track";
    
    [Header("UI References")]
    public GameObject countdownUI;
    public TMP_Text countdownText;
    public LeaderboardUIHandler leaderboardUIHandler;
    public Button continueButton;
    
    private RaceCompletionManager raceCompletionManager;
    private PositionHandler positionHandler;
    private bool raceStarted = false;
    private bool raceFinished = false;
    
    public static bool IsRaceActive { get; private set; } = false;
    
    void Start()
    {
        raceCompletionManager = GetComponent<RaceCompletionManager>();
        if (raceCompletionManager == null)
        {
            raceCompletionManager = gameObject.AddComponent<RaceCompletionManager>();
        }
        
        positionHandler = FindObjectOfType<PositionHandler>();
        
        StartCoroutine(StartRaceCountdown());
    }
    
    private IEnumerator StartRaceCountdown()
    {
        IsRaceActive = false;
        if (countdownUI != null) countdownUI.SetActive(true);
        
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null) countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        
        if (countdownText != null) countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        
        if (countdownUI != null) countdownUI.SetActive(false);
        
        raceStarted = true;
        IsRaceActive = true;
        Debug.Log("Race Started!");
    }
    
    public void OnPlayerFinishedRace(int position, float totalTime)
    {
        if (raceFinished) return;
        
        raceFinished = true;
        
        if (raceCompletionManager != null)
        {
            string timeString = FormatTime(totalTime);
            raceCompletionManager.OnRaceCompleted(position, trackName, timeString);
        }
        
        int prizeMoney = CalculatePrizeMoney(position);
        
        if (leaderboardUIHandler != null)
        {
            leaderboardUIHandler.ShowRaceFinishedUI(position, prizeMoney);
        }
        
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ReturnToMainMenu);
        }
        
        Debug.Log($"Player finished race in position {position} with time {FormatTime(totalTime)}");
    }
    
    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    private int CalculatePrizeMoney(int position)
    {
        return position switch
        {
            1 => 1000,
            2 => 750,
            3 => 500,
            4 => 300,
            5 => 100,
            _ => 0
        };
    }
    
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);
        
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
    
    
    public bool IsRaceStarted()
    {
        return raceStarted;
    }
    
    public bool IsRaceFinished()
    {
        return raceFinished;
    }
}