using UnityEngine;

public class CarStatsApplier : MonoBehaviour
{
    [Header("Car Reference")]
    public Car carData; // Assign this in the inspector or set it via script

    private TopDownCarController carController;
    
    void Start()
    {
        carController = GetComponent<TopDownCarController>();
        
        if (carData != null && carController != null)
        {
            ApplyCarStats();
        }
        else
        {
            Debug.LogWarning("CarStatsApplier: Missing carData or TopDownCarController component!");
        }
    }
    
    public void SetCarData(Car car)
    {
        carData = car;
        if (carController != null)
        {
            ApplyCarStats();
        }
    }
    
    private void ApplyCarStats()
    {
        if (carData == null || carController == null) return;
        
        // Apply grip (affects drift factor - higher grip = less drift)
        float gripFactor = carData.Grip / 100f; // Convert to 0-1 range
        carController.driftFactor = Mathf.Lerp(0.98f, 0.92f, gripFactor); // Higher grip = lower drift factor
        
        // Apply handling (affects turn factor)
        float handlingFactor = carData.Handling / 100f; // Convert to 0-1 range
        carController.turnFactor = (float)carData.getTurnFactor(carData.Suspenssion, carData.Weight, carData.Handling);
        
        // Apply top speed
        carController.maxSpeed = (float)carData.getTopSpeed(carData.Gearbox, carData.Engine, carData.Turbocharger, carData.Weight);
        
        // Apply acceleration factor
        carController.accelerationFactor = (float)carData.getAccelerationFactor(carData.Engine, carData.Turbocharger, carData.Weight);
        
        Debug.Log($"Applied car stats: Grip={carData.Grip}, Handling={carData.Handling}, DriftFactor={carController.driftFactor}, TurnFactor={carController.turnFactor}");
    }
}