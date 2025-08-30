using System.IO;
using UnityEngine;

public static class InventorySaveSystem
{
    private static string inventoryPath = Application.persistentDataPath + "/inventory.json";

    public static void SaveInventory(InventorySaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(inventoryPath, json);
        Debug.Log("Inventory saved to: " + inventoryPath);
    }

    public static InventorySaveData LoadInventory()
    {
        if (File.Exists(inventoryPath))
        {
            string json = File.ReadAllText(inventoryPath);
            return JsonUtility.FromJson<InventorySaveData>(json);
        }
        return null;
    }

    public static void DeleteInventory()
    {
        if (File.Exists(inventoryPath))
        {
            File.Delete(inventoryPath);
        }
    }
}
