using UnityEngine;
public class EnemyAnimControl : MonoBehaviour
{
    public Animator anim;
    public void animControl(Vector2 direction){
        direction = direction.normalized;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        if(direction.x >= 0.01f){
            transform.localScale = new Vector3( 1, transform.localScale.y, transform.localScale.z);
        }
        else if(direction.x < 0.01f){
            transform.localScale = new Vector3( -1, transform.localScale.y, transform.localScale.z);
        }

        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
    }
}
