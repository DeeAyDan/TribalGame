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

    List<Unit> allUnits = new List<Unit>();

    private void Start()
    {
           allUnits = SaveSystemUnits.LoadAllUnits();

        if (allUnits.Count == 0)
        {
            SpawnUnit(100);
            SaveSystemUnits.SaveAllUnits(allUnits);
        }
    }

    private void Update()
    {
 
    }

    void SpawnUnit(int unitCount)
    {

        for (int i = 0; i < unitCount; i++)
        {
            Unit newUnit = UnitFactory.GenerateRandomUnit();
            allUnits.Add(newUnit);

        }
    }
}
