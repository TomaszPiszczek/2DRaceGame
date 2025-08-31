using UnityEngine;

public class RaceMovementController : MonoBehaviour
{
    private MonoBehaviour[] carControllers;
    
    void Start()
    {
        carControllers = GetComponents<MonoBehaviour>();
    }
    
    void Update()
    {
        bool canMove = RaceManager.IsRaceActive;
        
        foreach (var controller in carControllers)
        {
            if (controller != this && controller != null)
            {
                controller.enabled = canMove;
            }
        }
    }
}