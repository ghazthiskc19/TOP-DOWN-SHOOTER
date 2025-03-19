using System.Collections;
using UnityEngine;

public class InvisibiilityController : MonoBehaviour
{
    private HealthController _healthController;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void StartInvicibility(float InvicibilityDuration){
        StartCoroutine(InvicibilityCoroutine(InvicibilityDuration));
    }

    // private IEnumerator InvicibilityCoroutine(float InvicibilityDuration){
    //     _healthController.IsInvicible =  true;
    //     yield return new WaitForSeconds(InvicibilityDuration);
    //     _healthController.IsInvicible = false;
    // }
    private IEnumerator InvicibilityCoroutine(float InvicibilityDuration){
        _healthController.IsInvicible =  true;
        float elapsedTime = 0;
        bool isInvisible = true;
        while(elapsedTime < InvicibilityDuration){
            isInvisible = !isInvisible;

            _sr.enabled = isInvisible;
            yield return new WaitForSeconds(0.15f);

            elapsedTime += 0.15f;
        }
        _sr.enabled = true;
        _healthController.IsInvicible = false;
    }
}
