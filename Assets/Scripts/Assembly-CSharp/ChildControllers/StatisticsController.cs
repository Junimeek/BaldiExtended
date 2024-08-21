using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsController : MonoBehaviour
{
    private void Start()
    {
        this.finalSeconds = 0f;
        this.itemsUsed = new int[0];
        this.detentions = 0;
    }

    public bool disableSaving;
    public float finalSeconds;
    public int[] itemsUsed;
    public int detentions;
}
