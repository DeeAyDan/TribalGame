using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class FoodDeliveryPanel : MonoBehaviour
{
    [Header("UI References")]
    public Transform reservedUnitsContainer;
    public Button confirmButton;
    public Button cancelButton;

    private Vector3 originalPos;
    private Vector3 originalScale;

    [Header("Food UI")]
    public Button meatAddButton;
    public Button meatRemoveButton;
    public TextMeshProUGUI meatCountText;

    public Button carrotAddButton;
    public Button carrotRemoveButton;
    public TextMeshProUGUI carrotCountText;

    public Button berriesAddButton;
    public Button berriesRemoveButton;
    public TextMeshProUGUI berriesCountText;

    [Header("Prefabs")]
    public GameObject meleeCardPrefab;
    public GameObject rangedCardPrefab;
    public GameObject supportCardPrefab;

    private UnitCardUI selectedUnit;

    private Dictionary<string, int> foodSelection = new Dictionary<string, int>();

    private void Awake()
    {
        foodSelection["Meat"] = 0;
        foodSelection["Carrot"] = 0;
        foodSelection["Berries"] = 0;


        meatAddButton.onClick.AddListener(() => AdjustFoodCount("Meat", 1));
        meatRemoveButton.onClick.AddListener(() => AdjustFoodCount("Meat", -1));

        carrotAddButton.onClick.AddListener(() => AdjustFoodCount("Carrot", 1));
        carrotRemoveButton.onClick.AddListener(() => AdjustFoodCount("Carrot", -1));

        berriesAddButton.onClick.AddListener(() => AdjustFoodCount("Berries", 1));
        berriesRemoveButton.onClick.AddListener(() => AdjustFoodCount("Berries", -1));


        confirmButton.onClick.AddListener(ConfirmDelivery);
        cancelButton.onClick.AddListener(CancelDelivery);
    }

    private void OnEnable()
    {
        OpenPanel();
    }

    public void OpenPanel()
    {
        Debug.Log("OpenPanel called");

        PopulateReservedUnits();
        selectedUnit = null;

        ResetFoodSelection();
    }

    public void CancelDelivery()
    {
        if (selectedUnit != null)
        {
            selectedUnit.transform.localPosition = originalPos;
            selectedUnit.transform.localScale = originalScale;
            selectedUnit = null;
        }

        ClearReservedUnits();
    }

    private void PopulateReservedUnits()
    {
        ClearReservedUnits();

        List<Unit> allUnits = SaveSystemUnits.LoadAllUnits();
        foreach (Unit unit in allUnits)
        {
            if (unit.UnitStatus == UnitStatus.Reserved)
            {
                GameObject cardObj = Instantiate(GetCardPrefab(unit.Class), reservedUnitsContainer);
                UnitCardUI ui = cardObj.GetComponent<UnitCardUI>();
                ui.Setup(unit);

                CardDragHandler dropHandler = cardObj.GetComponent<CardDragHandler>();
                if (dropHandler != null)
                {
                    dropHandler.enabled = false;
                }

                Button cardButton = cardObj.AddComponent<Button>();
                cardButton.onClick.AddListener(() => SelectUnit(ui));
            }
        }
    }

    private void ClearReservedUnits()
    {
        foreach (Transform child in reservedUnitsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void SelectUnit(UnitCardUI unitUI)
    {
        if (selectedUnit != null)
        {
            selectedUnit.transform.localPosition = originalPos;
            selectedUnit.transform.localScale = originalScale;
        }

        selectedUnit = unitUI;
        originalPos = unitUI.transform.localPosition;
        originalScale = unitUI.transform.localScale;
        unitUI.transform.localPosition += new Vector3(0, 20f, 0);
        unitUI.transform.localScale = originalScale * 1.1f;
    }

    private void AdjustFoodCount(string foodType, int delta)
    {
        foodSelection[foodType] = Mathf.Max(0, foodSelection[foodType] + delta);
        UpdateFoodTexts();
    }

    private void ResetFoodSelection()
    {
        foodSelection["Meat"] = 0;
        foodSelection["Carrot"] = 0;
        foodSelection["Berries"] = 0;
        UpdateFoodTexts();
    }

    private void UpdateFoodTexts()
    {
        meatCountText.text = foodSelection["Meat"].ToString();
        carrotCountText.text = foodSelection["Carrot"].ToString();
        berriesCountText.text = foodSelection["Berries"].ToString();
    }

    private void ConfirmDelivery()
    {
        if (selectedUnit == null)
        {
            Debug.LogWarning("Must select a unit!");
            return;
        }

        int totalFood = 0;
        foreach (var kvp in foodSelection) totalFood += kvp.Value;
        if (totalFood == 0)
        {
            Debug.LogWarning("Must select at least one food!");
            return;
        }

        InventoryManager inv = InventoryManager.Instance;

        if (inv == null)
        {
            Debug.LogError("InventoryManager.Instance is null!");
            return;
        }

        Debug.Log($"About to check berries. Selection: {foodSelection["Berries"]}");

        // Berries - Add try-catch to isolate the exact issue
        if (foodSelection["Berries"] > 0)
        {
            try
            {
                int berriesCount = inv.GetBerriesCount();
                Debug.Log($"Current berries count: {berriesCount}");

                if (foodSelection["Berries"] > berriesCount)
                {
                    Debug.LogWarning("Not enough Berries!");
                    return;
                }
                inv.AddBerries(-foodSelection["Berries"]);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error in berries check: {e.Message}");
                Debug.LogError($"Stack trace: {e.StackTrace}");
                return;
            }
        }

        // Rest of your code...
    }

    private GameObject GetCardPrefab(UnitClass c)
    {
        switch (c)
        {
            case UnitClass.Melee: return meleeCardPrefab;
            case UnitClass.Ranged: return rangedCardPrefab;
            case UnitClass.Support: return supportCardPrefab;
            default: return null;
        }
    }


}
