using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyController : MonoBehaviour
{
    private int _Money;

    public int Money
    {
        get
        {
            return _Money;
        }
        set
        {
            _Money = value;
            if (MoneyChanged != null)
            {
                MoneyChanged(value);
            }
        }
    }


    public Action<int> MoneyChanged;

}
