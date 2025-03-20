using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Image backgroundImage; // Referensi ke komponen Image yang digunakan sebagai background
    public float transitionDuration = 5f;
    public float delayBeforeReverse = 3f; // Durasi perubahan warna dalam detik

    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    bool blackBG = false;

    void Start()
    {
        // Pastikan backgroundImage sudah diassign di Inspector
        if (backgroundImage == null)
        {
            Debug.LogError("Background Image belum diassign!");
            return;
        }

        // Set warna awal
        backgroundImage.color = startColor;
    }
    public void changeLightColor(){
        StartCoroutine(ChangeColor());
    }

    // Coroutine untuk mengubah warna secara bertahap
    private System.Collections.IEnumerator ChangeColor()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            // Hitung progress perubahan warna (0 sampai 1)
            float t = elapsedTime / transitionDuration;

            if(!blackBG){
                backgroundImage.color = Color.Lerp(startColor, endColor, t);
            }
            else{
                backgroundImage.color = Color.Lerp(endColor, startColor, t);
            }
            

            // Tambahkan waktu yang telah berlalu
            elapsedTime += Time.deltaTime;

            yield return null; // Tunggu frame berikutnya
        }
        blackBG = !blackBG;
    }
}
