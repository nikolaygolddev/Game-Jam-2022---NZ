using Assets.WeaponsData;
using UnityEngine;

namespace Assets.Scripts
{
    public class PistolWeapon : Weapon
    {
        public override void Shoot(GameObject spawn)
        {
            Vector3 position = spawn.transform.position;
            Instantiate(prefabBullet, position, spawn.transform.rotation);
        }
    }
}
