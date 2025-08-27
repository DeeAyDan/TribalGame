using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    [Header("Prefabs For Classes")]
    public GameObject meleeCardPrefab;
    public GameObject rangedCardPrefab;
    public GameObject supportCardPrefab;

    [Header("UI Parent")]
    public Transform cardParent;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnUnit();
        }
    }

    void SpawnUnit()
    {

        Unit newUnit = UnitFactory.GenerateRandomUnit();

        GameObject prefab = GetCardPrefab(newUnit.Class);
        GameObject cardGo = Instantiate(prefab, cardParent);

        UnitCardUI ui = cardGo.GetComponent<UnitCardUI>();
        ui.Setup(newUnit, null);

    }

    GameObject GetCardPrefab(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Melee:
                return meleeCardPrefab;
            case UnitClass.Ranged:
                return rangedCardPrefab;
            case UnitClass.Support:
                return supportCardPrefab;
            default:
                Debug.LogError("No prefab found for unit class: " + unitClass);
                return null;
        }
    }
}
