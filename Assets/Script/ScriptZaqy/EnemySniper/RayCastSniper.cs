using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCastSniper : MonoBehaviour
{
    public GameObject obstacleRayObject;
    public float rayDistance;
    public float rayAngle;
    public RaycastHit2D ray;
    Vector2 rayCastDirection;
    Vector2 rayCastDirection1;
    Vector2 rayCastDirection2;
    public AIEnemySniper enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void rayControl(Vector2 direction){
        if(direction.x > 0.5f){
            rayCastDirection = new Vector2(1, 0);
            rayCastDirection1 = new Vector2(1, rayAngle);
            rayCastDirection2 = new Vector2(1, -rayAngle);
        }
        else if(direction.x < -0.5f){
            rayCastDirection = new Vector2(-1, 0);
            rayCastDirection1 = new Vector2(-1, rayAngle);
            rayCastDirection2 = new Vector2(-1, -rayAngle);
        }
        else if(direction.y > 0.5f){
            rayCastDirection = new Vector2(0, 1);
            rayCastDirection1 = new Vector2(rayAngle, 1);
            rayCastDirection2 = new Vector2(-rayAngle, 1);
        }
        else if(direction.y < -0.5f){
            rayCastDirection = new Vector2(0, -1);
            rayCastDirection1 = new Vector2(rayAngle, -1);
            rayCastDirection2 = new Vector2(-rayAngle, -1);
        }
    }
    public void rayForPatrol(){
        RaycastHit2D ray1 = Physics2D.Raycast(obstacleRayObject.transform.position, rayCastDirection1, rayDistance);
        ray = Physics2D.Raycast(obstacleRayObject.transform.position, rayCastDirection, rayDistance);
        RaycastHit2D ray2 = Physics2D.Raycast(obstacleRayObject.transform.position, rayCastDirection2, rayDistance);

        Debug.DrawRay(obstacleRayObject.transform.position, ray.distance * rayCastDirection, Color.red);
        Debug.DrawRay(obstacleRayObject.transform.position, ray.distance * rayCastDirection1, Color.red);
        Debug.DrawRay(obstacleRayObject.transform.position, ray.distance * rayCastDirection2, Color.red);
        RaycastHit2D[] rays = {ray, ray1, ray2};
        foreach (RaycastHit2D Ray in rays){
            if(Ray.collider != null){
                if(Ray.collider.gameObject.CompareTag("Player")){
                    enemy.foundPlayer();
                }
            } 
        }
        
    }
    public void rayForChase(Transform[] target){
        ray = Physics2D.Raycast(transform.position, target[0].position - transform.position);
        if(ray.collider != null){
            if(ray.collider.gameObject.CompareTag("Player")){
                Debug.DrawRay(transform.position, target[0].position - transform.position, Color.red);
                enemy.timerLostPlayer = enemy.timerSearchPlayer;
            }
            else{
                enemy.timerLostPlayer -= Time.deltaTime;
                // Debug.Log(target);
                Debug.DrawRay(transform.position, target[0].position - transform.position, Color.green);
            }
        }
    }
}
