using UnityEngine;

public class VendingMachineScript : MonoBehaviour
{
    private void Start()
    {
        this.curQuarterCount = this.InitialQuarterCount();
        this.audioDevice = GetComponent<AudioSource>();
        
        if (!this.disableQuarterRequirement)
            this.display.sprite = this.numbers[this.curQuarterCount];
    }

    public void UseQuarter()
    {
        if (this.vendingMachineType == machineType.Map_Upgrade)
        {
            GameControllerScript gc = FindObjectOfType<GameControllerScript>();

            if (this.curQuarterCount > 0)
            {
                this.curQuarterCount--;
                this.display.sprite = this.numbers[this.curQuarterCount];
                gc.UpgradeMap(this.curQuarterCount);
            }
            else
                gc.UpgradeMap(99);
        }
        else
        {
            if (this.disableQuarterRequirement)
                this.curQuarterCount = 0;
            else
            {
                this.curQuarterCount--;
                if (!(this.curQuarterCount <= 0))
                    this.display.sprite = this.numbers[this.curQuarterCount];
                else this.display.sprite = this.numbers[this.InitialQuarterCount()];
            }
        }

        this.audioDevice.PlayOneShot(this.quarterSound);
    }

    public void ResetQuarterCount()
    {
        this.curQuarterCount = this.InitialQuarterCount();
    }

    public int DispensedItem()
    {
        switch(this.vendingMachineType)
        {
            case machineType.BSODA:
                return 4;
            case machineType.Zesty:
                return 1;
            case machineType.Diet_BSODA:
                return 13;
            case machineType.Crystal_Zesty:
                return 14;
            default:
                return 5;
        }
    }

    private int InitialQuarterCount()
    {
        switch(this.vendingMachineType)
        {
            case machineType.BSODA:
                return 3;
            case machineType.Zesty:
                return 3;
            case machineType.Diet_BSODA:
                 return 1;
            case machineType.Crystal_Zesty:
                return 1;
            case machineType.Map_Upgrade:
                return 4;
            default:
                return 1;
        }
    }

    [Header("Configuration")]
    [SerializeField] private bool disableQuarterRequirement;
    public machineType vendingMachineType;
    public enum machineType
    {
        BSODA, Zesty, Diet_BSODA, Crystal_Zesty, Map_Upgrade
    }
    [Header("Current State")]
    public int curQuarterCount;
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private SpriteRenderer display;
    [SerializeField] private AudioClip quarterSound;
    private AudioSource audioDevice;
}
