using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class AIEnemy : Enemy
{
    public EnemyAnimControl enemyAnimation;
    public RayCast enemyRayCast;
    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    int currentWaypoint;
    float timer;
    bool _Return;
    bool trackPlayer;
    bool idle;
    Vector2 direction = new Vector2 (1f, 1f);
    // bool reachedEndOfPath = false;
    [SerializeField] private GameObject _bulletPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = _timeBetweenAttack;
        nextTarget = target.Length - 1;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 1f);
        seeker.StartPath(rb.position, target[nextTarget].position,  OnPathComplete);
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
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
            
            if(path.vectorPath.Count < 10){
                idle = true;
                anim.SetBool("idle", true);
                if(enemyRayCast.ray.collider.gameObject.CompareTag("Player")){
                    FireBullet();
                }
            }
            else{
                idle = false;
                anim.SetBool("idle", false);
            }

            if(path.vectorPath.Count > 12){
                lostPlayer();
                StartCoroutine(goIdle(3f));
            }
        }
        
        if(currentWaypoint >= path.vectorPath.Count){
            // reachedEndOfPath = true;
            StartCoroutine(goIdle(idleDuration));
            targetBerikutnya();
            UpdatePath();
        }
        // else{
        //     reachedEndOfPath = false;
        // }

        if(!idle || (trackPlayer && !enemyRayCast.ray.collider.gameObject.CompareTag("Player"))){
            goWalk();
        }
        

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }
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

        // Update nextTarget berdasarkan arah patroli
        if (!_Return)
        {
            nextTarget++; // Maju ke titik berikutnya
        }
        else
        {
            nextTarget--; // Mundur ke titik sebelumnya
        }
        // nextTarget = Mathf.Clamp(nextTarget, 0, target.Length - 1);
    }

    public IEnumerator goIdle(float duration){
        idle = true;
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(duration);
        anim.SetBool("idle", false);
        idle = false;
    }
    public void foundPlayer(){
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
        GameObject enemyBullet = Instantiate(_bulletPrefabs, transform.position, transform.rotation);
        var bullet = enemyBullet.GetComponent<EnemyBullet>();
        bullet.enemy = GetComponent<AIEnemy>();
        Rigidbody2D _rbBullet = enemyBullet.GetComponent<Rigidbody2D>();
        _rbBullet.linearVelocity = _bulletSpeed / 2 * (Vector2)(target[0].position - transform.position);
    }
}
