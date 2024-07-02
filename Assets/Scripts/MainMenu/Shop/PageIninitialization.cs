using System.Collections.Generic;
using TMPro;
using UI.Pagination;
using UnityEngine;
using UnityEngine.UI;

public class PageInitialization : MonoBehaviour
{
    private GameObject pageTemplatePrefab;
    public Transform contentTransform;

    public PagedRect pagedRect;

    private List<Car> cars = new List<Car>();

 

    public void LoadCarsForTier(int tierIndex)
    {
        switch (tierIndex)
        {
            case 0:
                cars = CarShopList.getATierCars();
                break;
            case 1:
                cars = CarShopList.getBTierCars();
                break;
            case 2:
                cars = CarShopList.getCTierCars();
                break;
            case 3:
                cars = CarShopList.getDTierCars();
                break; 
            case 4:
                cars = CarShopList.getETierCars();
                break;       


            default:
                cars = CarShopList.getATierCars();
                break;
        }

        CreateCarPages();
    }

    void CreateCarPages()
    {
        pageTemplatePrefab = Resources.Load<GameObject>("PageTemplate");
        if (pageTemplatePrefab == null)
        { 
            Debug.LogError("Page Template Prefab is not loaded. Cannot create car pages.");
            return;
        }

        
      
        
      
        pagedRect.RemoveAllPages();
       

        

        // Create new pages
        foreach (var car in cars)
        {
            GameObject page = Instantiate(pageTemplatePrefab, contentTransform);
            Page pageObject = page.GetComponent<Page>();

            TextMeshProUGUI carNameText = page.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = page.transform.Find("Description").GetComponent<TextMeshProUGUI>();

            carNameText.text = car.Name;
            descriptionText.text = car.ToString();

            Button buyButton = page.transform.Find("Buy").GetComponent<Button>();
            TextMeshProUGUI buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            buyButtonText.text = "$" + car.Price.ToString();

            buyButton.onClick.AddListener(() => OnBuyButtonClicked(car));
            pagedRect.AddPage(pageObject);

        }
        
    }

    void OnBuyButtonClicked(Car car)
    {
        Debug.Log("Buy button clicked for " + car.Name);
        
    }
}
