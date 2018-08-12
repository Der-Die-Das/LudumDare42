using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public Transform TopStart;
    public Transform BottomStart;

    public float Speed;

    private CarType[] allCarTypes;

    private List<CarStatus> spawnedCars;

    // Use this for initialization
    void Start()
    {
        allCarTypes = Resources.LoadAll<CarType>("Cars");
        SpawnCars();
        StartCoroutine(SpawnNewCars());
    }

    private void SpawnCars()
    {
        spawnedCars = new List<CarStatus>();

        foreach (CarType type in allCarTypes)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(type.Prefab);
                Destroy(go.GetComponent<Car>());
                go.SetActive(false);
                spawnedCars.Add(new CarStatus(go));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void SpawnNewCar()
    {
        CarStatus car = null;
        List<CarStatus> possibleCars = new List<CarStatus>();
        foreach (var spawnedCar in spawnedCars)
        {
            if (spawnedCar.State == CarStatus.CarState.Idle)
            {
                possibleCars.Add(spawnedCar);
            }
        }
        if (possibleCars.Count > 0)
        {
            car = possibleCars[Random.Range(0, possibleCars.Count)];
        }


        if (car == null)
        {
            return;
        }

        car.State = (CarStatus.CarState)(int)Random.Range(1, 3);

        if (car.State == CarStatus.CarState.DownTop)
        {
            car.Car.transform.position = BottomStart.position;
            car.Car.transform.rotation = BottomStart.rotation;
        }
        else
        {
            car.Car.transform.position = TopStart.position;
            car.Car.transform.rotation = TopStart.rotation;
        }

        car.Car.SetActive(true);
    }

    private IEnumerator SpawnNewCars()
    {
        while (true)
        {
            SpawnNewCar();
            yield return new WaitForSeconds(Random.Range(0.5f, 2));
        }
    }

    private void Move()
    {
        foreach (var car in spawnedCars)
        {
            if (car.State != CarStatus.CarState.Idle)
            {
                float multiplier = car.State == CarStatus.CarState.TopDown ? -1f : 1f;
                car.Car.transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));

                if (multiplier > 0)
                {
                    if (car.Car.transform.position.y >= TopStart.position.y)
                    {
                        car.State = CarStatus.CarState.Idle;
                        car.Car.SetActive(false);
                        return;
                    }
                }
                else
                {
                    if (car.Car.transform.position.y <= BottomStart.position.y)
                    {
                        car.State = CarStatus.CarState.Idle;
                        car.Car.SetActive(false);
                        return;
                    }
                }
            }
        }
    }
}

class CarStatus
{
    public GameObject Car;
    public enum CarState { Idle = 0, TopDown = 1, DownTop = 2 }
    public CarState State;

    public CarStatus(GameObject car)
    {
        Car = car;
        State = CarState.Idle;
    }
}
