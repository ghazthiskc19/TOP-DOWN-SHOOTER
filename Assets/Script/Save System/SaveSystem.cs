using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();
    [System.Serializable]
    public struct SaveData {
        public PlayerSaveData PlayerData;
        public SaveWeapon PlayerWeapon;
        public SpawnSave SpawnSave;
        public PlayerStats playerStats;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" +".save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    public static void HandleSaveData()
    {
        PlayerMovement.instance.Save(ref _saveData.PlayerData);
        PlayerMovement.instance.WeaponHolder.Save(ref _saveData.PlayerWeapon);
        PlayerMovement.instance.SpawnManager.Save(ref _saveData.SpawnSave);
        PlayerInformation.instance.Save(ref _saveData.playerStats);
        
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    public static void HandleLoadData()
    {
        PlayerMovement.instance.Load(_saveData.PlayerData);
        PlayerMovement.instance.WeaponHolder.Load(_saveData.PlayerWeapon);
        PlayerMovement.instance.SpawnManager.Load(_saveData.SpawnSave);
        PlayerInformation.instance.Load(_saveData.playerStats);
    }

}
