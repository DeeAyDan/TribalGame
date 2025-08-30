using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Items")]
    int defenseBoostCount;
    int attackBoostCount;
    int speedBoostCount;
    int healCount;
    [SerializeField] TMP_Text defenseBoostCountText;
    [SerializeField] TMP_Text attackBoostCountText;
    [SerializeField] TMP_Text speedBoostCountText;
    [SerializeField] TMP_Text healCountText;


    [Header("Food")]
    int berriesCount;
    int carrotsCount;
    int meatCount;
    [SerializeField] TMP_Text berriesCountText;
    [SerializeField] TMP_Text carrotsCountText;
    [SerializeField] TMP_Text meatCountText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadInventory();
    }

    public void AddDefenseBoost(int amount)
    {
        defenseBoostCount += amount;
        defenseBoostCountText.text = defenseBoostCount.ToString();
    }

    public void AddAttackBoost(int amount)
    {
        attackBoostCount += amount;
        attackBoostCountText.text = attackBoostCount.ToString();
    }

    public void AddSpeedBoost(int amount)
    {
        speedBoostCount += amount;
        speedBoostCountText.text = speedBoostCount.ToString();
    }

    public void AddHeal(int amount)
    {
        healCount += amount;
        healCountText.text = healCount.ToString();
    }

    #region Food Methods
    public void AddBerries(int amount)
    {
        berriesCount += amount;
        if (berriesCount < 0) berriesCount = 0;
        berriesCountText.text = berriesCount.ToString();
    }

    public void AddCarrots(int amount)
    {
        carrotsCount += amount;
        if (carrotsCount < 0) carrotsCount = 0;
        carrotsCountText.text = carrotsCount.ToString();
    }

    public void AddMeat(int amount)
    {
        meatCount += amount;
        if (meatCount < 0) meatCount = 0;
        meatCountText.text = meatCount.ToString();
    }

    // Getters
    public int GetBerriesCount() => berriesCount;
    public int GetCarrotsCount() => carrotsCount;
    public int GetMeatCount() => meatCount;
    #endregion


    #region Save/Load
    public void SaveInventory()
    {
        InventorySaveData data = new()
        {
            defenseBoostCount = defenseBoostCount,
            attackBoostCount = attackBoostCount,
            speedBoostCount = speedBoostCount,
            healCount = healCount,
            berriesCount = berriesCount,
            carrotsCount = carrotsCount,
            meatCount = meatCount
        };

        InventorySaveSystem.SaveInventory(data);
    }

    public void LoadInventory()
    {
        InventorySaveData data = InventorySaveSystem.LoadInventory();
        if (data == null) return;

        defenseBoostCount = data.defenseBoostCount;
        attackBoostCount = data.attackBoostCount;
        speedBoostCount = data.speedBoostCount;
        healCount = data.healCount;
        berriesCount = data.berriesCount;
        carrotsCount = data.carrotsCount;
        meatCount = data.meatCount;

        // Update UI
        defenseBoostCountText.text = defenseBoostCount.ToString();
        attackBoostCountText.text = attackBoostCount.ToString();
        speedBoostCountText.text = speedBoostCount.ToString();
        healCountText.text = healCount.ToString();
        berriesCountText.text = berriesCount.ToString();
        carrotsCountText.text = carrotsCount.ToString();
        meatCountText.text = meatCount.ToString();
    }
    #endregion

}

[System.Serializable]
public class InventorySaveData
{
    public int defenseBoostCount;
    public int attackBoostCount;
    public int speedBoostCount;
    public int healCount;

    public int berriesCount;
    public int carrotsCount;
    public int meatCount;
}
