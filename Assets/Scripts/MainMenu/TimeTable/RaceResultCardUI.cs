using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceResultCardUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text weekText;
    public TMP_Text positionText;
    public TMP_Text revenueText;
    public TMP_Text mapText;
    public TMP_Text carText;
    public TMP_Text timeText;

    [Header("Position Colors")]
    public Color firstPlaceColor = Color.yellow;
    public Color secondPlaceColor = new Color(0.75f, 0.75f, 0.75f); // Silver
    public Color thirdPlaceColor = new Color(0.8f, 0.5f, 0.2f); // Bronze
    public Color defaultColor = Color.white;

    [Header("Background")]
    public Image backgroundImage;

    private RaceResult data;

    public void SetData(RaceResult raceResult)
    {
        data = raceResult;

        if (weekText) weekText.text = raceResult.Week.ToString();
        if (positionText) positionText.text = GetPositionText(raceResult.Position);
        if (revenueText) revenueText.text = "$" + raceResult.Revenue.ToString("N0");
        if (mapText) mapText.text = raceResult.Map;
        if (carText) carText.text = raceResult.Car;
        if (timeText) timeText.text = raceResult.Time;

        // Set position color
        SetPositionColor(raceResult.Position);
    }

    private string GetPositionText(int position)
    {
        return position switch
        {
            1 => "1st",
            2 => "2nd",
            3 => "3rd",
            _ => position + "th"
        };
    }

    private void SetPositionColor(int position)
    {
        Color targetColor = position switch
        {
            1 => firstPlaceColor,
            2 => secondPlaceColor,
            3 => thirdPlaceColor,
            _ => defaultColor
        };

        if (positionText) positionText.color = targetColor;
        if (backgroundImage) backgroundImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0.1f);
    }
}