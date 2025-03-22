using TMPro;
using UnityEngine;
public class WeaponPickup : MonoBehaviour
{
    public bool IsPickup = false;
    private GameObject _interractText;
    private void Awake()
    {
        _interractText = GameObject.FindGameObjectWithTag("InterractText");
    }
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
                    SoundManager.instance.PlaySFX(SoundManager.instance.getWeapon);
                    IsPickup = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerMovement>()){
            Gun weapon = GetComponent<Gun>();
            _interractText.GetComponent<CanvasGroup>().alpha = 1f;
            _interractText.GetComponentInChildren<TMP_Text>().text = "Press <F> to Collect "+weapon._weaponName;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>()) _interractText.GetComponent<CanvasGroup>().alpha = 0f;
    }
}