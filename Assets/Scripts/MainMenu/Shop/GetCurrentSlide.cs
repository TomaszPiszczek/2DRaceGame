using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    public ShopManager shopManager;

    void OnTransformChildrenChanged()
    {
        CheckBottomSlide();
    }

    // Method to check which slide is on top
    void CheckBottomSlide()
    {
        Transform bottomSlide = null;

        // Iterate through each child of this GameObject
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentSlide = transform.GetChild(i);


            // If this is the first slide we're checking, or if the current slide is lower in hierarchy
            if (bottomSlide == null || currentSlide.GetSiblingIndex() > bottomSlide.GetSiblingIndex())
            {
                bottomSlide = currentSlide;
            }
        }

        // Log the name of the bottom slide
        if (bottomSlide != null)
        {

            // Get the ShopItem component from the bottom slide
            ShopItem shopItem = bottomSlide.GetComponent<ShopItem>();
            if (shopItem != null)
            {
                if (shopManager != null)
                {
                    // Update ShopManager with the car details
                    shopManager.UpdateCarDetails(shopItem.car);
                }
                else
                {
                    Debug.LogError("ShopManager reference is null");
                }
            }
            else
            {
                Debug.LogError("ShopItem component is null on the bottom slide: " + bottomSlide.name);
            }
        }
        else
        {
            Debug.LogError("No bottom slide found");
        }
    }
}
