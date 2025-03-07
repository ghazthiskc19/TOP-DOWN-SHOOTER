using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject defaultWeaponPrefab;
    public GameObject currentWeapon; // Default senjata
    private Gun currentGun;

    private void Start()
    {
        if(currentWeapon != null){
            currentWeapon = Instantiate(defaultWeaponPrefab, transform.position, Quaternion.identity, transform);
            currentGun = currentWeapon.GetComponent<Gun>();
            currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        // DropWeapon();
    }

    public void EquipWeapon(GameObject weaponPickUp)
    {
        // currentWeapon = Instantiate(weaponPickUp, transform.position, Quaternion.identity, transform);
        // currentWeapon.transform.localPosition = Vector3.zero;

        if(currentWeapon != null){
            DropWeapon();
        }
        currentWeapon = weaponPickUp;
        currentWeapon.transform.SetParent(transform);
        currentWeapon.transform.localPosition = Vector3.zero;

        currentGun = currentWeapon.GetComponent<Gun>();
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public Gun GetCurrentWeapon()
    {
        return currentGun;
    }

    public void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);
        currentWeapon.GetComponentInChildren<SpriteRenderer>().enabled = true;
        currentWeapon = null;
        currentGun = null;
    }
}
