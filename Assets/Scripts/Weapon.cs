using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public ObjectPool cartridge;
    public Transform aimingPoint;

    public float force;

    public delegate void OnWeaponDischarge();
    public OnWeaponDischarge onWeaponDischargeCallback;

    public abstract void Shot();
}
