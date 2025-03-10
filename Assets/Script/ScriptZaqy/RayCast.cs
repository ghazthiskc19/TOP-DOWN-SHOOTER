using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCast : MonoBehaviour
{
    public GameObject obstacleRayObject;
    public float rayDistance;
    public RaycastHit2D ray;
    Vector2 rayCastDirection;
    public AIEnemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void rayControl(Vector2 direction){
        if(direction.x > 0.5f){
            rayCastDirection = new Vector2(1, 0);
        }
        else if(direction.x < -0.5f){
            rayCastDirection = new Vector2(-1, 0);
        }
        else if(direction.y > 0.5f){
            rayCastDirection = new Vector2(0, 1);
        }
        else if(direction.y < -0.5f){
            rayCastDirection = new Vector2(0, -1);
        }
    }
    public void rayForPatrol(){
        ray = Physics2D.Raycast(obstacleRayObject.transform.position, rayCastDirection, rayDistance);

        if(ray.collider != null){
            if (ray.collider.gameObject.CompareTag("Player")){
                enemy.foundPlayer();
            }
            Debug.DrawRay(obstacleRayObject.transform.position, ray.distance * rayCastDirection, Color.red);
        }
        else{
            Debug.DrawRay(obstacleRayObject.transform.position, ray.distance * rayCastDirection, Color.green);
        }
    }
    public void rayForChase(Transform[] target){
        ray = Physics2D.Raycast(transform.position, target[0].position - transform.position);
        if(ray.collider != null){
            if(ray.collider.gameObject.CompareTag("Player")){
                Debug.DrawRay(transform.position, target[0].position - transform.position, Color.red);
            }
            else{
                Debug.DrawRay(transform.position, target[0].position - transform.position, Color.green);
            }
        }
    }
}
