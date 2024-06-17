using UnityEngine;

public class MachineGun : Weapon
{
    public int maxBulletAmount = 30;
    public float frequency = 0.1f;
    public float maxSpreadAngle = 180f / 4;

    private int bulletsLeft;
    private float timeStamp;
    private bool is_shooting;

    private void Start()
    {
        cartridge = GameObject.Find("MGBulletStorage").GetComponent<ObjectPool>();

        bulletsLeft = maxBulletAmount;
        timeStamp = frequency;
        is_shooting = false;
    }

    private void Update()
    {
        if (!is_shooting)
            return;

        if (bulletsLeft <= 0)
        {
            if (onWeaponDischargeCallback != null)
                onWeaponDischargeCallback.Invoke();
            return;
        }

        timeStamp -= Time.deltaTime;

        if (timeStamp <= 0f)
        {
            float angle = NormalDistribution(0, maxSpreadAngle);

            GameObject bullet = cartridge.GetObject();

            bullet.transform.position = aimingPoint.position;
            bullet.transform.rotation = aimingPoint.rotation;
            bullet.SetActive(true);

            Vector3 rotationVector = new Vector3(0f, 0f, angle);
            bullet.transform.Rotate(rotationVector);

            Vector2 forceVector = force * bullet.transform.up;
            bullet.GetComponent<Rigidbody2D>().AddForce(forceVector);

            bulletsLeft--;
            timeStamp = frequency;
        }
    }

    public override void Shot()
    {
        if (!is_shooting)
        {
            is_shooting = true;
            return;
        }

        if (onWeaponDischargeCallback != null)
            onWeaponDischargeCallback.Invoke();
    }

    private float NormalDistribution(float mean, float std)
    {
        float u = 0f;
        float v = 0f;
        float s = 0f;

        while ((s >= 1f) || (s <= 0f))
        {
            u = Random.Range(-1f, 1f);
            v = Random.Range(-1f, 1f);

            s = u * u + v * v;
        }

        float r = Mathf.Sqrt(-2f * Mathf.Log(s) / s);

        return r * u * std + mean;
    }
}