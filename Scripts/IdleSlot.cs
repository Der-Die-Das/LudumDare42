using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IdleSlot : ParkingSlot
{
    public enum IdleSlotType { QueueSlot, WaitSlot, DepartSlot }

    public IdleSlotType SlotType;

    public static Action Departured;

    private MoneyController moneyController;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        moneyController = FindObjectOfType<MoneyController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void CarAssigned(Car car)
    {
        if (SlotType == IdleSlotType.DepartSlot)
        {
            moneyController.Money += car.neededServices.Sum(x => x.Profit);
            Destroy(car.gameObject);
        }
    }

    protected override void CarRemoved(Car car)
    {
        //no reaction .. ?
    }

    public override bool CanGetAssigned(Car car)
    {
        if (SlotType == IdleSlotType.DepartSlot)
        {
            if (car.servicesDone.Count == car.neededServices.Length)
            {
                if (Departured != null)
                {
                    Departured();
                }
                return true;
            }
            return false;
        }
        if (SlotType == IdleSlotType.QueueSlot)
        {
            return false;
        }

        return true;
    }
}
