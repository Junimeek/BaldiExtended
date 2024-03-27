using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class DetentionTextScript : MonoBehaviour
{
	// Token: 0x0600002B RID: 43 RVA: 0x00002A10 File Offset: 0x00000E10
	private void Start()
	{
		this.text = base.GetComponent<TMP_Text>();
		//this.board.SetActive(false);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002A20 File Offset: 0x00000E20
	private void Update()
	{
		/*
		if (this.door.lockTime > 0f)
		{
			this.text.text = "You have detention! \n" + Mathf.CeilToInt(this.door.lockTime) + " seconds remain!";
			this.board.SetActive(true);
		}
		else
		{
			this.text.text = string.Empty;
			this.board.SetActive(false);
		}
		*/
		return;
	}

	// Token: 0x0400002B RID: 43
	public DoorScript door;

	// Token: 0x0400002C RID: 44
	[SerializeField] private TMP_Text text;
	[SerializeField] private GameObject board;
}
