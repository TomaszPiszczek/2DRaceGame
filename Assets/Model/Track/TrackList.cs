using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class TrackList
{

  private static List<Track> tracks; 
  public static List<Track> GetTracks(){
        Debug.Log("Initializing tracks");

        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Database/Tracks/Tracks");

        if (jsonTextAsset != null)
                {
                    tracks = JsonUtility.FromJson<TrackListWrapper>(jsonTextAsset.text).tracks;

                
                }
                else
                {
                    Debug.LogError("Failed to load Tracks.json file!");
                    tracks = new List<Track>();
                }

        return tracks;
  }

   

   [System.Serializable]
    private class TrackListWrapper
    {
        public List<Track> tracks;
    }
}
