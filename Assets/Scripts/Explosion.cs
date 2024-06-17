using UnityEngine;

class Explosion : MonoBehaviour
{
    public int shardAmount;
    public float force;

    public ObjectPool pool;

    public void Explode()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < shardAmount; i++)
        {
            float angle = Random.Range(0f, 360f);

            GameObject shard = pool.GetObject();

            shard.transform.position = transform.position;
            shard.transform.rotation = transform.rotation;
            shard.SetActive(true);

            Vector3 rotationVector = new Vector3(0f, 0f, angle);
            shard.transform.Rotate(rotationVector);

            Vector2 forceVector = force * shard.transform.up;
            shard.GetComponent<Rigidbody2D>().AddForce(forceVector);
        }
    }
}