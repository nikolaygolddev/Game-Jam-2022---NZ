using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetSpeed(float temp)
    {
        speed = temp;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground"))
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Enemy>().Death();
        }
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.GameOver();
            Destroy(gameObject);
        }
    }
}
