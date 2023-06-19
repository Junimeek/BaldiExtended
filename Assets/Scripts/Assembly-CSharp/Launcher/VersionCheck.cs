using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionCheck : MonoBehaviour
{
    // will do this some other time

    private void Start()
    {
        versionObject.SetActive(false);
    }

    [SerializeField] private GameObject versionObject;
}
