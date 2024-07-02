using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopManager : MonoBehaviour
{

    public TMP_Text coinUI;
    public TMP_Text carName;
    public TMP_Text price;
    Car car;



    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins()
    {
        Player.Instance.money +=5;
        coinUI.text = "Coins :" +Player.Instance.money.ToString();
         
       
    }

    public void UpdateCarDetails(Car car)
    {
        if (car != null)
        {
            carName.text = "Car Name: " + car.Name;
            price.text = "Price: " + car.Price.ToString();
            this.car = car;
        }else
        {
            Debug.Log("car is null");
        }
    }

    public void BuyCar(){
        Player.Instance.money -= this.car.Price;
        Player.Instance.garage.addCar(this.car);
        coinUI.text = "Coins :" +Player.Instance.money.ToString();

        Debug.Log("Car bought");
    }

   
}
