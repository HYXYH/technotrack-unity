using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[System.Serializable]
public class BulletLauncher : Weapon
{
    [SerializeField]
    private float _bulletForce = 10;
    
    [SerializeField]
    private GameObject _bulletPrefab;
    

    public override void Fire ()
    {
        if (CheckWeaponReady())
        {
            var bullet = PoolManager.GetObject(_bulletPrefab.name, transform.position,
                transform.rotation);
            if (bullet == null)
            {
                Debug.Log("cant fire bullet, check _bulletPrefabName");
                return;
            }
            
            var force = _bulletForce * bullet.transform.forward;

            bullet.GetComponent<Rigidbody>().AddForce(force,
                ForceMode.Impulse);
            bullet.GetComponent<Bullet>().SetOwnerName(_ownerName);
            
            base.Fire();
        }

    }

}
