using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/mapSave.json";

    public static void SaveMap(MapSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Map saved to: " + savePath);
    }

    public static MapSaveData LoadMap()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            MapSaveData data = JsonUtility.FromJson<MapSaveData>(json);
            Debug.Log("Map loaded from: " + savePath);
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return null;
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }
}
