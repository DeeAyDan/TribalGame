using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public List<Unit> playerUnits = new List<Unit>();

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (playerUnits.Count >= 23)
            {
                Debug.Log("Maximum unit limit reached.");
                return;
            }

            Unit newUnit = UnitFactory.GenerateRandomUnit();
            playerUnits.Add(newUnit);

            if (playerUnits.Count <= 5)
            {
                string slotName = $"ActiveCard{playerUnits.Count}";
                UIManager.AddCard(newUnit, slotName);
            }
            else 
            {
                string slotName = $"ReservedCard{playerUnits.Count - 5}";
                UIManager.AddCard(newUnit, slotName);
            }
        }
    }
}
