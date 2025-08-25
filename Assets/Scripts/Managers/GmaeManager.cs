using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GmaeManager : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Unit newUnit = UnitFactory.GenerateRandomUnit();
            units.Add(newUnit);
            Debug.Log($"Generated Unit: {newUnit.Name}, Class: {newUnit.Class}, SubClass: {newUnit.SubClass}, Rarity: {newUnit.Rarity}, Stats - ATK: {newUnit.Attack}, DEF: {newUnit.Defense}, HP: {newUnit.Health}, SPD: {newUnit.Speed}");
        }
    }
}
