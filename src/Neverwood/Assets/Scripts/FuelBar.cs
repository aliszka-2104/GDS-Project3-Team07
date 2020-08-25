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
        lantern = GameObject.FindObjectOfType<Lantern>();
    }

    void Update()
    {
        image.fillAmount = lantern.lanternLeft / lantern.lanternLength;
    }
}
