using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public float idleDuration = 2.5f; // Durasi idle
    public AIEnemy enemy;
    public bool _Return;

    public void GoPatrol(bool patrol, Animator anim)
    {
            StartCoroutine(Idle(anim));
    }

    public IEnumerator Idle(Animator anim)
    {
        anim.SetTrigger("idle");
        yield return new WaitForSeconds(idleDuration);
        nextPoint();
        anim.SetTrigger("idle");
    }

    public void nextPoint()
    {
        if (enemy == null || enemy.target == null || enemy.target.Length == 0)
        {
            Debug.LogError("Enemy or target array is missing!");
            return;
        }
        // Tentukan arah patroli (maju/mundur)
        if (enemy.nextTarget >= enemy.target.Length - 1)
        {
            _Return = true; // Jika mencapai akhir, balik arah
        }
        else if (enemy.nextTarget <= 0)
        {
            _Return = false; // Jika kembali ke awal, maju lagi
        }

        // Update nextTarget berdasarkan arah patroli
        if (!_Return)
        {
            enemy.nextTarget++; // Maju ke titik berikutnya
        }
        else
        {
            enemy.nextTarget--; // Mundur ke titik sebelumnya
        }

        // Pastikan nextTarget tetap dalam batas array
        enemy.nextTarget = Mathf.Clamp(enemy.nextTarget, 0, enemy.target.Length - 1);
    }
}