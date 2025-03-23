using System.Collections;
using UnityEngine;

public class SanityBarUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _SanityBarForeground;
    [SerializeField] private AnimationCurve curve;
    private float _currentSanity;
    public void UpadteSanityBar(SanityController sanityController){
        StartCoroutine(UpdateSanityBarSmooth(sanityController));
    }
    private IEnumerator UpdateSanityBarSmooth(SanityController sanityController){

        _currentSanity = _SanityBarForeground.fillAmount;
        float t = 0;
        int timeAnimation = 2;
        while(t < 1){
            t += Time.deltaTime / timeAnimation;
            _SanityBarForeground.fillAmount = Mathf.Lerp(_currentSanity ,sanityController.RemainingSanity, curve.Evaluate(t));
            yield return null;
        }
    }
}
