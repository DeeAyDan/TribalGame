using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject cardMenuPanel;
    public Transform activeSlotsParent;
    public Transform reservedSlotsParent;

    [Header("Card Prefabs by Class (assign)")]
    public GameObject meleeCardPrefab;
    public GameObject rangedCardPrefab;
    public GameObject supportCardPrefab;

    private bool isOpen = false;

    private void Start()
    {
        if (cardMenuPanel != null) cardMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
            ToggleMenu();
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        if (cardMenuPanel != null)
        {
            cardMenuPanel.SetActive(isOpen);
            if (isOpen) PopulateFromSave();
        }
    }

    // Hook this to the X button onClick
    public void CloseMenu()
    {
        isOpen = false;
        if (cardMenuPanel != null) cardMenuPanel.SetActive(false);
    }

    private void PopulateFromSave()
    {
        List<Unit> allUnits = SaveSystem.LoadAllUnits();

        List<Unit> activeUnits = allUnits.FindAll(u => u.UnitStatus == UnitStatus.Active);
        List<Unit> reservedUnits = allUnits.FindAll(u => u.UnitStatus == UnitStatus.Reserved);

        if (activeUnits.Count > activeSlotsParent.childCount)
        {
            Debug.LogWarning("More active units than slots — trimming extras into reserved.");
            var extras = activeUnits.GetRange(activeSlotsParent.childCount, activeUnits.Count - activeSlotsParent.childCount);
            activeUnits.RemoveRange(activeSlotsParent.childCount, extras.Count);
            reservedUnits.AddRange(extras);
        }

        ClearChildren(activeSlotsParent);
        ClearChildren(reservedSlotsParent);

        // Fill active
        for (int i = 0; i < activeSlotsParent.childCount; i++)
        {
            Transform slot = activeSlotsParent.GetChild(i);
            if (i < activeUnits.Count)
            {
                SpawnCardInSlot(activeUnits[i], slot);
            }
        }

        // Fill reserved
        for (int i = 0; i < reservedSlotsParent.childCount; i++)
        {
            Transform slot = reservedSlotsParent.GetChild(i);
            if (i < reservedUnits.Count)
            {
                SpawnCardInSlot(reservedUnits[i], slot);
            }
        }
    }

    private void SpawnCardInSlot(Unit unit, Transform slot)
    {
        GameObject prefab = GetCardPrefab(unit.Class);
        if (prefab == null)
        {
            Debug.LogError("No prefab found for class " + unit.Class);
            return;
        }

        GameObject card = Instantiate(prefab, slot, false);
        UnitCardUI ui = card.GetComponent<UnitCardUI>();
        if (ui != null) ui.Setup(unit, null);

        if (card.GetComponent<CanvasGroup>() == null) card.AddComponent<CanvasGroup>();
        if (card.GetComponent<CardDragHandler>() == null) card.AddComponent<CardDragHandler>();
    }

    private GameObject GetCardPrefab(UnitClass c)
    {
        switch (c)
        {
            case UnitClass.Melee: return meleeCardPrefab;
            case UnitClass.Ranged: return rangedCardPrefab;
            case UnitClass.Support: return supportCardPrefab;
            default: return meleeCardPrefab;
        }
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }
    }
}