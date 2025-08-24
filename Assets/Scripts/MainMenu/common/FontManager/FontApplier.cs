using UnityEngine;
using TMPro;

public class FontApplierTMP : MonoBehaviour
{
    public FontSettings fontSettings;

    void Awake()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        if (text != null && fontSettings != null && fontSettings.globalTMPFont != null)
        {
            text.font = fontSettings.globalTMPFont;
        }
        text.rectTransform.anchoredPosition += new Vector2(0f, -15f); 
    }
}