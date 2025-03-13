using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] target;
    public int nextTarget;
    public float speed = 2f;
    public float bulletDamage;
    public float nextWaypointDistance = 3f;
    public float idleDuration = 3f;
    public float _bulletSpeed;
    public float _timeBetweenAttack;
    public bool meeleEnemy;
    public int attackRange;
    public bool havedoneattack;
}
