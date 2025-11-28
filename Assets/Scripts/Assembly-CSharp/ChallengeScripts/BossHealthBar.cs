using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] RectTransform barTransform;
    [SerializeField] AudioClip healthSound;
    [SerializeField] Image[] bars;
    [SerializeField] int curHealth;
    [SerializeField] AudioSource audioDevice;

    public void InitializeHealthBar()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].color = Color.clear;
        }
        StartCoroutine(MoveHealthBar());
    }

    IEnumerator MoveHealthBar()
    {
        float newXPos = 0f;

        while (barTransform.anchoredPosition.x < 69f)
        {
            newXPos += Time.unscaledDeltaTime * 370f;
            barTransform.anchoredPosition = new Vector2(newXPos, -88f);
            yield return null;
        }

        barTransform.anchoredPosition = new Vector2(69f, -88f);
        StartCoroutine(FillHealthBar());
    }

    IEnumerator FillHealthBar()
    {
        int healthThing = 0;
        curHealth = 0;

        float delay = 0.6f;
        while (delay > 0f)
        {
            delay -= Time.unscaledDeltaTime;
            yield return null;
        }

        switch(healthThing)
        {
            case 0:
                this.bars[curHealth].color = Color.white;
                this.curHealth++;
                this.audioDevice.Play();
                goto case 1;
            case 1:
                while (this.audioDevice.isPlaying)
                    yield return null;
                goto case 2;
            case 2:
                if (curHealth < 9)
                    goto case 0;
                break;
        }
    }

    public void DecreaseHealthBar()
    {
        curHealth--;
        bars[curHealth].color = Color.clear;
    }

    public void HideHealthBar()
    {
        base.gameObject.SetActive(false);
    }
}
