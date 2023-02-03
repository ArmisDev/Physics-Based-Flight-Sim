using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public PlaneController planeController;

    [SerializeField]
    PickupSlot[] pickUpSlots;

    [System.Serializable]
    public class PickupSlot
    {
        public PickupType pickupType;
        public float _currentFuel;
        public float _fuelTotal;
    }


    public void ReduceFuel(PickupType pickupType)
    {
        GrabPickupType(pickupType)._currentFuel -= planeController._throttle * 0.0001f;

        if(GrabPickupType(pickupType)._currentFuel <= 0)
        {
            GrabPickupType(pickupType)._currentFuel = 0f;
        }
        
    }

    public void OutOfFuelHandler(PickupType pickupType)
    {
        if (GrabPickupType(pickupType)._currentFuel <= 0)
        {
            Debug.Log("Out Of Fuel!!");
            FindObjectOfType<PlaneController>().FuelHandler();
        }
    }

    public void HandleFuelPickup(PickupType pickupType, float _currentFuel)
    {
        GrabPickupType(pickupType)._currentFuel += _currentFuel;
    }
    
    public PickupSlot GrabPickupType(PickupType pickupType)
    {
        foreach(PickupSlot slot in pickUpSlots)
        {
            if(slot.pickupType == pickupType)
            {
                return slot;
            }
        }

        return null;
    }
}