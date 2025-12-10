using System;
using System.Collections;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    [Header("Game State")]
    public int darkModePhase;

    [Header("Scripts")]
    [SerializeField] GameControllerScript gc;
    [SerializeField] BaldiScript baldiScript;

    [Header("Dark Mode")]
	[SerializeField] GameObject nullBoss;
    [SerializeField] NullBoss nullBossScript;
	public GameObject[] windowBlockers;
	[SerializeField] GameObject projectile;
	public int createdProjectiles;
	public GameObject[] projectilesInPlay;

    void Awake()
    {
        this.darkModePhase = 1;
    }

    public void EndChallengeGame(int type)
    {
        switch(type)
        {
            case 1:
                StartCoroutine(this.NullKill());
                break;
        }
    }

    IEnumerator NullKill()
    {
        if (this.darkModePhase == 1)
        {
            RenderSettings.ambientLight = Color.black;
            this.baldiScript.NullOffset();
        }
        else
        {
            Transform funnyNullSprite = this.nullBoss.transform.Find("Sprite");
			Vector3 curPosition = funnyNullSprite.transform.position;
			funnyNullSprite.position = new Vector3(curPosition.x, curPosition.y - 0.5f, curPosition.z);
        }

        gc.isDynamicColor = false;
        RenderSettings.ambientIntensity = 0f;

        float remTime = 0f;
        float targetTime = 4f;
        int lightState = 0;
        int curLightState = 0;
        float colorDivisor = 8f;

        switch (lightState)
        {
            case 0:
                remTime += targetTime;
                RenderSettings.ambientIntensity += Time.unscaledDeltaTime / 7f;
                RenderSettings.ambientLight += new Color(Time.unscaledDeltaTime / colorDivisor, Time.unscaledDeltaTime / colorDivisor, Time.unscaledDeltaTime / colorDivisor);
                remTime -= Time.unscaledDeltaTime;
                while (remTime > 0f)
                {
                    remTime -= Time.unscaledDeltaTime;
                    yield return null;
                }
                if (curLightState == 0)
                    goto case 1;
                else if (curLightState == 5)
                    goto case 3;
                else if (curLightState == 6)
                    goto case 4;
                else
                    goto case 2;
            case 1:
                RenderSettings.ambientLight = GetRandomColor();
                colorDivisor = 4f;
                targetTime = 2f;
                curLightState++;
                goto case 0;
            case 2:
                RenderSettings.ambientLight = GetRandomColor();
                colorDivisor = 4f;
                targetTime = 1f;
                curLightState++;
                goto case 0;
            case 3:
                RenderSettings.ambientLight = new Color(0.8f, 0f, 0f);
                targetTime = 2f;
                curLightState++;
                goto case 0;
            case 4:
                break;
        }
    }

    UnityEngine.Color GetRandomColor()
    {
        float r = UnityEngine.Random.Range(0f, 1f);
		float g = UnityEngine.Random.Range(0f, 1f);
		float b = UnityEngine.Random.Range(0f, 1f);

		return new Color(r, g, b);
    }

    public IEnumerator ToggleWindowBlockers()
	{
		this.baldiScript.allowWindowBreaking = true;

		for (int i = 0; i < this.windowBlockers.Length; i++)
			this.windowBlockers[i].SetActive(false);

		while (this.baldiScript.currentPriority > 1)
			yield return null;
		
		for (int i = 0; i < this.windowBlockers.Length; i++)
			this.windowBlockers[i].SetActive(true);
		
		this.baldiScript.allowWindowBreaking = false;
	}

    public void EnableAllWindowBlockers()
	{
		StopCoroutine(this.ToggleWindowBlockers());

		this.baldiScript.currentPriority = 0;
		this.baldiScript.allowWindowBreaking = false;

		for (int i = 0; i < this.windowBlockers.Length; i++)
			this.windowBlockers[i].SetActive(true);
	}

    public void ActivateNullBossFight(Vector3 nullPosition)
    {
        gc.isDynamicColor = false;
        this.nullBoss.SetActive(true);
        this.nullBossScript.WarpToExit(nullPosition);
        RenderSettings.ambientLight = new Color(1f, 1f, 1f);

        this.darkModePhase = 2;
        this.createdProjectiles = 3;

        gc.TemporaryBossActivation();
        gc.SwitchToMinimalUI();
    }

    	public void InitializeProjectileArrays()
    {
        this.projectilesInPlay = new GameObject[6];
    }

	public void CreateProjectile(byte number)
	{
		this.createdProjectiles += number;
		Vector3 newProjectileLocation = Vector3.zero;

		for (int i = this.projectilesInPlay.Length - number; i < this.projectilesInPlay.Length; i++)
        {
			newProjectileLocation = GetNewProjectileLocation();
			GameObject projectileToPlace = Instantiate(this.projectile, newProjectileLocation, Quaternion.identity);
            SetProjectileInFirstAvailableSlot(projectileToPlace);
        }
	}

	Vector3 GetNewProjectileLocation()
    {
        Vector3 newLocation = gc.GetNewWanderLocation("Projectile");
		int checkStep = 0;
		switch(checkStep)
        {
            case 0:
				if (IsProjectileOverlap(newLocation))
					goto case 1;
				else
					return newLocation;
			case 1:
				newLocation = gc.GetNewWanderLocation("Projectile");
				goto case 0;
        }
		return newLocation; // this is just here because the compiler throws an error without it
    }

	bool IsProjectileOverlap(Vector3 locationToCheck)
    {
        for (int i = 0; i < projectilesInPlay.Length; i++)
        {
			if (projectilesInPlay[i] == null)
				continue;
            else if (projectilesInPlay[i].transform.position == locationToCheck)
				return true;
			else
				continue;
        }
		return false;
    }

	void SetProjectileInFirstAvailableSlot(GameObject projectileToPlace)
    {
        for (int i = 0; i < projectilesInPlay.Length; i++)
        {
            if (projectilesInPlay[i] == null)
            {
                projectilesInPlay[i] = projectileToPlace;
				break;
            }
        }
    }

	public void CheckProjectileCount(NullBoss nullBoss)
	{
		byte hits = nullBoss.hits;
		byte Hitclip()
		{
			if (nullBoss.isFightStarted)
				return 4;
			else
				return 0;
		}
		byte[] allowedProjectiles = {
			1, Hitclip(), 3, 3, 3, 3, 3, 3, 2, 1, 0
		};

		if (this.createdProjectiles < allowedProjectiles[hits])
			this.CreateProjectile(1);
	}

	public void DeleteProjectiles()
	{
		foreach (GameObject i in this.projectilesInPlay)
			Destroy(i);
		
		Array.Resize(ref this.projectilesInPlay, 0);
		this.createdProjectiles = 0;
	}

    public void DeactivateBossFight()
	{
		gc.ModifyExits("raise");
	}
}
