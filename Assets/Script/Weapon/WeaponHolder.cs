using UnityEngine;
using UnityEngine.UI;
public class WeaponHolder : MonoBehaviour
{
    public GameObject defaultWeaponPrefab;
    public Image _spriteShowCurrentWeapon;
    public GameObject currentWeapon; // Default senjata
    private Gun currentGun;

    private void Start()
    {
        if(currentWeapon != null){
            currentWeapon = Instantiate(defaultWeaponPrefab, transform.position, Quaternion.identity, transform);
            currentGun = currentWeapon.GetComponent<Gun>();
            currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
            _spriteShowCurrentWeapon = GameObject.Find("showCurrentWeapon").GetComponent<Image>();
            _spriteShowCurrentWeapon.sprite = currentGun.GetComponentInChildren<SpriteRenderer>().sprite;
            _spriteShowCurrentWeapon.preserveAspect = true;
        }
    }

    public void EquipWeapon(GameObject weaponPickUp)
    {
        _spriteShowCurrentWeapon = GameObject.Find("showCurrentWeapon").GetComponent<Image>();

        if(currentWeapon != null){
            DropWeapon();
        }
        currentWeapon = weaponPickUp;
        currentWeapon.transform.SetParent(transform);
        currentWeapon.transform.localPosition = Vector3.zero;

        currentGun = currentWeapon.GetComponent<Gun>();
        _spriteShowCurrentWeapon.sprite = currentGun.GetComponentInChildren<SpriteRenderer>().sprite;
        _spriteShowCurrentWeapon.preserveAspect = true;
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
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
}
