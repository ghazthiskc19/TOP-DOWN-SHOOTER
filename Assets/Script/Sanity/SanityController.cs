using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SanityController : MonoBehaviour
{
    [SerializeField] private float _currentSanity;
    [SerializeField] private float _MaxSanity;
    [SerializeField] private HealthController _health;
    [SerializeField] private EnemyMeele enemyMeele;
    [SerializeField] private EnemyPistol enemyPistol;
    [SerializeField] private EnemySniper enemySniper;
    [SerializeField] private EnemySemi enemySemi;
    [SerializeField] private EnemySMG enemySMG;

    int _sanityLevel;
    public float RemainingSanity
    {
        get 
        {
            return _currentSanity / _MaxSanity;
        }
    }
    public UnityEvent sanitylevelChanged;
    public UnityEvent OnSanityChanged;
    public UnityEvent Suicide;
    public bool phobiaApi;
    public bool phobiaDarah;
    public bool phobiaSuara;
    public bool phobiaKematian;
    public bool phobiaBendaTajam;
    private List<string> availablePhobias;

    void Start()
    {
        availablePhobias = new List<string> { "Api", "Darah", "Suara", "Kematian", "BendaTajam" };
        enemyPistol.setMeeleEnemy(false);
        enemyMeele.setMeeleSize(new Vector2(2f, 2f));
        enemyMeele.setAttackSpeed(1f);
        enemyMeele.setSpeed(800f);

        enemySniper.setAttackRange(20f);
    }

    public void goInsane(){
        _sanityLevel++;

        if (_sanityLevel == 1)
        {
            int randomIndex = Random.Range(0, 2);
            if (randomIndex == 0)
            {
                getPhobiaApi();
            }
            else
            {
                getPhobiaDarah();
            }
            availablePhobias.RemoveAt(randomIndex);
        }
        else
        {
            if (availablePhobias.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePhobias.Count);
                string selectedPhobia = availablePhobias[randomIndex];
                ApplyPhobia(selectedPhobia);
                availablePhobias.RemoveAt(randomIndex);
            }
        }
    }

    public void lostSanity(int sanityLost){
        if(!_health.IsInvicible){
            int sanityDamages = Random.Range(sanityLost, sanityLost + 5);
            _currentSanity += sanityDamages;
            OnSanityChanged.Invoke();
            if(_currentSanity >= 350){
                Suicide.Invoke();
            }
            if(_currentSanity >= (100 * _sanityLevel) + 100){
                goInsane();
                _currentSanity = (100 * _sanityLevel);
            }
        }
    }
    public void gainSanity(int sanityGain){
        _currentSanity -= sanityGain;
        if(_currentSanity < 0){
            _currentSanity = 0;
        }
    }

    private void ApplyPhobia(string phobiaName)
    {
        switch (phobiaName)
        {
            case "Api":
                getPhobiaApi();
                break;
            case "Darah":
                getPhobiaDarah();
                break;
            case "Suara":
                getPhobiaSuara();
                break;
            case "Kematian":
                getPhobiaKematian();
                break;
            case "BendaTajam":
                getPhobiaBendaTajam();
                break;
            default:
                Debug.LogWarning("Phobia tidak dikenal: " + phobiaName);
                break;
        }
    }
    public void getPhobiaApi(){
        phobiaApi = true;
    }
    public void getPhobiaDarah(){
        phobiaDarah = true;
    }
    public void getPhobiaSuara(){
        enemySniper.setAttackRange(2.2f);
        phobiaSuara = true;
    }
    public void getPhobiaKematian(){
        enemyPistol.setMeeleEnemy(true);
        enemyMeele.setSpeed(950f);
        phobiaKematian = true;
    }
    public void getPhobiaBendaTajam(){
        enemyMeele.setMeeleSize(new Vector2(3f, 3f));
        enemyMeele.setAttackSpeed(0.5f);
        phobiaBendaTajam = true;
    }
}
