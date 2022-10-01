using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);

            SaveObject saveObject = new SaveObject() { levelUnlock = 0, coinsEarned = 1000 };
            string json = JsonUtility.ToJson(saveObject);
            SaveSystem.Save(json);

            SaveObjectPlayer saveObjectPlayer = new SaveObjectPlayer() { playerSelectIndex = 0, healthUpgrade = 0, attackUpgrade = 0, speedUpgrade = 0, coinsUpgrade = 0 };
            string jsonPlayer = JsonUtility.ToJson(saveObjectPlayer);
            SaveSystem.SavePlayer(jsonPlayer);

            List<int> weaponNewEquipedList = new List<int>();
            weaponNewEquipedList.Add(0);
            weaponNewEquipedList.Add(0);
            weaponNewEquipedList.Add(0);
            List<int> weaponOwnedList = new List<int>();
            weaponOwnedList.Add(0);

            SaveObjectWeapons saveObjectWeapons = new SaveObjectWeapons() { weaponOwnedList = weaponOwnedList, weaponEquipList = weaponNewEquipedList, unlockSlots = 1 };
            string jsonWeapon = JsonUtility.ToJson(saveObjectWeapons);
            SaveSystem.SaveWeapons(jsonWeapon);
        }
    }

    // Buat save level progression dan coins
    public static void Save(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + "save.txt", saveString);
    }

    public static string Load()
    {
        if(File.Exists(SAVE_FOLDER + "save.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "save.txt");
            return saveString;
        }
        else
        {
            return null;
        }
    }

    // Buat save weapons
    public static void SaveWeapons(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + "saveWeapons.txt", saveString);
    }

    public static string LoadWeapons()
    {
        if (File.Exists(SAVE_FOLDER + "saveWeapons.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "saveWeapons.txt");
            return saveString;
        }
        else
        {
            return null;
        }
    }

    // Buat save player dan stats
    public static void SavePlayer(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + "savePlayer.txt", saveString);
    }

    public static string LoadPlayer()
    {
        if (File.Exists(SAVE_FOLDER + "savePlayer.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "savePlayer.txt");
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
