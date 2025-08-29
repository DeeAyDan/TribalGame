using UnityEngine;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;

[System.Serializable]
public class UnitListWrapper
{
    public List<Unit> units;
    public UnitListWrapper(List<Unit> units)
    {
        this.units = units;
    }
}

public class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/units.json";
    public static void SaveAllUnits(List<Unit> allUnits)
    {
        UnitListWrapper wrapper = new UnitListWrapper(allUnits);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"Saved units");
    }

    public static List<Unit> LoadAllUnits()
    {

        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            UnitListWrapper wrapper = JsonUtility.FromJson<UnitListWrapper>(json);
            
            if(wrapper != null && wrapper.units != null)
            {
                Debug.Log($"Loaded {wrapper.units.Count} units");
                return wrapper.units;
            }
        }
        Debug.Log("Save file is empty or corrupted, returning empty unit list");
        return new List<Unit>();
    }
}
