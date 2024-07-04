using UnityEngine;

public class TrackSchedule : MonoBehaviour
{
    private Track track;
    private int raceWeek;
    private int result;
    private int rivalResult;
    private int teamResult;
    private bool finished;

    public TrackSchedule(Track track, int raceWeek, int result, int rivalResult, int teamResult, bool finished)
    {
        this.track = track;
        this.raceWeek = raceWeek;
        this.result = result;
        this.rivalResult = rivalResult;
        this.teamResult = teamResult;
        this.finished = finished;
    }


    public Track Track
    {
        get { return track; }
        set { track = value; }
    }

    public int RaceWeek
    {
        get { return raceWeek; }
        set { raceWeek = value; }
    }

    public int Result
    {
        get { return result; }
        set { result = value; }
    }

    public int RivalResult
    {
        get { return rivalResult; }
        set { rivalResult = value; }
    }

    public int TeamResult
    {
        get { return teamResult; }
        set { teamResult = value; }
    }

    public bool Finished
    {
        get { return finished; }
        set { finished = value; }
    }
}
