using UnityEngine;

public class PlayerDamagesInvicicbility : MonoBehaviour
{   
    [SerializeField]
    private float _invincibilityDuration;
    private InvisibiilityController _invisiblilityController;
    void Awake()
    {
        _invisiblilityController = GetComponent<InvisibiilityController>();
    }
    public void StartInvicibility(){
        _invisiblilityController.StartInvicibility(_invincibilityDuration);
    }
}
