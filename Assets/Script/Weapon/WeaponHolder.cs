using UnityEngine;
using UnityEngine.UI;
public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] WeaponsPrefabs;
    public GameObject defaultWeaponPrefab;
    private  Image[] _spriteShowCurrentWeapons;
    public GameObject currentWeapon;
    private Gun currentGun;

    private void Awake()
    {
        PlayerMovement.instance.WeaponHolder = this;
        StartGun();   
    }

    public void StartGun()
    {
        currentWeapon = Instantiate(defaultWeaponPrefab, transform.position, Quaternion.identity, transform);
        currentGun = currentWeapon.GetComponent<Gun>();
        currentWeapon.transform.localPosition = Vector3.zero;
        UpdateWeaponUI();
    }

    public void EquipWeapon(GameObject weaponPickUp)
    {

        if(currentWeapon != null){
            DropWeapon();
        }
        currentWeapon = weaponPickUp;
        currentWeapon.transform.SetParent(transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentGun = currentWeapon.GetComponent<Gun>();
        UpdateWeaponUI();
    }

    public Gun GetCurrentWeapon()
    {
        if(currentGun != null) return currentGun;
        return null;
    }

    public void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    private void UpdateWeaponUI()
    {
        GameObject[] weaponUIObjects = GameObject.FindGameObjectsWithTag("CurrentWeapon");
        _spriteShowCurrentWeapons = new Image[weaponUIObjects.Length];
        for(int i = 0; i < weaponUIObjects.Length; i++){
            _spriteShowCurrentWeapons[i] = weaponUIObjects[i].GetComponent<Image>();
            _spriteShowCurrentWeapons[i].sprite = currentGun.GetComponentInChildren<SpriteRenderer>().sprite;
            _spriteShowCurrentWeapons[i].preserveAspect = true;
        }
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
    public void Save(ref SaveWeapon data)
    {
        Gun gun = currentWeapon.GetComponent<Gun>();
        data.weaponSaveName = gun._weaponName;
        Debug.Log("aaa "+data.weaponSaveName);
    }

    public void Load(SaveWeapon data)
    {
        if(currentWeapon != null) Destroy(currentWeapon.gameObject);
        string weaponNameLoad = data.weaponSaveName;
        Debug.Log("aaa "+weaponNameLoad);
        GameObject prefab = FindWeaponPrefabsByName(weaponNameLoad);

        currentWeapon = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        currentGun = currentWeapon.GetComponent<Gun>();
        currentWeapon.transform.localPosition = Vector3.zero;
        UpdateWeaponUI();
    }

    private GameObject FindWeaponPrefabsByName(string weaponName)
    {
        foreach(GameObject weapon in WeaponsPrefabs){
            Gun gun = weapon.GetComponent<Gun>();
            Debug.Log(gun._weaponName + " | " + weaponName);
            if(gun._weaponName == weaponName){
                return weapon;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct SaveWeapon
{
    public string weaponSaveName;
}
