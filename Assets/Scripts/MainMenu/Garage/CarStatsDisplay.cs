using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarStatsDisplay : MonoBehaviour
{
    [Header("Car Stats UI Elements")]
    public TextMeshProUGUI carNameText;
    public TextMeshProUGUI horsePowerText;
    public TextMeshProUGUI topSpeedText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI gripText;
    public TextMeshProUGUI handlingText;
    
    [Header("Car Stats Sliders (Optional)")]
    public Slider horsePowerSlider;
    public Slider topSpeedSlider;
    public Slider gripSlider;
    public Slider handlingSlider;
    
    public void DisplayCarStats(Car car)
    {
        if (car == null) return;
        
        // Update text elements if they exist
        if (carNameText != null)
            carNameText.text = car.Name;
            
        if (horsePowerText != null)
            horsePowerText.text = $"HP: {car.Engine.HorsePower + car.Turbocharger.HpBoost}";
            
        if (topSpeedText != null)
            topSpeedText.text = $"Top Speed: {car.getTopSpeed(car.Gearbox, car.Engine, car.Turbocharger, car.Weight)*5:F0} km/h";
            
        if (weightText != null)
            weightText.text = $"Weight: {car.Weight} kg";
            
        if (priceText != null)
            priceText.text = $"Price: ${car.Price:N0}";
            
        if (gripText != null)
            gripText.text = $"Grip: {car.Grip}";
            
        if (handlingText != null)
            handlingText.text = $"Handling: {car.Handling}";
        
        // Update sliders if they exist (normalize values to 0-1 range)
        if (horsePowerSlider != null)
            horsePowerSlider.value = Mathf.Clamp01((car.Engine.HorsePower + car.Turbocharger.HpBoost) / 650f); // Max HP around 650
            
        if (topSpeedSlider != null)
            topSpeedSlider.value = Mathf.Clamp01((float)car.getTopSpeed(car.Gearbox, car.Engine, car.Turbocharger, car.Weight) / 25f); // Max speed around 25
            
        if (gripSlider != null)
            gripSlider.value = car.Grip / 100f; // Grip is 0-100
            
        if (handlingSlider != null)
            handlingSlider.value = car.Handling / 100f; // Handling is 0-100
    }
    
    public void ClearDisplay()
    {
        if (carNameText != null) carNameText.text = "";
        if (horsePowerText != null) horsePowerText.text = "";
        if (topSpeedText != null) topSpeedText.text = "";
        if (weightText != null) weightText.text = "";
        if (priceText != null) priceText.text = "";
        if (gripText != null) gripText.text = "";
        if (handlingText != null) handlingText.text = "";
        
        if (horsePowerSlider != null) horsePowerSlider.value = 0;
        if (topSpeedSlider != null) topSpeedSlider.value = 0;
        if (gripSlider != null) gripSlider.value = 0;
        if (handlingSlider != null) handlingSlider.value = 0;
    }
}