using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Car : MonoBehaviour
{
    [SerializeField]
    private Service[] AvailableServices = new Service[1];

    [HideInInspector]
    public ParkingSlot slot;

    [HideInInspector]
    public Service[] neededServices;

    [HideInInspector]
    public List<Service> servicesDone = new List<Service>();


    public Transform Bubble;

    public SpriteRenderer Icon;

    public void SetRandomService()
    {
        Service service = AvailableServices[Mathf.FloorToInt(Random.Range(0, AvailableServices.Length))];
        neededServices = new Service[] { service };
        Bubble.gameObject.SetActive(true);
        Icon.sprite = service.Icon;
    }

    public void SetService(Service service)
    {
        neededServices = new Service[] { service };
        Bubble.gameObject.SetActive(true);
        Icon.sprite = service.Icon;
    }

    private void OnDestroy()
    {
        if (Bubble != null)
        {
            Destroy(Bubble.gameObject);
        }
    }

}
