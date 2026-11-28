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
    private static string basePath = Settings.Instance.folder;
    private static string path
    {
        get
        {
            string folder = Settings.Instance.folder;

            // Make sure the directory exists
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }
    }
    public static void Init()
    {
        string settingsPath = Path.Combine(basePath, $"Settings.txt");
        if (!File.Exists(settingsPath))
        {
            string settings = JsonUtility.ToJson(Settings.Instance);
            File.WriteAllText(settingsPath, settings);
        }
        else
        {
            Settings settings = JsonUtility.FromJson<Settings>(File.ReadAllText(settingsPath));
            Settings.Instance.animationDuration = settings.animationDuration;
            Settings.Instance.folder = settings.folder;
        }
    }
    public static void UpdateSettings()
    {
        string settingsPath = Path.Combine(basePath, $"Settings.txt");
        string settings = JsonUtility.ToJson(Settings.Instance);
        File.WriteAllText(settingsPath, settings);
    }
    public static void SaveData()
    {
        string fullPath = Path.Combine(path, $"Save{slot}.txt");
        Debug.Log(fullPath);

        SaveFile save = new()
        {
            tankList = TankManager.instance.database.tanks,
            time = DateTime.Now,
            money = MoneyManager.Instance.money
        };
        string dataToSave = JsonUtility.ToJson(save);

        File.WriteAllText(fullPath, dataToSave);

        string settingsPath = Path.Combine(basePath, $"Settings.txt");
        string settings = JsonUtility.ToJson(Settings.Instance);
        File.WriteAllText(settingsPath, settings);
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
    public static bool TryToSave(string newPath)
    {
        string fullPath = Path.Combine(newPath, $"Save{slot}.txt");
        Debug.Log(fullPath);

        SaveFile save = new()
        {
            tankList = new(),
            time = DateTime.Now,
            money = 0,
        };
        string dataToSave = JsonUtility.ToJson(save);

        try
        {
            File.WriteAllText(fullPath, dataToSave);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
    public static void Replace(string newPath)
    {
        for (int i = 0; i < 3; i++)
        {
            string fullOldPath = Path.Combine(path, $"Save{slot}.txt");
            string fullNewPath = Path.Combine(newPath, $"Save{slot}.txt");

            if (!File.Exists(fullOldPath))
            {
                continue;
            }

            File.WriteAllText(fullNewPath, File.ReadAllText(fullOldPath));
            File.Delete(fullOldPath);
        }
        Settings.Instance.folder = newPath;
    }
}