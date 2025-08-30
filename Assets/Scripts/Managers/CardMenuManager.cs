using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool isPopulated = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (cardMenuPanel == null)
        {
            Debug.LogError("CardMenuPanel not assigned!");
            return;
        }

        bool willOpen = !cardMenuPanel.activeSelf;
        cardMenuPanel.SetActive(willOpen);

        if (willOpen)
        {
            Debug.Log("Opening menu and repopulating...");
            PopulateFromSave();
        }
        else
        {
            CloseMenu();
            Debug.Log("Closing menu...");
        }
    }

    public void CloseMenu()
    {
        if (cardMenuPanel != null)
        {
            cardMenuPanel.SetActive(false);
            Debug.Log("Menu closed with X button");
        }
    }

    private void PopulateFromSave()
    {
        if (activeSlotsParent == null || reservedSlotsParent == null)
        {
            Debug.LogError("Active or Reserved slots parent is not assigned. Assign them in the inspector.");
            return;
        }

        List<Unit> allUnits = SaveSystemUnits.LoadAllUnits();
        if (allUnits == null) allUnits = new List<Unit>();

        List<Unit> activeUnits = allUnits.FindAll(u => u.UnitStatus == UnitStatus.Active);
        List<Unit> reservedUnits = allUnits.FindAll(u => u.UnitStatus == UnitStatus.Reserved);

        if (activeUnits.Count > activeSlotsParent.childCount)
        {
            Debug.LogWarning("More active units than active slots — trimming extras into reserved.");
            int extrasCount = activeUnits.Count - activeSlotsParent.childCount;
            var extras = activeUnits.GetRange(activeSlotsParent.childCount, extrasCount);
            activeUnits.RemoveRange(activeSlotsParent.childCount, extrasCount);
            reservedUnits.AddRange(extras);
        }
        if (reservedUnits.Count > reservedSlotsParent.childCount)
        {
            Debug.LogWarning("More reserved units than reserve slots — trimming extras.");
            reservedUnits.RemoveRange(reservedSlotsParent.childCount, reservedUnits.Count - reservedSlotsParent.childCount);
        }

        ClearAllChildren(activeSlotsParent);
        ClearAllChildren(reservedSlotsParent);

        for (int i = 0; i < activeSlotsParent.childCount; i++)
        {
            Transform slot = activeSlotsParent.GetChild(i);
            if (i < activeUnits.Count)
                SpawnCardInSlot(activeUnits[i], slot);
        }

        for (int i = 0; i < reservedSlotsParent.childCount; i++)
        {
            Transform slot = reservedSlotsParent.GetChild(i);
            if (i < reservedUnits.Count)
                SpawnCardInSlot(reservedUnits[i], slot);
        }

        isPopulated = true;
    }

    private void ClearAllChildren(Transform slotsParent)
    {
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            Transform slot = slotsParent.GetChild(i);
            for (int j = slot.childCount - 1; j >= 0; j--)
            {
                GameObject child = slot.GetChild(j).gameObject;
                if (!child.name.Contains("Background") && !child.name.Contains("Slot"))
                {
                    Destroy(child);
                }
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

        StretchToFillParent(card);

        UnitCardUI ui = card.GetComponent<UnitCardUI>();
        if (ui != null) ui.Setup(unit, null);

        if (card.GetComponent<CanvasGroup>() == null) card.AddComponent<CanvasGroup>();
        if (card.GetComponent<CardDragHandler>() == null) card.AddComponent<CardDragHandler>();
    }

    private void StretchToFillParent(GameObject child)
    {
        if (child == null) return;
        RectTransform rt = child.GetComponent<RectTransform>();
        if (rt == null) return;

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        child.transform.localScale = Vector3.one;
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
}
