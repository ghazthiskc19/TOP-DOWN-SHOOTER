using UnityEngine;

public class EnemyMeele : MonoBehaviour
{
    public Transform[] target;
    public int nextTarget;
    public static float speed = 800f;
    public static float bulletDamage = 10f;
    public float nextWaypointDistance = 3f;
    public float idleDuration = 3f;
    public float _bulletSpeed;
    public static float _timeBetweenAttack = 1;
    public bool meeleEnemy;
    public float attackRange;
    public bool havedoneattack;
    public bool notPatrol;
    public static Vector2 MeeleSize = new Vector2(2f, 2f);
    public static float MeeleRange = 0.1f;

    public void setBulletDamage(float damage)
    {
        bulletDamage += damage;
    }

    public float getBulletDamage()
    {
        return bulletDamage;
    }
    public void setMeeleSize(Vector2 newSize){
        MeeleSize = newSize;
    }
    public void setSpeed(float setSpeed){
        speed = setSpeed;
    }
    public void setAttackSpeed(float newSpeed){
        _timeBetweenAttack = newSpeed;
    }
}
