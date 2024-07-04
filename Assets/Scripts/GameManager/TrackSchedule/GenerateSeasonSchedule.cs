using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public static class GenerateSeasonSchedule 
{
    public static List<TrackSchedule> trackSchedules;
    //GENERATE SEASON SCHEDULE FROM RANDOM 8 TRACKS
    private static List<TrackSchedule> GenerateTrackSeasonSchedule()
    {


        List<Track> tracks = TrackList.GetTracks();
        List<TrackSchedule> trackSchedules = new List<TrackSchedule>();

        System.Random rng = new System.Random();

        // OrderBy to shuffle order of tracks
        tracks = tracks.OrderBy(x => rng.Next()).ToList();
        int raceWeek=0;

        foreach(Track track in tracks)
        {
            raceWeek++;
            TrackSchedule trackSchedule = new TrackSchedule(track,raceWeek,0,0,0,false);
            trackSchedules.Add(trackSchedule);
        }

        return trackSchedules;


    }

    public static List<TrackSchedule> GetTrackSchedules()
    {
        if (trackSchedules == null)
        {
           trackSchedules = GenerateTrackSeasonSchedule();
        }

        return trackSchedules;
    }

}
