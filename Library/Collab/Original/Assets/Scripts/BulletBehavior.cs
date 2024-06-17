using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float timer = 10f; //bullet's lifetime

    private float timeLeft; //counter

    private void OnEnable()
    {
        timeLeft = timer;
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
            collidedObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        
    }
}
