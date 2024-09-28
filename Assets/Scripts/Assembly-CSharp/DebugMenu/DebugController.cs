using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DebugController : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isEditor)
            Destroy(this.gameObject);
        else
        {
            this.gc = FindObjectOfType<GameControllerScript>();
            this.player = FindObjectOfType<PlayerScript>();
            this.ToggleMenu("none");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7) && !this.gc.learningActive)
        {
            if (!this.isMenuOpen)
                this.ToggleMenu("main");
            else
                this.ToggleMenu("none");
        }
    }

    private void ToggleMenu(string id)
    {
        this.curSubmenu = id;

        switch(id)
        {
            case "main": // Main menu
                this.isMenuOpen = true;
                this.gc.PauseGame();
                this.menuGroup.SetActive(true);
                this.menuMain.SetActive(true);
                this.menuPlayer.SetActive(false);
                this.menuItems.SetActive(false);
                this.menuItemPicker.SetActive(false);
                break;
            case "player": // Player menu
                this.menuMain.SetActive(false);
                this.menuPlayer.SetActive(true);
                break;
            case "items":
                this.menuMain.SetActive(false);
                this.menuItems.SetActive(true);
                break;
            default: // No menu
                this.isMenuOpen = false;
                this.gc.UnpauseGame();
                this.menuGroup.SetActive(false);
                break;
        }
    }

    public void MenuSwitchAction(string id)
    {
        this.ToggleMenu(id);
    }

    public void ButtonAction(int id)
    {
        switch(this.curSubmenu)
        {
            case "player":
                switch(id)
                {
                    case 1: // Super speed
                        this.player.DebugAction(1);
                        break;
                }
                break;
            case "items":
                this.menuItems.SetActive(false);
                this.menuItemPicker.SetActive(true);
                this.curSubmenu = "items";
                this.gc.itemSelected = id;
                break;
            case "itemPicker":
                this.curSubmenu = "itemPicker";
                this.gc.item[this.gc.itemSelected] = id;
                break;
            default:
                this.ToggleMenu("none");
                break;
        }
    }

    [SerializeField] private bool isMenuOpen;
    [SerializeField] private string curSubmenu;
    [SerializeField] private GameObject menuGroup;
    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuPlayer;
    [SerializeField] private GameObject menuItems;
    [SerializeField] private GameObject menuItemPicker;
    private GameControllerScript gc;
    private PlayerScript player;
}
