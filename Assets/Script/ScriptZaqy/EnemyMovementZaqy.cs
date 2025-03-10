// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
// using System.Collections.Generic;
// public class EnemyMovementZaqy : MonoBehaviour
// {
//     public Animator anim;
//     private EnemyPatrol _EnemyPatrol;
//     public float speed;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         _EnemyPatrol = GetComponent<EnemyPatrol>();
//     }

//     // Update is called once per frame
//     void Update()
//     {

//         Vector3 direction = (_EnemyPatrol.patrolPoints[_EnemyPatrol.targetPoint].position - transform.position).normalized;
        
//         if(_EnemyPatrol.patrol){
//            _EnemyPatrol.GoPatrol(false, anim);
//         }

//         anim.SetFloat("horizontal", direction.x);
//         anim.SetFloat("vertical", direction.y);
        
//         if (direction.x > 0){
//             transform.localScale = new Vector3( 1, transform.localScale.y, transform.localScale.z);
//         }
//         if (direction.x < 0){
//             transform.localScale = new Vector3( -1, transform.localScale.y, transform.localScale.z);
//         }

//     }
// }
