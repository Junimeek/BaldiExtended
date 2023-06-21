using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerSpawner : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(this.dataManagerPrefab);
    }

    [SerializeField] private DataPersistenceManager dataManagerPrefab;
}
