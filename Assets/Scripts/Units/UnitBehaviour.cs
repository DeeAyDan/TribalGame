using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{

    public Unit data;

    public void Setup(Unit unitData)
    {
        data = unitData;
        gameObject.name = $"{unitData.ID}";

        var renderer = GetComponent<Renderer>();
    }
}
