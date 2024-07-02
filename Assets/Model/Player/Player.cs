using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public  class Player 
{

    public static Player Instance { get; private set; }

    public Garage garage;
    public int money;
    
    public Player(int money,Garage garage){
        this.money = money;
        this.garage = garage;
        Instance = this;
    }

   


    
}
