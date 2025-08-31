using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    private TMP_Text moneyText;
    
    private void Start()
    {
        moneyText = GetComponent<TMP_Text>();
        UpdateDisplay();
    }
    
    private void OnEnable()
    {
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (GameManager.Instance != null && moneyText != null)
        {
            moneyText.text = "$" + GameManager.Instance.GetMoney().ToString("N0");
        }
    }
    
    private void Update()
    {
        UpdateDisplay();
    }
}