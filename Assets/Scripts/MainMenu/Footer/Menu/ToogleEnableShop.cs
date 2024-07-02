using System.Collections;
using System.Collections.Generic;
using UI.Pagination;
using UnityEngine;

public class ToogleEnableShop : MonoBehaviour
{
    public GameObject CarTabs;
    public GameObject ChooseCarTier;

    // Metoda, która zostanie wywołana przy włączeniu GameObjectu
    void OnEnable()
    {
        if (CarTabs != null && ChooseCarTier != null)
        {
            
            CarTabs.SetActive(false);

            
            ChooseCarTier.SetActive(true);
        }
        else
        {
            Debug.LogError("Jedna lub więcej referencji do GameObjectów jest pusta!");
        }
    }
}
