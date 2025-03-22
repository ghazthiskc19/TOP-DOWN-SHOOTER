using UnityEngine;

public class EnemySniper : MonoBehaviour
{
    public Transform[] target;
    public int nextTarget;
    public static float speed = 800f;
    public static float bulletDamage = 15f;
    public float nextWaypointDistance = 3f;
    public float idleDuration = 3f;
    public float _bulletSpeed;
    public float _timeBetweenAttack;
    public bool meeleEnemy;
    public static float attackRange;
    public bool havedoneattack;
    public bool notPatrol;
    public Vector2 MeeleSize = new Vector2(1.5f, 1.0f);
    public float MeeleRange = 1.0f;

    public void setBulletDamage(float damage)
    {
        bulletDamage = damage;
    }

    public float getBulletDamage()
    {
        return bulletDamage;
    }
    public void setAttackRange(float newRange){
        attackRange = newRange;
    }
    public float getAttackRange(){
        return attackRange;
    }
    public void setSpeed(float newSpeed){
        speed = newSpeed;
    }
    public float getSpeed(){
        return speed;
    }
}
