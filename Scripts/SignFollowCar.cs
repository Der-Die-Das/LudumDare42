using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignFollowCar : MonoBehaviour
{
    [HideInInspector]
    public Transform Car;

    public Vector2 Offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Car != null)
        {
            transform.position = Car.position + new Vector3(Offset.x,Offset.y,0);
        }
    }
}
