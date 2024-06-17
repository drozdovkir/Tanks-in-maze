using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : Weapon
{
    public int maxBulletAmount = 5;

    private int bulletsLeft;

    private void Start()
    {
        cartridge = GameObject.Find("BulletStorage").GetComponent<ObjectPool>();
        bulletsLeft = maxBulletAmount;
        //Debug.Log("TestGun");
    }

    public override void Shot()
    {
        GameObject bullet = cartridge.GetObject();

        bullet.transform.position = aimingPoint.position;
        bullet.transform.rotation = aimingPoint.rotation;
        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * 250f);

        bulletsLeft--;

        if (bulletsLeft <= 0)
        {
            if (onWeaponDischargeCallback != null)
                onWeaponDischargeCallback.Invoke();
            return;
        }
    }
}
