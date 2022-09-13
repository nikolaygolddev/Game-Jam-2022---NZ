using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]CharacterController controller;
    [SerializeField]float speed = 5f;
    [SerializeField]float yRotationLimit = 89f;
    [SerializeField]Camera cam;
    [SerializeField]Weapon weapon;
    [SerializeField]float timer;
    [SerializeField]GameObject weaponHolder;
    [SerializeField]GameObject bulletSpawner;
    [SerializeField]Animator animate;
    
    [Header("Player Settings")]
    [SerializeField]float sensitivity = 5f;

    // locals;
    const string xAxis = "Mouse X";
	const string yAxis = "Mouse Y";
    Vector2 rotation = Vector2.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        Move();
        Shoot();
        Timer();
        Throw();
        if(CheckWeapon())
        {
            Debug.Log("no weapon");
            PickUp();
        }
        YourTime();
    }


    void YourTime()
    {
        if(GameManager.instance.GetActionTime() >= 10f)
        {
            if(Input.GetKeyDown("space"))
            {
                GameManager.instance.StartYourTime();
            }
        }
    }

    void Timer()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = 0;
        }
    }

    void Move()
    {

        //Player movement;
        int X = 0;
        int Z = 0;
        if(Input.GetKey("w"))
        {
            Z = 1;
        }
        if(Input.GetKey("s"))
        {
            Z = -1;
        }
        if(Input.GetKey("a"))
        {
            X = -1;
        }
        if(Input.GetKey("d"))
        {
            X = 1;
        }
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = X;
        moveDirection.z = Z;
        if(moveDirection.x != 0 || moveDirection.z != 0 || (moveDirection.z == 1 && moveDirection.x == 1))
        {
            Debug.Log("Test");
            GameManager.instance.ActionTime();
        }
        // Player movement;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        // Camera movement;
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
		rotation.y += Input.GetAxis(yAxis) * sensitivity;
		rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
		var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        cam.transform.localRotation = yQuat;
		transform.localRotation = xQuat;
    }

    public void SetSensitivity(float number)
    {
        sensitivity = number;
    }

    // shot bullets from the current weapon
    void Shoot()
    {
        if(weapon != null)
        {
            if(Input.GetMouseButtonDown(0) && timer == 0 && CheckAmmo())
            {
                weapon.Shoot(bulletSpawner);
                weapon.UseBullet();
                timer = weapon.GetReloadTime();
            }
        }
    }

    bool CheckAmmo()
    {
        bool condition = true;
        if(weapon != null)
        {
            if(weapon.GetAmmo() <= 0)
            {
                condition = false;
            }
        }
        return condition;
    }

    void PickUp()
    {
        // pickup weapon with raycast and put it in weapon holder and weapon script
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(cam.transform.forward, transform.forward * 15, Color.red);
        if(Physics.Raycast(ray, out hit, 15))
        {
            
            if(hit.collider.gameObject.GetComponent<Weapon>()!= null && hit.collider.gameObject.transform.parent == null)
            {
                Debug.DrawRay(cam.transform.forward, transform.forward * 15, Color.blue);
                if(Input.GetMouseButtonDown(1))
                {
                    Weapon temp = hit.collider.gameObject.GetComponent<Weapon>();
                    weapon = temp;
                    temp.gameObject.transform.position = weaponHolder.transform.position;
                    temp.gameObject.transform.rotation = weaponHolder.transform.rotation;
                    temp.gameObject.transform.SetParent(weaponHolder.transform);
                    temp.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    bool CheckWeapon()
    {
        // check if player has any weapons at the moment
        if(weapon == null)
            return true;
        return false;
    }

    void Throw()
    {
        // throw the weapon resulting it to to be removed
        if(weapon != null)
        {
            if(Input.GetMouseButtonDown(1))
            {
                weapon.Throw(true);
            }
        }
    }
}
