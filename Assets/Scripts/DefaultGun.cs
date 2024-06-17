using UnityEngine;

public class DefaultGun : Weapon
{
    private int bulletsLeft = 1;

    private void Start()
    {
        cartridge = GameObject.Find("BulletStorage").GetComponent<ObjectPool>();
    }

    public override void Shot()
    {
        if (bulletsLeft <= 0)
            return;

        GameObject bullet = cartridge.GetObject();

        bullet.transform.position = aimingPoint.position;
        bullet.transform.rotation = aimingPoint.rotation;
        bullet.SetActive(true);

        Vector2 forceVector = force * bullet.transform.up;

        bullet.GetComponent<Rigidbody2D>().AddForce(forceVector);
        bullet.GetComponent<BulletBehavior>().onBulletDestroyCallback += BulletDestroyed;

        bulletsLeft--;
    }

    public void BulletDestroyed()
    {
        bulletsLeft++;
    }
}
