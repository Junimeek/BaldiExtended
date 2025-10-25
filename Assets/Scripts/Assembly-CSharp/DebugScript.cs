using UnityEngine;

public class DebugScript : MonoBehaviour
{
	private void Start()
	{
		if (this.limitFramerate)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = this.framerate;
		}
	}

	[SerializeField] bool limitFramerate;
	[SerializeField] int framerate;
}
