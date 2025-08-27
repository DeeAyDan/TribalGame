using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject unitPrefab;

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
        GameObject go = Instantiate(unitPrefab, GetRandomPosition(), Quaternion.identity);
        
        UnitBehaviour behaviour = go.GetComponent<UnitBehaviour>();
        behaviour.Setup(newUnit);
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }
}
