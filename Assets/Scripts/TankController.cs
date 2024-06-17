using UnityEngine;

public class TankController : GameEventsListener
{
    private Weapon weapon;
    private Rigidbody2D rb2D;

    private GameObject currentWeaponObject; // link to weapon object that should be used
    private bool isEquiped; // true when bonus weapon is being held

    public float speed = 5f;
    public float rotationSpeed = 260f;

    public OnGameEvent onTankDestroyCallback;
    public OnGameEvent onTankAppearedCallback;

    public GameObject defaultWeaponObject;

    public bool IsEquiped { get => isEquiped; }
    public bool IsControlled { get; set; } = false;

    private void Start()
    {
        base.Start();
        EquipDefaultWeapon();
        rb2D = GetComponent<Rigidbody2D>();

        Debug.Log("tank");
        onTankAppearedCallback?.Invoke();
    }

    public override void RoundBegan()
    {
        IsControlled = true;
    }

    public override void RoundFinished()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!IsControlled)
            return;

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb2D.MovePosition(transform.position + transform.up * vertical * speed * Time.deltaTime);
        rb2D.MoveRotation(transform.rotation.eulerAngles.z - horizontal * rotationSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Attack"))
            weapon.Shot();
    }

    private void OnDestroy()
    {
        base.OnDestroy();
        if (onTankDestroyCallback != null)
            onTankDestroyCallback.Invoke();
    }

    public void EquipWeapon(GameObject weaponObject)
    {
        if (currentWeaponObject != defaultWeaponObject) // if weapon is already held uneqiup it
            Destroy(currentWeaponObject);

        weaponObject.SetActive(true);

        currentWeaponObject = weaponObject;                       // switch to new
        weapon = currentWeaponObject.GetComponent<Weapon>();      // weapon

        if (weaponObject != defaultWeaponObject)
        {
            weaponObject.transform.position = transform.position;  // set new weapon
            weaponObject.transform.rotation = transform.rotation;  // on the tank 
            weaponObject.transform.SetParent(transform);

            weapon.onWeaponDischargeCallback += EquipDefaultWeapon;

            defaultWeaponObject.SetActive(false);
        }

        isEquiped = weaponObject != defaultWeaponObject;
    }

    public void EquipDefaultWeapon()
    {
        EquipWeapon(defaultWeaponObject);
    }
}
