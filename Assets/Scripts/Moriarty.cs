using UnityEngine;

public class Moriarty : Weapon
{
    private ObjectPool shards;
    private bool has_released;

    private delegate void BombActivation();
    private BombActivation bombActivationCallback;

    private void Start()
    {
        cartridge = GameObject.Find("BombStorage").GetComponent<ObjectPool>();
        shards = GameObject.Find("ShardStorage").GetComponent<ObjectPool>();

        has_released = false;
    }

    public override void Shot()
    {
        if (has_released)
        {
            if (bombActivationCallback != null)
                bombActivationCallback.Invoke();
            return;
        }

        GameObject bomb = cartridge.GetObject();

        bomb.transform.position = aimingPoint.position;
        bomb.transform.rotation = aimingPoint.rotation;
        bomb.SetActive(true);

        Vector2 forceVector = force * bomb.transform.up;

        bomb.GetComponent<Rigidbody2D>().AddForce(forceVector);
        bomb.GetComponent<BulletBehavior>().onBulletDestroyCallback += BulletDestroyed;
        bomb.GetComponent<Explosion>().pool = shards;

        bombActivationCallback += bomb.GetComponent<Explosion>().Explode;

        has_released = true;
    }

    public void BulletDestroyed()
    {
        if (onWeaponDischargeCallback != null)
            onWeaponDischargeCallback.Invoke();
    }
}