using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RaceResultsList
{
    private static List<RaceResult> raceResults;

    static RaceResultsList()
    {
        LoadRaceResultsFromJson();
    }

    private static void LoadRaceResultsFromJson()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Database/TimeTable/raceResults");
        if (jsonTextAsset != null)
        {
            raceResults = JsonUtility.FromJson<RaceResultsWrapper>(jsonTextAsset.text).raceResults;
        }
        else
        {
            Debug.LogError("Failed to load raceResults.json file!");
            raceResults = new List<RaceResult>();
        }
    }

    public static List<RaceResult> GetAllResults()
    {
        return raceResults.OrderBy(result => result.Week).ToList();
    }

    public static List<RaceResult> GetResultsByWeek(int week)
    {
        return raceResults.FindAll(result => result.Week == week);
    }

    public static List<RaceResult> GetResultsByPosition(int position)
    {
        return raceResults.FindAll(result => result.Position == position).OrderBy(result => result.Week).ToList();
    }

    public static List<RaceResult> GetResultsByCar(string car)
    {
        return raceResults.FindAll(result => result.Car == car).OrderBy(result => result.Week).ToList();
    }

    public static List<RaceResult> GetResultsByMap(string map)
    {
        return raceResults.FindAll(result => result.Map == map).OrderBy(result => result.Week).ToList();
    }
}

[System.Serializable]
public class RaceResultsWrapper
{
    public List<RaceResult> raceResults;
}