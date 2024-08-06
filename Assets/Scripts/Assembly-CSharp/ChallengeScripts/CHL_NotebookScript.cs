using UnityEngine;

public class CHL_NotebookScript : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && (raycastHit.transform.tag == "Notebook" & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
			{
				base.transform.position = new Vector3(base.transform.position.x, 200f, base.transform.position.z);
				this.gc.CollectNotebook();
				GameObject gameObject = Instantiate(this.learningGame);
				gameObject.GetComponent<CHL_MathGameScript>().gc = this.gc;
				gameObject.GetComponent<CHL_MathGameScript>().baldiScript = this.bsc;
				gameObject.GetComponent<CHL_MathGameScript>().playerPosition = this.player.position;
			}
		}
	}

	public float openingDistance;
	public ChallengeControllerScript gc;
	public BaldiScript bsc;
	public Transform player;
	public GameObject learningGame;
	public AudioSource audioDevice;
}
