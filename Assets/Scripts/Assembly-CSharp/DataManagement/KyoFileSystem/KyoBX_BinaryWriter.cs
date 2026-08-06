using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class KyoBX_BinaryWriter : MonoBehaviour
{
    // send help i have absolutely no idea what im doing
    public void StartWrite()
    {
        
    }

    public int saveFileVersion;
    public uint[] itemsUsed;
    [SerializeField] byte[] rawFileOutput;
}

public class KyoStoryData
{
    public uint[] lifetimeItems;
    public uint bestTimeInMS;

    public KyoStoryData()
    {
        
    }
}