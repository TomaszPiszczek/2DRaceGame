using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetLeaderboardItemInfo : MonoBehaviour
{
    public TextMeshProUGUI postionText;
    public TextMeshProUGUI driverNameText;
    public TextMeshProUGUI lapTimeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }


   public void SetPositionText(string position)
    {
        if (postionText != null)
        {
            postionText.text = position;
        }
    }

    public void SetDriverNameText(string driverName)
    {
        if (driverNameText != null)
        {
            driverNameText.text = driverName;
        }
    }
    public void SetLapTimeText(string lapTime)
    {
        if (lapTimeText != null)
        {
            lapTimeText.text = lapTime;
        }
    }
   
}
