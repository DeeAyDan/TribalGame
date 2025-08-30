using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
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
        attackBoostCountText.text = defenseBoostCount.ToString();
    }

    public void AddSpeedBoost(int amount)
    {
        speedBoostCount += amount;
        speedBoostCountText.text = defenseBoostCount.ToString();
    }

    public void AddHeal(int amount)
    {
        healCount += amount;
        healCountText.text = defenseBoostCount.ToString();
    }

    public void AddBerries(int amount)
    {
        berriesCount += amount;
        berriesCountText.text = defenseBoostCount.ToString();
    }

    public void AddCarrots(int amount)
    {
        carrotsCount += amount;
        carrotsCountText.text = defenseBoostCount.ToString();
    }

    public void AddMeating(int amount)
    {
        meatCount += amount;
        meatCountText.text = defenseBoostCount.ToString();
    }

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
