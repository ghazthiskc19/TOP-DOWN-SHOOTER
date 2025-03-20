using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Image backgroundImage; // Referensi ke komponen Image yang digunakan sebagai background
    public float transitionDuration = 5f;
    public float delayBeforeReverse = 3f; // Durasi perubahan warna dalam detik

    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

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
        StartCoroutine(ChangeColorAndReverse());
    }

    private System.Collections.IEnumerator ChangeColorAndReverse()
    {
        // Ubah warna dari startColor ke endColor
        yield return StartCoroutine(ChangeColor(startColor, endColor, transitionDuration));

        // Tunggu selama delayBeforeReverse detik
        yield return new WaitForSeconds(delayBeforeReverse);

        // Ubah warna dari endColor kembali ke startColor
        yield return StartCoroutine(ChangeColor(endColor, startColor, transitionDuration));
    }

    // Coroutine untuk mengubah warna secara bertahap
    private System.Collections.IEnumerator ChangeColor(Color from, Color to, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            // Hitung progress perubahan warna (0 sampai 1)
            float t = elapsedTime / time;

            // Interpolasi warna dari `from` ke `to`
            backgroundImage.color = Color.Lerp(from, to, t);

            // Tambahkan waktu yang telah berlalu
            elapsedTime += Time.deltaTime;

            yield return null; // Tunggu frame berikutnya
        }

        // Pastikan warna akhir tepat
        backgroundImage.color = to;
    }
}
