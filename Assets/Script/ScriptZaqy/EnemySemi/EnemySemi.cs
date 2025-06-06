using UnityEngine;

public class EnemySemi : MonoBehaviour
{
    public Transform[] target;
    public int nextTarget;
    public float speed = 2f;
    public static float bulletDamage = 8f;
    public float nextWaypointDistance = 3f;
    public float idleDuration = 3f;
    public float _bulletSpeed;
    public float _timeBetweenAttack;
    public bool meeleEnemy;
    public int attackRange;
    public bool havedoneattack;
    public bool notPatrol;
    public Vector2 MeeleSize = new Vector2(1.5f, 1.0f);
    public float MeeleRange = 1.0f;

    public void setBulletDamage(float damage)
    {
        bulletDamage += damage;
    }

    public float getBulletDamage()
    {
        return bulletDamage;
    }
}
