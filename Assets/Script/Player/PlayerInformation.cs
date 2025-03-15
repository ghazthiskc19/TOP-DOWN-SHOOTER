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

    void Update()
    {
        currentKillText.text = "Current kills: " + currentKill;
        currentCureText.text = "Current cured: " + currentCure;
    }
}
