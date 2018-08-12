using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class ServiceSlot : ParkingSlot
{
    [SerializeField]
    private Service service = null;

    private float blinkSpeed = 0.3f;

    public Service _Service
    {
        get
        {
            return service;
        }
    }

    public Image progressImage;

    private float serviceStartTime;

    private AudioSource audioSrc;

    public static Action ServiceDone;

    private Color standartColor;

    public Gradient warnGradient;

    public float maxValue;

    private CarMover mover;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        audioSrc = GetComponent<AudioSource>();
        progressImage.fillAmount = 0;
        progressImage.transform.parent.gameObject.SetActive(false);


        standartColor = progressImage.color;

        mover = FindObjectOfType<CarMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Occupied)
        {
            float serviceDuration = Time.time - serviceStartTime;
            float progress = 1f / service.Duration * serviceDuration;
            progress = Mathf.Clamp01(progress);
            progressImage.fillAmount = progress;
            if (progress >= 1)
            {
                float value = Remap(serviceDuration - service.Duration, 0, maxValue, 0, 1);
                if (value >= 1)
                {
                    Occupied.slot = null;
                    Destroy(Occupied.gameObject);
                    Occupied = null;
                    mover.departureSound.Play();
                    return;
                }
                Color warnColor = warnGradient.Evaluate(value);
                if (Occupied.servicesDone.Contains(service) == false)
                {
                    Occupied.servicesDone.Add(service);
                    audioSrc.Play();
                    if (ServiceDone != null)
                    {
                        ServiceDone();
                    }
                    
                    StartCoroutine(Blink());
                }
                progressImage.color = warnColor;
            }
        }
    }

    private IEnumerator Blink()
    {
        bool on = true;
        while (true)
        {
            yield return new WaitForSeconds(blinkSpeed);
            on = !on;
            progressImage.gameObject.SetActive(on);
        }
    }


    protected override void CarAssigned(Car car)
    {
        serviceStartTime = Time.time;
        progressImage.transform.parent.gameObject.SetActive(true);
        progressImage.color = standartColor;
        progressImage.gameObject.SetActive(true);
    }

    protected override void CarRemoved(Car car)
    {
        progressImage.fillAmount = 0;
        progressImage.transform.parent.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public override bool CanGetAssigned(Car car)
    {
        if (car.neededServices.Contains(service))
        {
            return true;
        }
        return false;
    }
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}

