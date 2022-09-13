using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawner;


    public void Spawn(Enemy enemy)
    {
        Vector3 position = spawner.gameObject.transform.position;
        Enemy temp = Instantiate(enemy, position, Quaternion.identity);
        GameManager.instance.AddEnemy(temp);
        temp.SetTransform(GameManager.instance.GetPlayer().gameObject);
    }
}
