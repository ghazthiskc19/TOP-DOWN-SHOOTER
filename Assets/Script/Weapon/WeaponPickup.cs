using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public bool IsPickup = false;
    private void OnTriggerStay2D(Collider2D collider)

    {
        PlayerMovement player = collider.gameObject.GetComponent<PlayerMovement>();
        if(player != null && IsPickup)
        {
            player.TryPickupWeapon(this);
            if(IsPickup){
                WeaponHolder _weaponHolder = collider.gameObject.GetComponentInChildren<WeaponHolder>();
                if(_weaponHolder != null)
                {
                    _weaponHolder.EquipWeapon(gameObject);
                    // Destroy(gameObject);
                    IsPickup = false;
                }
            }
        }
    }
}
