using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitializeTrackRows : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform panel; 

    void Start()
    {
        // List<TrackSchedule> tracks = GenerateSeasonSchedule.GetTrackSchedules();
        // foreach (TrackSchedule trackSchedule in tracks)
        // {
        //     AddTrackRow(trackSchedule);
        // }
    }

    void AddTrackRow(TrackSchedule trackSchedule)
    {
        GameObject newRow = Instantiate(rowPrefab, panel);

        TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            Debug.Log("INITIALIZED" + trackSchedule.Track.trackName);
            if (text.name == "TrackName")
            {
                text.text = trackSchedule.Track.trackName;
            }
            if (text.name == "RaceWeek")
            {
                text.text = trackSchedule.RaceWeek.ToString();
            }
            if(!trackSchedule.Finished) continue;
             if (text.name == "Result")
            {
                text.text = trackSchedule.Result.ToString();
            }

             if (text.name == "RivalResult")
            {
                text.text =  trackSchedule.RivalResult.ToString();
            }
            if (text.name == "TeamResult")
            {
                text.text =  trackSchedule.TeamResult.ToString();
            }

            
        }
        }
        
}
