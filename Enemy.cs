using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField] GameObject BulletSpawner;
    [SerializeField] Transform player;
    [SerializeField] float distance = 0f;
    [SerializeField] Weapon weapon;
    [SerializeField] float speed = 5f;
    [SerializeField] bool isFrozen = false;
    [SerializeField] float timer = 0f;
    [SerializeField] GameObject bulletSpawner;

    void FixedUpdate()
    {
        if(!isFrozen)
        {
            Movement();
            Distance();
            Shoot();
            Timer();
        }
    }

    public void SetFreeze(bool condition)
    {
        isFrozen = condition;
    }

    public bool GetFreeze()
    {
        return isFrozen;
    }


    public void SetTransform(GameObject temp)
    {
        player = temp.transform;
    }

    void Distance()
    {
        float x = player.transform.position.x;
        float z = player.transform.position.z;
        float x1 = transform.position.x;
        float z2 = transform.position.z;
        distance = Mathf.Sqrt((x-x1)*(x-x1)+(z-z2)*(z-z2));
        if(distance < 10)
        {
            speed = 0;
        }
        else
        {
            speed = 5f;
        }
    }

    void Shoot()
    {
        if(weapon != null)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 35))
            {
                Debug.Log("hit");
                if(hit.collider.gameObject.tag == "Player" && timer == 0)
                {
                    timer = weapon.GetReloadTime();
                    weapon.Shoot(bulletSpawner);
                    Debug.DrawRay(transform.position, transform.forward * 35, Color.red);
                }
            }
            else 
            {
                Debug.DrawRay(transform.position, transform.forward * 35, Color.white);
            }
        }
    }

    void Movement()
    { 
        Vector3 lookPos = (player.position - transform.position).normalized;  
        Vector3 leftCast  = transform.position - (transform.right * 1f);
        Vector3 rightCast = transform.position + (transform.right * 1f);
        bool leftHit = false;
        bool rightHit = false;
        RaycastHit hit;
        float distance = 2.5f;
        // left side
        if(Physics.Raycast(leftCast, transform.forward, out hit, distance))
        {
            lookPos += hit.normal * 30;
            Debug.DrawRay(leftCast, transform.forward * distance, Color.yellow);
            leftHit = true;
        }
        else
        {
            Debug.DrawRay(leftCast, transform.forward * distance, Color.white);
        }
        // right side
        if(Physics.Raycast(rightCast, transform.forward, out hit, distance))
        {
            lookPos += hit.normal * 30;
            Debug.DrawRay(rightCast, transform.forward * distance, Color.yellow);  
            rightHit = true; 
        }   
        else
        {
            Debug.DrawRay(rightCast, transform.forward * distance, Color.white);
        }
        if(leftHit && rightHit)
        {
            transform.Rotate(0,90,0);
            leftHit = false;
            rightHit = false;
        }
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = 0;
        }
    }

    public void Death()
    {
        weapon.Throw(false);
        GameManager.instance.AddScore(this);
        GameManager.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
