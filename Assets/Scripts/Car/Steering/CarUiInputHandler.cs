using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUiInputHandler : MonoBehaviour
{
    CarInputHandler playerCarInputHandler;
    Vector2 inputVector = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        CarInputHandler[] carInputHandlers = FindObjectsOfType<CarInputHandler>();

        foreach(CarInputHandler carInputHandler  in carInputHandlers)
        {
            {
                if(carInputHandler.isUiInput)
                {
                    playerCarInputHandler = carInputHandler;
                    break;
                }
            }
        }


    }


    public void OnAcceleratePress()
    {
            inputVector.y = 1.0f;
            playerCarInputHandler.SetInput(inputVector);
    }

    public void OnAccelerateBrakRelease()
    {
            inputVector.y = 0.0f;
            playerCarInputHandler.SetInput(inputVector);
            
    }

    public void OnBrakePress()
    {
            inputVector.y = -1.0f;
            playerCarInputHandler.SetInput(inputVector);

    }
    public void OnRightPress()
    {
            inputVector.x = 1.0f;
            playerCarInputHandler.SetInput(inputVector);

    }

    public void OnLeftPress()
    {
            inputVector.x = -1.0f;
            playerCarInputHandler.SetInput(inputVector);


    }

    public void OnSteringRelease()
    {
            inputVector.x = 0.0f;
            playerCarInputHandler.SetInput(inputVector);


    }



}
