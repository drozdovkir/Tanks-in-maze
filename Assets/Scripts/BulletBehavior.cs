using UnityEngine;

public class BulletBehavior : GameEventsListener
{
    public float lifeTime; //bullet's lifetime

    private float timeLeft; //counter

    public delegate void OnBulletDestroy();
    public OnBulletDestroy onBulletDestroyCallback;

    private void Start()
    {
        base.Start();
    }

    public override void RoundFinished()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        timeLeft = lifeTime;
    }

    private void OnDisable()
    {
        if (onBulletDestroyCallback != null)
        {
            onBulletDestroyCallback.Invoke();
            onBulletDestroyCallback = null;
        }
    }

    private void Update()
    {
        if (timeLeft <= 0f)
            gameObject.SetActive(false);

        timeLeft -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.layer == 10) // if shell meets tank then both are destroyed
        {
            Destroy(collidedObject);
            gameObject.SetActive(false);
        }
    }
}
