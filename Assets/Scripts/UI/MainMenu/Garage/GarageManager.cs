using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GarageManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform carListParent;
    public GameObject carSlotPrefab;
    public TMP_Text currentCarNameText;
    public TMP_Text carStatsText;
    
    [Header("Car Display")]
    public CarStatsDisplay carStatsDisplay;
    
    private List<GameObject> carSlots = new List<GameObject>();
    
    private void OnEnable()
    {
        RefreshGarage();
    }
    
    public void RefreshGarage()
    {
        ClearCarSlots();
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        
        List<Car> ownedCars = GameManager.Instance.GetOwnedCars();
        Car currentCar = GameManager.Instance.GetCurrentCar();
        
        for (int i = 0; i < ownedCars.Count; i++)
        {
            CreateCarSlot(ownedCars[i], i, currentCar == ownedCars[i]);
        }
        
        UpdateCurrentCarDisplay(currentCar);
    }
    
    private void CreateCarSlot(Car car, int index, bool isSelected)
    {
        if (carSlotPrefab == null || carListParent == null) return;
        
        GameObject slot = Instantiate(carSlotPrefab, carListParent);
        carSlots.Add(slot);
        
        TMP_Text nameText = slot.GetComponentInChildren<TMP_Text>();
        if (nameText) nameText.text = car.Name;
        
        Button selectButton = slot.GetComponentInChildren<Button>();
        if (selectButton)
        {
            selectButton.onClick.AddListener(() => SelectCar(index));
            
            ColorBlock colors = selectButton.colors;
            if (isSelected)
            {
                colors.normalColor = Color.green;
                colors.selectedColor = Color.green;
            }
            selectButton.colors = colors;
        }
        
        // Image carImage = slot.GetComponentsInChildren<Image>()[1];
        // if (carImage && !string.IsNullOrEmpty(car.pathToTopDownImage))
        // {
        //     Sprite carSprite = Resources.Load<Sprite>(car.pathToTopDownImage);
        //     if (carSprite) carImage.sprite = carSprite;
        // }
    }
    
    private void SelectCar(int carIndex)
    {
        GameManager.Instance.SetCurrentCar(carIndex);
        RefreshGarage();
        
        Debug.Log($"Selected car: {GameManager.Instance.GetCurrentCar()?.Name}");
    }
    
    private void UpdateCurrentCarDisplay(Car currentCar)
    {
        if (currentCar == null)
        {
            if (currentCarNameText) currentCarNameText.text = "No Car Selected";
            if (carStatsText) carStatsText.text = "Buy a car from the shop first!";
            return;
        }
        
        if (currentCarNameText) currentCarNameText.text = currentCar.Name;
        
        if (carStatsDisplay) 
        {
            carStatsDisplay.DisplayCarStats(currentCar);
        }
        else if (carStatsText)
        {
            carStatsText.text = currentCar.ToString();
        }
        
    }
    
    private void ClearCarSlots()
    {
        foreach (GameObject slot in carSlots)
        {
            if (slot != null) Destroy(slot);
        }
        carSlots.Clear();
    }
}