using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI.TableUI;
public class TrackManager : MonoBehaviour
{
        public TableUI table;
        public Text timetableText; // Referencja do komponentu Text w UI do wyświetlania harmonogramu
        public Button nextRaceButton; // Referencja do przycisku "Next Race"

        [System.Serializable]
        public class Track
        {
            public string trackName;
            public string raceWeek;
            public string sceneName; // Nazwa sceny
        }

        public List<Track> allTracks = new List<Track>(); // Lista wszystkich torów (20 torów)
        public List<Track> timetable = new List<Track>(); // Lista torów w harmonogramie (8 torów)

        private void Start()
        {
            // Dodanie trzech torów z konkretnymi nazwami scen
            AddTrack("Track 1", "Track_01");
            AddTrack("Track 2", "Track_02");
            AddTrack("Track 3", "Track_03");

            // Inicjalizacja harmonogramu pierwszymi 8 torami
            InitializeTimetable();

            // Wyświetlenie harmonogramu w tabeli
            UpdateTable();

            // Subskrypcja przycisku "Next Race"
            nextRaceButton.onClick.AddListener(OnNextRaceButtonClicked);
        }

        private void AddTrack(string trackName, string sceneName)
        {
            // Wygenerowanie losowego tygodnia wyścigu
            string raceWeek = "Week " + Random.Range(1, 9).ToString();

            // Sprawdzenie, czy tydzień nie jest już przypisany
            while (allTracks.Exists(track => track.raceWeek == raceWeek))
            {
                raceWeek = "Week " + Random.Range(1, 9).ToString();
            }

            // Dodanie nowego toru do listy
            allTracks.Add(new Track
            {
                trackName = trackName,
                raceWeek = raceWeek,
                sceneName = sceneName
            });
        }

        private void InitializeTimetable()
        {
            for (int i = 0; i < 8 && i < allTracks.Count; i++)
            {
                timetable.Add(allTracks[i]);
            }
        }

        private void UpdateTable()
        {
            

            // Ustaw nagłówki kolumn
            table.GetCell(0, 0).text = "Track Name";
            table.GetCell(0, 1).text = "Race Week";

            // Wypełnij tabelę danymi z timetable
            for (int i = 0; i < timetable.Count; i++)
            {
                table.GetCell(i + 1, 0).text = timetable[i].trackName;
                table.GetCell(i + 1, 1).text = timetable[i].raceWeek;
            }
        }

        private void OnNextRaceButtonClicked()
        {
            if (timetable.Count > 0)
            {
                // Usunięcie pierwszego toru z harmonogramu
                Track nextTrack = timetable[0];
                timetable.RemoveAt(0);

                // Dodanie kolejnego toru z listy allTracks, jeśli istnieje
                if (allTracks.Count > timetable.Count)
                {
                    timetable.Add(allTracks[timetable.Count]);
                }

                // Zaktualizowanie wyświetlania harmonogramu w tabeli
                UpdateTable();

                // Załadowanie następnej sceny
                LoadNextTrack(nextTrack);
            }
        }

        private void LoadNextTrack(Track track)
        {
            SceneManager.LoadScene(track.sceneName);
        }
}
