using UnityEngine;

[System.Serializable]
public class Track
{
    public float timeRecord;        // Best lap time on the track
    public float topSpeed;          // Top speed achieved on the track
    public int laps;                // Number of laps in a race
    public int polePositions;       // Number of pole positions achieved
    public int wins;                // Number of wins on the track
    public string trackName;        // Name of the track
    public Sprite trackImage;       // Image representing the track

    // Constructor for ease of creating new tracks
    public Track(float timeRecord, float topSpeed, int laps, int polePositions, int wins, string trackName, Sprite trackImage)
    {
        this.timeRecord = timeRecord;
        this.topSpeed = topSpeed;
        this.laps = laps;
        this.polePositions = polePositions;
        this.wins = wins;
        this.trackName = trackName;
        this.trackImage = trackImage;
    }
}
