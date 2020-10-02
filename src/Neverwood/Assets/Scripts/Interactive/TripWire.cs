using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProximityTrigger))]
public class TripWire : MonoBehaviour
{
    public float audioTime = 2f;
    public float audioRange = 15f;
    private void Awake()
    {
        GetComponent<ProximityTrigger>().triggered.AddListener(Trip);
    }
    public void Trip()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Hearing");
        AuditoryCue auditoryCue = gameObject.AddComponent<AuditoryCue>();
        auditoryCue.length = audioTime;
        auditoryCue.range = audioRange;
        Destroy(this.gameObject, audioTime);
    }
}
