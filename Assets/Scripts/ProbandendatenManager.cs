using System.IO;
using UnityEngine;

public static class ProbandendatenManager
{
    private static string FilePath => Path.Combine(Application.persistentDataPath, $"Proband_{Probandendaten.Field0}_Trial_{Probandendaten.Field1}.json");

    // Speichert die Daten als JSON
    public static void Save()
    {
        ProbandendatenData data = new ProbandendatenData();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
        Debug.Log($"Probandendaten gespeichert: {FilePath}");
    }
}
