using UnityEngine;
using UnityEngine.UI;
// If using TextMeshPro:
 using TMPro;

public class CarCardUI : MonoBehaviour
{
    [Header("UI Refs")]
    public TMP_Text carNameText;          // or TMP_Text
    public Image carImage;            // optional
    public Slider hpSlider;
    public Slider handlingSlider;
    public Slider gripSlider;
    public Slider weightSlider;
    public TMP_Text priceText;            // or TMP_Text
    public Button buyButton;
    
    [Header("Stat Value Text")]
    public TMP_Text hpValueText;
    public TMP_Text handlingValueText;
    public TMP_Text gripValueText;
    public TMP_Text weightValueText;

    [Header("Normalization (tune for your data)")]
    public float hpMin = 420f;        // minimum HP value (AMG A45 S: 421)
    public float hpMax = 720f;        // maximum HP value (McLaren 720S: 720)
    public float handlingMin = 83f;   // minimum handling value (AMG A45 S: 86)
    public float handlingMax = 99f;   // maximum handling value (911 GT3: 99)
    public float gripMin = 83f;       // minimum grip value (AMG A45 S: 83)
    public float gripMax = 97f;       // maximum grip value (911 GT3: 97)
    public float weightMin = 1419f;   // minimum weight value (McLaren 720S: 1419)
    public float weightMax = 1855f;   // maximum weight value (BMW M8: 1855)

    private Car data;

    public void SetData(Car car)
    {
        data = car;

        if (carNameText) carNameText.text = car.Name;
        if (priceText) priceText.text = car.Price.ToString("N0"); // 6 000 style

     

        // Normalize values 0..1 using min-max range and set text values
        if (hpSlider) 
        {
            float hpNorm = Mathf.Clamp01((car.Engine.HorsePower - hpMin) / Mathf.Max(1f, hpMax - hpMin));
            hpSlider.value = hpNorm;
            hpSlider.interactable = false;
        }
        if (hpValueText) hpValueText.text = car.Engine.HorsePower.ToString();
        
        if (handlingSlider) 
        {
            float handlingNorm = Mathf.Clamp01((car.Handling - handlingMin) / Mathf.Max(1f, handlingMax - handlingMin));
            handlingSlider.value = handlingNorm;
            handlingSlider.interactable = false;
        }
        if (handlingValueText) handlingValueText.text = car.Handling.ToString();
        
        if (gripSlider) 
        {
            float gripNorm = Mathf.Clamp01((car.Grip - gripMin) / Mathf.Max(1f, gripMax - gripMin));
            gripSlider.value = gripNorm;
            gripSlider.interactable = false;
        }
        if (gripValueText) gripValueText.text = car.Grip.ToString();

        if (weightSlider)
        {
            // Normalize weight and invert so "lighter = more orange"
            float weightNorm = Mathf.Clamp01((car.Weight - weightMin) / Mathf.Max(1f, weightMax - weightMin));
            weightSlider.value = 1f - weightNorm;
            weightSlider.interactable = false;
        }
        if (weightValueText) weightValueText.text = car.Weight.ToString();

        if (buyButton)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyPressed);
        }

        // Make sliders look like your orange bars:
        // - Interactable = false
        // - Transition = None
        // - Set Fill Area color to your orange
        // - Remove handle if you don’t want the knob
    }

    private void OnBuyPressed()
    {
        Debug.Log($"Buy {data?.Name} for {data?.Price}");
        // TODO: Hook into your currency/garage systems.
    }
}