using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Garage 
{
    public int size;
    public List<Car> cars;
    public List<CarPart> carParts;

    public Garage(int size,List<Car> cars,List<CarPart> carParts)
    {
        this.size = size;
        this.cars = cars;
        this.carParts = carParts;
        
    }
    public void addCar(Car car)
    {
        if(isAvaliableSpaceInGarage()){
            cars.Add(car);
        }else{
            Debug.Log("No avaliable space in garage");
        }
    }
    private bool isAvaliableSpaceInGarage(){
        if(cars.Count == size) return false;
        return true;
    }

   
}
