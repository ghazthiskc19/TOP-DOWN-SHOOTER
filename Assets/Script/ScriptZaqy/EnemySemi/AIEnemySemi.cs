using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class AIEnemySemi : EnemySemi
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private SanityController playerPhobia;
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField] private GameObject _weaponPrefabs;
    public EnemyAnimControl enemyAnimation;
    public RayCastSemi enemyRayCast;
    public float timer;
    public float timerSearchPlayer;
    public float timerLostPlayer;
    public bool trackPlayer;
    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    // HealthController health;
    int currentWaypoint;
    bool _Return;
    bool idle;
    public Vector2 direction = new Vector2 (1f, 1f);
    

    void Start()
    {
        timerLostPlayer = timerSearchPlayer;
        timer = _timeBetweenAttack;
        nextTarget = target.Length - 1;
        // health = GetComponent<HealthController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 1f);
        seeker.StartPath(rb.position, target[nextTarget].position,  OnPathComplete);
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(notPatrol){
            idle = true;
            anim.SetBool("idle", true);
        }
        if(trackPlayer){
            enemyRayCast.rayForChase(target);
        }
        else{
            enemyRayCast.rayForPatrol();
            enemyRayCast.rayControl(direction);
        }
        if(path == null){
            return;
        }
        if(nextTarget == 0){
            float distancePlayer = Vector2.Distance(target[0].position, transform.position);
            if(distancePlayer < attackRange && enemyRayCast.ray.collider.gameObject.CompareTag("Player")){
                idle = true;
                anim.SetBool("idle", true);
                anim.SetBool("attacking", true);
                anim.SetBool("meeleEnemy", meeleEnemy);
                enemyAnimation.animControl(target[0].position - transform.position);
                if(enemyRayCast.ray.collider.gameObject.CompareTag("Player") && !meeleEnemy){
                    FireBullet();
                }
                else if(enemyRayCast.ray.collider.gameObject.CompareTag("Player") && meeleEnemy){
                    CoroutineMeeleAttack(_timeBetweenAttack);
                }
            }
            else{
                idle = false;
                anim.SetBool("idle", false);
                anim.SetBool("attacking", false);
            }

            if(timerLostPlayer <= 0){
                lostPlayer();
                StartCoroutine(goIdle(3f));
            }
        }
        
        if(currentWaypoint >= path.vectorPath.Count){
            StartCoroutine(goIdle(idleDuration));
            targetBerikutnya();
            UpdatePath();
        }

        if(!idle || (trackPlayer && !enemyRayCast.ray.collider.gameObject.CompareTag("Player"))){
            goWalk();
        }
        
        if (path.vectorPath != null && currentWaypoint >= 0 && currentWaypoint < path.vectorPath.Count){
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance){
                currentWaypoint++;
            }
        }

        
    }
    public void chanceBulletDamage(){
        setBulletDamage(5.0f);    
    }
    void goWalk(){
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * 4000 * Time.deltaTime;
        enemyAnimation.animControl(direction);
        rb.AddForce(force);
    }
    void UpdatePath(){
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, target[nextTarget].position,  OnPathComplete);
        }
    }
    
    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
    void targetBerikutnya(){
        if(nextTarget == 0){
            return;
        }
        if (nextTarget >= target.Length - 1 || nextTarget <= 1)
        {
            _Return = !_Return; // Jika mencapai akhir, balik arah
        }

        nextTarget = Mathf.Clamp(nextTarget, 0, target.Length - 1);
        if (_Return)
        {
            nextTarget--; // Mundur ke titik berikutnya
        }
        else
        {
            nextTarget++; // Maju ke titik sebelumnya
        }
        nextTarget = Mathf.Clamp(nextTarget, 0, target.Length - 1);
    }

    public IEnumerator goIdle(float duration){
        idle = true;
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(duration);
        anim.SetBool("idle", false);
        idle = false;
    }
    public void foundPlayer(){
        timerLostPlayer = timerSearchPlayer + 1;
        trackPlayer = true;
        anim.SetBool("idle", false);
        idle = false;
        nextTarget = 0;
        UpdatePath();
    }
    public void lostPlayer(){
        trackPlayer = false;
        anim.SetBool("idle", false);
        idle = false;
        nextTarget = 2;
        UpdatePath();
    }

    void FireBullet(){
        timer -= Time.deltaTime;
        if( timer < 0){
            timer = _timeBetweenAttack;
        }
        else{
            return;
        }
        GameObject enemyBullet = Instantiate(_bulletPrefabs, transform.position, Quaternion.LookRotation(Vector3.forward, target[0].position - transform.position));
        var bullet = enemyBullet.GetComponent<EnemyBulletSemi>();
        bullet.enemy = GetComponent<AIEnemySemi>();
        Rigidbody2D _rbBullet = enemyBullet.GetComponent<Rigidbody2D>();
        _rbBullet.linearVelocity = (target[0].position - transform.position).normalized * _bulletSpeed;
        SoundAttack();
    }

    void SoundAttack(){
        if(playerPhobia.phobiaSuara){
            Vector2 meeleDirection = (target[0].position - transform.position).normalized;
            float angle = Mathf.Atan2(meeleDirection.y, meeleDirection.x) * Mathf.Rad2Deg;

            Vector2 attackCenter = (Vector2)transform.position + meeleDirection * MeeleRange;
            Collider2D[] players = Physics2D.OverlapBoxAll(attackCenter, MeeleSize, angle, _playerLayer);

            foreach (Collider2D player in players){
                if (player.gameObject.CompareTag("Player")){
                    havedoneattack = true;
                    Debug.Log("Player terkena serangan suara!");
                    HealthController healthController = player.GetComponent<HealthController>();
                    if (healthController != null){
                        healthController.IsInvicible = false;
                        playerPhobia.lostSanity(5);
                        healthController.TakeDamage(5);
                    }
                }
            }
        }
    }

    public void goDie(){
        PlayerInformation.instance.currentKill++;
        anim.SetTrigger("IsDead");
        GetComponent<AIEnemySemi>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
    public void dropWeapon(){
        GameObject weapon = Instantiate(_weaponPrefabs, transform.position, transform.rotation);
    }

    private void CoroutineMeeleAttack(float AttackDuration)
    {
        timer -= Time.deltaTime;
        if( timer < 0){
            timer = _timeBetweenAttack;
        }
        else{
            return;
        }
        Vector2 meeleDirection = (target[0].position - transform.position).normalized;
        float angle = Mathf.Atan2(meeleDirection.y, meeleDirection.x) * Mathf.Rad2Deg;

        Vector2 attackCenter = (Vector2)transform.position + meeleDirection * MeeleRange;
        Collider2D[] players = Physics2D.OverlapBoxAll(attackCenter, MeeleSize, angle, _playerLayer);

        foreach (Collider2D player in players)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                havedoneattack = true;
                Debug.Log("Player terkena serangan melee!");
                HealthController healthController = player.GetComponent<HealthController>();
                if (healthController != null)
                {
                    healthController.TakeDamage(getBulletDamage());
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Vector2 attackDirection = (target[0].position - transform.position).normalized;
        Vector2 attackCenter = (Vector2)transform.position + attackDirection * MeeleRange;

        float attackAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(attackCenter, Quaternion.Euler(0, 0, attackAngle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, MeeleSize);
    }

}
