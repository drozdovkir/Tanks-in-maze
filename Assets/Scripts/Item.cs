using UnityEngine;

public class Item : GameEventsListener
{
    public GameObject weaponPrefab;

    public OnGameEvent onItemPickCallback;

    private void Start()
    {
        Debug.Log("new item");
        base.Start();
    }

    public override void RoundFinished()
    {
        Debug.Log("item poka");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TankController tankController = collision.gameObject.GetComponent<TankController>();

        if ((tankController != null) && (!tankController.IsEquiped))
        {
            GameObject weapon = Instantiate(weaponPrefab);
            tankController.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        base.OnDestroy();
        if (onItemPickCallback != null)
            onItemPickCallback.Invoke();
    }
}
