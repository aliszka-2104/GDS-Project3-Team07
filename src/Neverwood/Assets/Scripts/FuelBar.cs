using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    public Image image;
    private Lantern lantern;

    void Start()
    {
        lantern = FindObjectOfType<Lantern>();
    }

    void Update()
    {
        if (lantern)
        {
            image.fillAmount = lantern.lanternLeft / lantern.lanternLength;
        }
        else
        {
            lantern = FindObjectOfType<Lantern>();
        }
    }
}
