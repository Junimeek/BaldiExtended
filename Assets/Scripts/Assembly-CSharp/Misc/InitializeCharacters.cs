using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCharacters : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerPosition;
    private void Awake()
    {
        player.SetActive(true);
    }

    private void Start()
    {
        player.transform.position = playerPosition;
    }
}
