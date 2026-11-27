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
    private static string path
    {
        get
        {
            string folder;

#if UNITY_WEBGL && !UNITY_EDITOR
            folder = "/idbfs/my_tank_game_saves";
#else
            folder = Application.persistentDataPath;
#endif

            // Make sure the directory exists
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }
    }
    public static void SaveData(List<Tank> tanks, int slot)
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");
        Debug.Log(fullPath);

        SaveFile save = new()
        {
            tankList = tanks,
            time = DateTime.Now,
            money = MoneyManager.Instance.money
        };
        string dataToSave = JsonUtility.ToJson(save);

        File.WriteAllText(fullPath, dataToSave);
    }
    public static void LoadData(int slot)
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");

        if (!File.Exists(fullPath))
        {
            save = null;
            return;
        }
        try
        {
            save = JsonUtility.FromJson<SaveFile>(File.ReadAllText(fullPath));
        }
        catch (Exception)
        {
            File.Delete(fullPath);
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