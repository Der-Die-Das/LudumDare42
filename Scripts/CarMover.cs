using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarMover : MonoBehaviour
{
    private Car MovingCar;

    private Camera cam;

    private MoneyController moneyController;

    private AudioSource boughtSound;
    public AudioSource departureSound;
    public static System.Action SpotBought;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
        moneyController = FindObjectOfType<MoneyController>();
        boughtSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MovingCar = GetHitCar();

            //not moving cars, hehe
            if (MovingCar == null)
            {
                ParkingSlot slot = GetHitParkSlot();
                if (slot != null && slot.Unlocked == false)
                {
                    if (moneyController.Money >= ParkingSlot.UnlockPrice)
                    {
                        moneyController.Money -= ParkingSlot.UnlockPrice;
                        slot.Unlock();
                        boughtSound.Play();
                        if (SpotBought != null)
                        {
                            SpotBought();
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (MovingCar != null)
            {
                Vector3 newPos = cam.ScreenToWorldPoint(Input.mousePosition);
                MovingCar.transform.position = new Vector3(newPos.x, newPos.y, 0);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (MovingCar != null)
            {
                ParkingSlot slot = GetHitParkSlot();
                if (slot != null && slot.Occupied == null && slot.Unlocked)
                {
                    if (slot.CanGetAssigned(MovingCar) == false)
                    {
                        ReturnCar();
                        return;
                    }
                    SetCarToNewSlot(slot);
                    if (slot is IdleSlot && ((IdleSlot)slot).SlotType == IdleSlot.IdleSlotType.DepartSlot)
                    {
                        departureSound.Play();
                    }
                    return;
                }
                ReturnCar();
            }
        }
    }

    private void ReturnCar()
    {
        MovingCar.transform.position = MovingCar.slot.transform.position;
        MovingCar.transform.rotation = MovingCar.slot.transform.rotation;
        MovingCar = null;
    }

    private void SetCarToNewSlot(ParkingSlot newSlot)
    {
        MovingCar.transform.position = newSlot.transform.position;
        MovingCar.transform.rotation = newSlot.transform.rotation;
        MovingCar.slot.Occupied = null;
        newSlot.Occupied = MovingCar;
        MovingCar.slot = newSlot;

        MovingCar = null;
    }

    private Car GetHitCar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 100);

        foreach (var hit in hits)
        {
            Car hitCar = hit.transform.GetComponent<Car>();
            if (hitCar != null)
            {
                return hitCar;
            }
        }
        return null;
    }
    private ParkingSlot GetHitParkSlot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 100);

        foreach (var hit in hits)
        {
            ParkingSlot hitSlot = hit.transform.GetComponent<ParkingSlot>();
            if (hitSlot != null)
            {
                return hitSlot;
            }
        }

        return null;
    }
}
