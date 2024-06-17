using UnityEngine;

class RocketLauncher : Weapon
{
    private bool hasReleased;

    private void Start()
    {
        cartridge = GameObject.Find("RocketStorage").GetComponent<ObjectPool>();

        hasReleased = false;
    }

    public override void Shot()
    {
        if (!hasReleased)
        {
            GameObject rocket = cartridge.GetObject();

            rocket.transform.position = aimingPoint.position;
            rocket.transform.rotation = aimingPoint.rotation;
            rocket.SetActive(true);

            BulletBehavior bulletBehavior = rocket.GetComponent<BulletBehavior>();
            bulletBehavior.onBulletDestroyCallback += RocketDestroyed;
            
            hasReleased = true;
        }
    }

    public void RocketDestroyed()
    {
        if (onWeaponDischargeCallback != null)
            onWeaponDischargeCallback.Invoke();
    }
}