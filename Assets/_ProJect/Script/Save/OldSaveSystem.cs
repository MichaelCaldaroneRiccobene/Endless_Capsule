using System.IO;
using UnityEngine;

public class OldSaveSystem
{
    public static string GetPath() => Application.persistentDataPath + "/porn.data";

    public static bool Save(SaveData saveData)
    {
        string path = GetPath();

        string jsonString = JsonUtility.ToJson(saveData);

        File.WriteAllText(path, jsonString);

        return true;
    }

    public static bool DoesSaveFilesExists() => File.Exists(GetPath());

    public static SaveData Load()
    {
        string path = GetPath();

        if (!DoesSaveFilesExists()) return null;

        string jsonsString = File.ReadAllText(GetPath());
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonsString);

        return saveData;
    }

    public static void Delete() { if (File.Exists(GetPath())) File.Delete(GetPath()); }
}
