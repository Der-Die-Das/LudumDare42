using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class ParkingSlot : MonoBehaviour
{
    [SerializeField]
    private bool _Unlocked;
    public bool Unlocked
    {
        get
        {
            return _Unlocked;
        }
        private set
        {
            _Unlocked = value;

            if (ForSaleSign != null)
            {
                ForSaleSign.SetActive(!_Unlocked);
            }
        }
    }

    private static int _UnlockPrice = 40;
    public static int UnlockPrice
    {
        get
        {
            return _UnlockPrice;
        }
        set
        {
            _UnlockPrice = value;
            if (UnlockPriceChanged != null)
            {
                UnlockPriceChanged(_UnlockPrice);
            }
        }
    }

    public static Action<int> UnlockPriceChanged;

    public static int IncreasePerUnlock = 10;

    public GameObject ForSaleSign;

    private Car _Occupied;

    public Car Occupied
    {
        get
        {
            return _Occupied;
        }
        set
        {
            if (value == null)
            {
                CarRemoved(_Occupied);
            }
            else
            {
                CarAssigned(value);
            }
            _Occupied = value;
        }
    }

    protected virtual void Start()
    {
        Unlocked = Unlocked;
        UnlockPrice = UnlockPrice;
    }

    protected abstract void CarAssigned(Car car);

    protected abstract void CarRemoved(Car car);

    public abstract bool CanGetAssigned(Car car);

    public void Unlock()
    {
        Unlocked = true;
        UnlockPrice += IncreasePerUnlock;
    }
}
