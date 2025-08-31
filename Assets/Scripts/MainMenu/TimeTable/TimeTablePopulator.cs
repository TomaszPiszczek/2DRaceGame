using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTablePopulator : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform content;
    public ScrollRect scrollRect;

    [Header("Prefabs")]
    public RaceResultCardUI cardPrefab;

    [Header("Layout Settings")]
    public float cardHeight = 80f;
    public float spacing = 5f;
    public Vector4 padding = new Vector4(10, 10, 10, 10); // L, T, R, B

    [Header("Sorting Options")]
    public SortType currentSortType = SortType.Week;

    public enum SortType
    {
        Week,
        Position,
        Revenue,
        Map,
        Car,
        Time
    }

    private readonly List<GameObject> spawnedRows = new();

    private void OnEnable()
    {
        if (ValidateComponents())
        {
            PopulateTable();
        }
    }

    private bool ValidateComponents()
    {
        if (content == null)
        {
            Debug.LogError("Content RectTransform is null!");
            return false;
        }
        if (cardPrefab == null)
        {
            Debug.LogError("CardPrefab is null!");
            return false;
        }
        return true;
    }

    public void PopulateTable()
    {
        if (!ValidateComponents())
        {
            Debug.LogError("Cannot populate table - missing components!");
            return;
        }

        ClearTable();

        List<RaceResult> results = GetSortedResults();
        
        // Calculate total content height needed
        float totalHeight = padding.y + padding.w + (results.Count * cardHeight) + ((results.Count - 1) * spacing);
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);

        // Create all rows
        for (int i = 0; i < results.Count; i++)
        {
            CreateResultRow(results[i], i);
        }

        // Reset scroll position to top
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void CreateResultRow(RaceResult result, int index)
    {
        var card = Instantiate(cardPrefab, content);
        card.SetData(result);

        // Position the row
        var rectTransform = card.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 1);
        
        // Calculate Y position from top
        float yPosition = -(padding.y + (index * (cardHeight + spacing)));
        rectTransform.anchoredPosition = new Vector2(0, yPosition);
        rectTransform.sizeDelta = new Vector2(-padding.x - padding.z, cardHeight);

        spawnedRows.Add(card.gameObject);
    }

    private List<RaceResult> GetSortedResults()
    {
        return currentSortType switch
        {
            SortType.Week => RaceResultsList.GetAllResults(),
            SortType.Position => GetResultsSortedByPosition(),
            SortType.Revenue => GetResultsSortedByRevenue(),
            SortType.Map => GetResultsSortedByMap(),
            SortType.Car => GetResultsSortedByCar(),
            SortType.Time => GetResultsSortedByTime(),
            _ => RaceResultsList.GetAllResults()
        };
    }

    private List<RaceResult> GetResultsSortedByPosition()
    {
        var results = RaceResultsList.GetAllResults();
        results.Sort((a, b) => a.Position.CompareTo(b.Position));
        return results;
    }

    private List<RaceResult> GetResultsSortedByRevenue()
    {
        var results = RaceResultsList.GetAllResults();
        results.Sort((a, b) => b.Revenue.CompareTo(a.Revenue)); // Descending
        return results;
    }

    private List<RaceResult> GetResultsSortedByMap()
    {
        var results = RaceResultsList.GetAllResults();
        results.Sort((a, b) => string.Compare(a.Map, b.Map));
        return results;
    }

    private List<RaceResult> GetResultsSortedByCar()
    {
        var results = RaceResultsList.GetAllResults();
        results.Sort((a, b) => string.Compare(a.Car, b.Car));
        return results;
    }

    private List<RaceResult> GetResultsSortedByTime()
    {
        var results = RaceResultsList.GetAllResults();
        results.Sort((a, b) => string.Compare(a.Time, b.Time));
        return results;
    }

    private void ClearTable()
    {
        foreach (var row in spawnedRows)
        {
            if (row) Destroy(row);
        }
        spawnedRows.Clear();

        if (content != null)
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                if (content.GetChild(i) != null)
                    Destroy(content.GetChild(i).gameObject);
            }
        }
    }

    // Public methods for UI buttons
    public void SortByWeek()
    {
        if (ValidateComponents())
        {
            currentSortType = SortType.Week;
            PopulateTable();
        }
    }

    public void SortByPosition()
    {
        if (ValidateComponents())
        {
            currentSortType = SortType.Position;
            PopulateTable();
        }
    }

    public void SortByRevenue()
    {
        if (ValidateComponents())
        {
            currentSortType = SortType.Revenue;
            PopulateTable();
        }
    }

    public void SortByMap()
    {
        if (ValidateComponents())
        {
            currentSortType = SortType.Map;
            PopulateTable();
        }
    }

    public void SortByCar()
    {
        if (ValidateComponents())
        {
            currentSortType = SortType.Car;
            PopulateTable();
        }
    }

    public void SortByTime()
    {
        if (ValidateComponents())
        {
            currentSortType = SortType.Time;
            PopulateTable();
        }
    }
}