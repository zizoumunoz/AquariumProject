using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveAndLoad
{
    public static SaveFile save { get; private set; } = null;
    public class SaveFile
    {
        public List<Tank> tankList = new();
        public DateTime time;
        public int money;

    }
    public static int slot = 0;
    private static string path = Application.dataPath;
    public static void SaveData(List<Tank> tanks, int slot)
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");

        SaveFile save = new()
        {
            tankList = tanks,
            time = DateTime.Now,
            money = MoneyManager.Instance.money
        };
        string dataToSave = JsonUtility.ToJson(save);

        using (FileStream stream = new(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new(stream))
            {
                writer.Write(dataToSave);
            }
        }
    }
    public static void LoadData(int slot)
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");

        if (!File.Exists(fullPath))
        {
            save = null;
            return;
        }

        using (FileStream stream = new(fullPath, FileMode.Open))
        {
            using (StreamReader reader = new(stream))
            {
                save = JsonUtility.FromJson<SaveFile>(reader.ReadToEnd());
            }
        }
    }
    public static bool ExistSlot(int slot)
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");

        return File.Exists(fullPath);
    }
    public static void NewData()
    {
        save = null;
    }
    public static void DeleteData(int slot)
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}