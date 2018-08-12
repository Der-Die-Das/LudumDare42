using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "service", menuName = "Items/Service", order = 1)]
public class Service : ScriptableObject
{
    //public string Name;
    public Sprite Icon;
    public float Duration;
    public int Profit;
}
