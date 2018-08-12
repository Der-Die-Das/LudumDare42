using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarSpawner : MonoBehaviour
{
    public AnimationCurve SpawnTimePerSpawnedCar;

    private CarType[] AllCarTypes;

    public IdleSlot[] QueueSlots;

    public GameObject BubblePrefab;

    private AudioSource audioSrc;

    public Service repair;


    private int spawnedCars = 0;
    // Use this for initialization
    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        LoadCarTypes();
    }

    public void StartSpawning()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnCars());
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            SpawnRandomCar();
            float value = SpawnTimePerSpawnedCar.Evaluate(spawnedCars);
            float time = Random.Range(value - 1f, value + 1f);

            yield return new WaitForSeconds(time);
        }
    }

    public void SpawnRepairCar()
    {
        Car car = SpawnCar();
        if (car != null)
        {
            car.SetService(repair);
        }
    }
    public void SpawnRandomCar()
    {
        SpawnCar();
    }

    private Car SpawnCar()
    {
        CarType randomType = AllCarTypes[Mathf.FloorToInt(Random.Range(0, AllCarTypes.Length))];
        IdleSlot slotToQueueIn = null;

        for (int i = 0; i < QueueSlots.Length; i++)
        {
            if (QueueSlots[i].Occupied == null)
            {
                slotToQueueIn = QueueSlots[i];
                break;
            }
        }

        if (slotToQueueIn == null)
        {
            Debug.Log("you lost");
            StopAllCoroutines();
            FindObjectOfType<UIManager>().ShowLostScreen();
            return null;
        }

        GameObject newCarGO = Instantiate(randomType.Prefab, slotToQueueIn.transform.position, slotToQueueIn.transform.rotation);
        GameObject bubble = Instantiate(BubblePrefab);

        bubble.GetComponent<SignFollowCar>().Car = newCarGO.transform;

        Car newCar = newCarGO.GetComponent<Car>();
        newCar.slot = slotToQueueIn;
        newCar.Bubble = bubble.transform;
        newCar.Icon = bubble.transform.GetChild(0).GetComponent<SpriteRenderer>();
        newCar.SetRandomService();

        slotToQueueIn.Occupied = newCar;

        audioSrc.Play();
        spawnedCars += 1;

        return newCar;
    }



    private void LoadCarTypes()
    {
        AllCarTypes = Resources.LoadAll<CarType>("Cars");
    }
}
