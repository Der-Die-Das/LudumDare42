using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text moneyText;
    public Text unlockText;
    public GameObject LostScreen;

    private MoneyController moneyController;


    // Use this for initialization
    void Start()
    {
        moneyController = FindObjectOfType<MoneyController>();
        moneyText.text = "Money: " + moneyController.Money.ToString();
        moneyController.MoneyChanged += MoneyChanged;
        ParkingSlot.UnlockPriceChanged += UnlockPriceChanged;
    }

    private void MoneyChanged(int money)
    {
        moneyText.text = "Money: " + money.ToString();
    }

    private void UnlockPriceChanged(int price)
    {
        unlockText.text = "Price per unlock: " + price.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowLostScreen()
    {
        LostScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        moneyController.MoneyChanged -= MoneyChanged;
        ParkingSlot.UnlockPriceChanged -= UnlockPriceChanged;
    }

}
