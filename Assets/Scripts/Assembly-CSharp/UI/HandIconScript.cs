using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HandIconScript : MonoBehaviour
{
    private void Start()
    {
        this.icon.sprite = this.recticle;
    }

    public void ChangeIcon(int icon)
    {
        switch(icon)
        {
            case 1:
                this.icon.sprite = this.hand;
            break;
            case 2:
                this.icon.sprite = this.thinkpad;
            break;
            case 3:
                this.icon.sprite = this.quarter;
            break;
            case 4:
                this.icon.sprite = this.keys;
            break;
            case 5:
                this.icon.sprite = this.tape;
            break;
            case 6:
                this.icon.sprite = this.doorlock;
            break;
            case 7:
                this.icon.sprite = this.nosquee;
            break;
            case 8:
                this.icon.sprite = this.scissors;
            break;
            default:
                this.icon.sprite = this.recticle;
            break;
        }
    }

    [SerializeField] private Image icon;

    [SerializeField] private Sprite recticle;
    [SerializeField] private Sprite hand;
    [SerializeField] private Sprite thinkpad;
    [SerializeField] private Sprite quarter;
    [SerializeField] private Sprite keys;
    [SerializeField] private Sprite tape;
    [SerializeField] private Sprite doorlock;
    [SerializeField] private Sprite nosquee;
    [SerializeField] private Sprite scissors;
}
