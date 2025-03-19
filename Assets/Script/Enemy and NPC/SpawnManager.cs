using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public GameObject[] enemyObjects;
    public GameObject[] NpcObjects;
    public void GetObjectsList()
    {
        enemyObjects = GetGameObjectsByComponent<EnemyAnimControl>();
        NpcObjects = GetGameObjectsByComponent<DetectQTE>();
    }

    void Start()
    {
        if(instance == null) instance = this;
        PlayerMovement.instance.SpawnManager = this;
        GetObjectsList();
    }


    public static GameObject[] GetGameObjectsByComponent<T>() where T : Component
    {
        return FindObjectsByType<T>(FindObjectsSortMode.None).Select(x => x.gameObject).ToArray();
    }
    public void Save(ref SpawnSave data)
    {   
        GetObjectsList();
        var enemySaves = enemyObjects.Select(enemy =>
        {
            var id = enemy.GetComponent<UniqueID>().ID;
            var enemyHealth = enemy.GetComponent<HealthController>();
            return new SaveEnemy
            {
                ID = id,
                position = enemy.transform.position,
                amountHealth = enemyHealth.GetCurrentHealth(),
            };
        }).ToArray();

        var npcSaves = NpcObjects
        .Select(npc =>
        {
            var detectQTE = npc.GetComponent<DetectQTE>();
            var id = npc.GetComponent<UniqueID>().ID;
            return new SaveNPC
            {
                ID = id,
                position = npc.transform.position,
                QteStatus = detectQTE.QTEStatus,
                IsWinOrNot = detectQTE.LastWinResult
            };
        }).ToArray();

        data = new SpawnSave
        {
            EnemyData = new SceneEnemyData { Enemies = enemySaves },
            NPCData = new SceneNPCData { Npcs = npcSaves }
        };
    }

    public void Load(SpawnSave data)
    {
        GetObjectsList();
        // untuk Enemy
        foreach (var enemy in enemyObjects)
        {
            var uniqueID = enemy.GetComponent<UniqueID>().ID;
            var enemyHealth = enemy.GetComponent<HealthController>();

            var saveData = data.EnemyData.Enemies.FirstOrDefault(x => x.ID == uniqueID);

            if(!string.IsNullOrEmpty(saveData.ID))
            {
                enemy.transform.position = saveData.position;
                enemyHealth.SetCurrentHealth(saveData.amountHealth);
            }else
            {
                Destroy(enemy);
            }
        }

        // Untuk NPC
        foreach (var npc in NpcObjects){
            var uniqueID = npc.GetComponent<UniqueID>().ID;
            var detectQTe = npc.GetComponent<DetectQTE>();

            var saveData = data.NPCData.Npcs.FirstOrDefault(x => x.ID == uniqueID);
            if(saveData.ID != null)
            {
                npc.transform.position = saveData.position;
                detectQTe.QTEStatus = saveData.QteStatus;
                detectQTe.LastWinResult = saveData.IsWinOrNot;
                detectQTe.ChangeAnimationNPC(detectQTe.QTEStatus, saveData.IsWinOrNot);
            }
        }
    }
}


[System.Serializable]
public struct SpawnSave
{
    public SceneEnemyData EnemyData;
    public SceneNPCData NPCData;
}
[System.Serializable]
public struct SceneEnemyData
{
    public SaveEnemy[] Enemies;

}
[System.Serializable]
public struct SaveEnemy
{
    public string ID;
    public Vector3 position;
    public float amountHealth;
}
[System.Serializable]
public struct SceneNPCData
{
    public SaveNPC[] Npcs;
}
[System.Serializable]
public struct SaveNPC
{
    public string ID;
    public Vector3 position;
    public bool QteStatus;
    public bool IsWinOrNot;
}

