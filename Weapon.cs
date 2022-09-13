using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int numberOfbullets = 5;
    [SerializeField] public GameObject prefabBullet;
    [SerializeField] float reloadTimer = 1f;
    [SerializeField] bool isThrown = false;

    public virtual void Shoot(GameObject spawn)
    {
        Vector3 position = spawn.transform.position;
        Instantiate(prefabBullet, position, spawn.transform.rotation);
    }

    public void Throw(bool condition)
    {
        this.transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider cb = GetComponent<BoxCollider>();
        cb.enabled = true;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 1000);
        rb.AddTorque(transform.transform.right * 0.5f + transform.transform.up * 0.001f, ForceMode.Impulse);
        isThrown = condition;
    }

    public virtual void UseBullet()
    {
        numberOfbullets -= 1;
    }

    public virtual float GetReloadTime()
    {
        return reloadTimer; 
    }

    public int GetAmmo()
    {
        return numberOfbullets;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(isThrown == true && (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground"))
        {
            Destroy(gameObject);
        }
    }
}
