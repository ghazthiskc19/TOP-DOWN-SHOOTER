using TMPro;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    public static PlayerInformation instance;
    public int currentKill;
    public int currentCure;
    public TMP_Text currentKillText;
    public TMP_Text currentCureText;

    void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void UpdateUI()
    {
        currentKillText.text = "Current kills: " + currentKill;
        currentCureText.text = "Current cured: " + currentCure;
    }

    public void AddKill()
    {
        currentKill++;
        UpdateUI();
    }

    public void AddCure()
    {
        currentCure++;
        UpdateUI();
    }
    public void Save(ref PlayerStats data)
    {
        data.currentKill = currentKill;
        data.currentCure = currentCure;
    }

    public void Load(PlayerStats data)
    {
        currentKill = data.currentKill;
        currentCure = data.currentCure;
        UpdateUI();
    }
}


[System.Serializable]
public struct PlayerStats
{
    public int currentKill;
    public int currentCure;
}
