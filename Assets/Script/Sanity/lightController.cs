using UnityEngine;
using UnityEngine.UI;

public class lightController : MonoBehaviour
{
    public Image backgroundImage; // Referensi ke komponen Image yang digunakan sebagai background
    public float transitionDuration = 5f; // Durasi transisi warna dalam detik
    public float delayBeforeReverse = 3f; // Jeda sebelum kembali ke warna awal (dalam detik)

    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private SanityController sanity;
    [SerializeField] private SpriteRenderer playerRenderer;

    public void changeBacground(){
        // Mulai perubahan warna
            StartCoroutine(ChangeColorAndReverse());
    }

    // Coroutine untuk mengubah warna dan memutar balik dengan jeda
    private System.Collections.IEnumerator ChangeColorAndReverse()
    {
        playerRenderer.sortingLayerName = "UI";
        // Ubah warna dari startColor ke endColor
         yield return StartCoroutine(ChangeColor(startColor, endColor, transitionDuration));

        
        // Tunggu selama delayBeforeReverse detik
        yield return new WaitForSeconds(delayBeforeReverse);
        //Player gain phobia
        sanity.goInsanePlayer();
         

        // Ubah warna dari endColor kembali ke startColor
        yield return StartCoroutine(ChangeColor(endColor, startColor, transitionDuration));
        sanity.phobiaInvincibility = false;
        playerRenderer.sortingLayerName = "Player";
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