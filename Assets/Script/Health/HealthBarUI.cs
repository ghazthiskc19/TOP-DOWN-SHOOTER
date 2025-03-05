using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBarForeground;
    [SerializeField] private AnimationCurve curve;
    private float _currentHealth;
    public void UpadteHealthBar(HealthController healthController){
        StartCoroutine(UpdateHealthBarSmooth(healthController));
    }
    private IEnumerator UpdateHealthBarSmooth(HealthController healthController){

        _currentHealth = _healthBarForeground.fillAmount;
        float t = 0;
        int timeAnimation = 2;
        while(t < 1){
            t += Time.deltaTime / timeAnimation;
            _healthBarForeground.fillAmount = Mathf.Lerp(_currentHealth ,healthController.RemainingHealth, curve.Evaluate(t));
            yield return null;
        }
    }
}
