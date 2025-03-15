using UnityEngine;
using UnityEngine.UI;
public class WeaponHolder : MonoBehaviour
{
    public GameObject defaultWeaponPrefab;
    public  Image[] _spriteShowCurrentWeapons;
    public GameObject currentWeapon;
    private Gun currentGun;

    private void Awake()
    {   
        if(currentWeapon != null){
            GameObject[] weaponUIObjects = GameObject.FindGameObjectsWithTag("CurrentWeapon");
            
            currentWeapon = Instantiate(defaultWeaponPrefab, transform.position, Quaternion.identity, transform);
            currentGun = currentWeapon.GetComponent<Gun>();
            _spriteShowCurrentWeapons = new Image[weaponUIObjects.Length];
            for(int i = 0; i < weaponUIObjects.Length; i++){
                _spriteShowCurrentWeapons[i] = weaponUIObjects[i].GetComponent<Image>();
                _spriteShowCurrentWeapons[i].sprite = currentGun.GetComponentInChildren<SpriteRenderer>().sprite;
                _spriteShowCurrentWeapons[i].preserveAspect = true;
            }
            currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    public void EquipWeapon(GameObject weaponPickUp)
    {
        GameObject[] weaponUIObjects = GameObject.FindGameObjectsWithTag("CurrentWeapon");

        if(currentWeapon != null){
            DropWeapon();
        }
        currentWeapon = weaponPickUp;
        currentWeapon.transform.SetParent(transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentGun = currentWeapon.GetComponent<Gun>();
        _spriteShowCurrentWeapons = new Image[weaponUIObjects.Length];

        for(int i = 0; i < weaponUIObjects.Length; i++){
            _spriteShowCurrentWeapons[i] = weaponUIObjects[i].GetComponent<Image>();
            _spriteShowCurrentWeapons[i].sprite = currentGun.GetComponentInChildren<SpriteRenderer>().sprite;
            _spriteShowCurrentWeapons[i].preserveAspect = true;
        }
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public Gun GetCurrentWeapon()
    {
        if(currentGun != null) return currentGun;
        Debug.Log("Ada gak???");
        return null;
    }

    public void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
