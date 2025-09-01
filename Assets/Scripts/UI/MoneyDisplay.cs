using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    private TMP_Text moneyText;
    
    private void Start()
    {
        moneyText = GetComponent<TMP_Text>();
        Debug.Log($"MoneyDisplay Start: moneyText={moneyText}, GameManager.Instance={GameManager.Instance}");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterMoneyDisplay(this);
        }
        UpdateDisplay();
    }
    
    private void OnEnable()
    {
        Debug.Log($"MoneyDisplay OnEnable: GameManager.Instance={GameManager.Instance}");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterMoneyDisplay(this);
        }
        UpdateDisplay();
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterMoneyDisplay(this);
        }
    }
    
    public void UpdateDisplay()
    {
        Debug.Log($"UpdateDisplay called: GameManager.Instance={GameManager.Instance}, moneyText={moneyText}");
        if (GameManager.Instance != null && moneyText != null)
        {
            int money = GameManager.Instance.GetMoney();
            Debug.Log($"Setting money text to: ${money:N0}");
            moneyText.text = "$" + money.ToString("N0");
        }
        else
        {
            Debug.LogError($"UpdateDisplay failed: GameManager.Instance={GameManager.Instance}, moneyText={moneyText}");
        }
    }
    
}