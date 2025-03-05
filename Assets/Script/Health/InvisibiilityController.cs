using System.Collections;
using UnityEngine;

public class InvisibiilityController : MonoBehaviour
{
    private HealthController _healthController;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
    }

    public void StartInvicibility(float InvicibilityDuration){
        StartCoroutine(InvicibilityCoroutine(InvicibilityDuration));
    }

    private IEnumerator InvicibilityCoroutine(float InvicibilityDuration){
        _healthController.IsInvicible =  true;
        yield return new WaitForSeconds(InvicibilityDuration);
        _healthController.IsInvicible = false;
    }
}
