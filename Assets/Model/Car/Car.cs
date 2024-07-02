using System;
using UnityEngine;
[Serializable]
public class Car
{
     public String pathToTopDownImage;


    public string Name;
    public Engine Engine; //calculate top speed / accceleration
    public Turbocharger Turbocharger; //increase Engine power
    public Tire Tire; //used to calculate Tire friction
    public Gearbox Gearbox;  // Used to calculate topspeed
    public Suspenssion Suspenssion; // used to calculate turn factor default 1.5
    public int Weight;  // Value in kgs
    public int Price;
    public int Tier;



   //public Sprite carImage; // Dodane pole obrazka

    public Car(string Name, Engine Engine, Turbocharger Turbocharger, Tire Tire, Gearbox Gearbox, Suspenssion Suspenssion, int Weight, int Price,String pathToTopDownImage,int Tier)
    {
        this.Name = Name;
        this.Weight = Weight;
        this.Engine = Engine;
        this.Turbocharger = Turbocharger;
        this.Tire = Tire;
        this.Gearbox = Gearbox;
        this.Suspenssion = Suspenssion;
        this.Price = Price;
        this.pathToTopDownImage = pathToTopDownImage;
        this.Tier = Tier;
    } 


   //AVG 15
    public double getTopSpeed(Gearbox Gearbox,Engine Engine,Turbocharger Turbocharger,int Weight){
      if((Engine.HorsePower + Turbocharger.HpBoost)/10 < Gearbox.TopSpeed){
        return Engine.HorsePower/10 + getAccelerationFactor(Engine,Turbocharger,Weight)/10 ;
      }
      return Gearbox.TopSpeed + getAccelerationFactor(Engine,Turbocharger,Weight)/10 + getAccelerationFactor(Engine,Turbocharger,Weight)/10 ;
    }

    //AVG 2
    public double getTurnFactor(Suspenssion Suspenssion,int Weight){

      return Suspenssion.TurnFactor + calculateTurnFactorBaseOnWeight(Weight);
    }
    //AVG 0.95  CANT BE HIGHER THAN 0.99 LOWER THAN 0.93
    public double getTireFriction(Tire Tire){
      return Tire.Friction;
    }

    
    public double getAccelerationFactor(Engine Engine,Turbocharger Turbocharger,int Weight){

        double powerToWeightRatio = (Engine.HorsePower + Turbocharger.HpBoost) / Weight;
        double zeroToHundredTime = Math.Pow(Weight / (Engine.HorsePower + Turbocharger.HpBoost) * 0.875, 0.75);

      return convertTimeAccelerationToAcceleartionFactor(zeroToHundredTime);
    }


    //Calculate the turn factor basing on Weight for cars Weighting between 1000kg and 3000kg
    //For car Weighting 1000kg value is 0.4 for car Weighting 3000kg value is -0.4 for car 2000kg value is 0
    private double calculateTurnFactorBaseOnWeight(int Weight){
      
            if (Weight < 1000)
                    {
                        return 0.4;
                    }
                    else if (Weight > 3000)
                    {
                        return -0.4;
                    }
                    else
                    {
                        return -0.0004 * Weight + 0.8;
                    }
    }



    //based on //https://www.dcode.fr/function-equation-finder
    /*
    example:

    (time zero to hundred) = acceleration factor in game
                2s          =     	19.035
                3s          =     	16
                4s          =     	13
                5s          =	      11
                7s          =     	8
                10s	        =       5
                13.235s         =      	2.872
    */
    private double convertTimeAccelerationToAcceleartionFactor(double zeroToHundredTime){
        if(zeroToHundredTime < 2) return 20;
        if(zeroToHundredTime > 13) return 2.5;

        double a = 0.128036;
        double b = -3.3892;
        double c = 25.3006;
        
        return a * zeroToHundredTime * zeroToHundredTime + b *zeroToHundredTime + c;
    }




    public override string ToString()
{
    return 
           $"Weight: {Weight} kg\n" +
           $"Engine: {Engine.HorsePower}\n" +
           $"Top Speed: {getTopSpeed(Gearbox, Engine, Turbocharger, Weight)*5} km/h\n";
}
    
}
