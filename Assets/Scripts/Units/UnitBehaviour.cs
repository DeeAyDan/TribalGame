using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{

    public Unit data;

    public void Setup(Unit unitData)
    {
        data = unitData;
        gameObject.name = $"{unitData.Name} ({unitData.SubClass})";

        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        { 
            switch (unitData.Class)
            {
                case UnitClass.Melee:
                    renderer.material.color = Color.red;
                    break;
                case UnitClass.Ranged:
                    renderer.material.color = Color.blue;
                    break;
                case UnitClass.Support:
                    renderer.material.color = Color.green;
                    break;
                default:
                    renderer.material.color = Color.yellow;
                    break;
            }
        }
        Debug.Log($"Spawned {unitData.Name}, a {unitData.SubClass} ({unitData.Class}) with {unitData.Attack} ATK, {unitData.Defense} DEF, {unitData.Health} HP, {unitData.Speed} SPD, Age: {unitData.Age}, Rarity: {unitData.Rarity}");
    }
}
