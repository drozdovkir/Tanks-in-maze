using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed;
    public float timer = 10f;

    private float timeLeft;

    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timeLeft = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0f)
            gameObject.SetActive(false);
        timeLeft -= Time.deltaTime;
        transform.position += transform.up * Time.deltaTime * speed;
              
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
    }
}
