using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class QuarterSpawnScript : MonoBehaviour
{
	private void Start()
	{
		base.transform.position = this.wanderer.NewTarget("Quarter") + Vector3.up * 4f;
	}

	public AILocationSelectorScript wanderer;
	public Transform location;
}
