using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject DialogWindow;
    public Text DialogText;
    public Button NextButton;
    [Header("Dialogs")]
    public TutorialDialog[] Dialogs;

    private GameManager gm;
    private CarSpawner spawner;

    private int ActiveDialog = 0;
    // Use this for initialization
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<CarSpawner>();
        if (gm.PlayTutorial == false)
        {
            spawner.StartSpawning();
            DestroyImmediate(gameObject);
            return;
        }
        DialogWindow.SetActive(true);
        DialogText.text = Dialogs[ActiveDialog].Text;
        Dialogs[ActiveDialog].Show();

    }

    public void Next()
    {
        Dialogs[ActiveDialog].Hide();
        ActiveDialog++;

        if (ActiveDialog > Dialogs.Length - 1)
        {
            //tut done
            gm.TutorialPlayed();
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else
        {
            DialogText.text = Dialogs[ActiveDialog].Text.Replace(" ", "   ");
            Dialogs[ActiveDialog].Show();
            AssignEventsAndStuff();
        }
    }

    private void AssignEventsAndStuff()
    {
        if (Dialogs[ActiveDialog].SpawnCar)
        {
            spawner.SpawnRepairCar();
        }
        if (Dialogs[ActiveDialog].WaitForServiceCompleted)
        {
            NextButton.gameObject.SetActive(false);
            ServiceSlot.ServiceDone += Next;
            return;
        }
        else
        {
            NextButton.gameObject.SetActive(true);
            ServiceSlot.ServiceDone -= Next;
        }

        if (Dialogs[ActiveDialog].WaitForDepartureCompleted)
        {
            NextButton.gameObject.SetActive(false);
            IdleSlot.Departured += Next;
            return;
        }
        else
        {
            NextButton.gameObject.SetActive(true);
            IdleSlot.Departured -= Next;
        }

        if (Dialogs[ActiveDialog].WaitForSpotBought)
        {
            NextButton.gameObject.SetActive(false);
            CarMover.SpotBought += Next;
            FindObjectOfType<MoneyController>().Money += 30;
            return;
        }
        else
        {
            NextButton.gameObject.SetActive(true);
            CarMover.SpotBought -= Next;
        }
    }
}
