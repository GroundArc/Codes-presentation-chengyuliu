using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        None,
        Pistol,
        Rifle,
        TommyGun,
        AK47,
        ShotGun,
        Deagle,
    }

    [System.Serializable]
    public class WeaponInfo
    {
        public WeaponType weaponType;
        public string key;
        public GameObject view;

        public int bulletsLeft;
        public bool isActivated = false;

    }

    public WeaponInfo[] weapons;
    private Animator animator;
    public WeaponInfo crtWeapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        SetToWeapon(weapons[0]);
        for (int i = 1; i < weapons.Length; i++)
        {
            weapons[i].view.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (WeaponInfo w in weapons)
        {
            if (w.isActivated && Input.GetKeyDown(w.key))
            {
                Debug.Log("Switching to: " + w.weaponType.ToString());
                SetToWeapon(w);
            }
        }
    }

    public void SetToWeapon(WeaponInfo weaponToActivate)
    {
        if (crtWeapon != null && crtWeapon.view != null)
        {
            Weapon currentWeaponScript = crtWeapon.view.GetComponent<Weapon>();
            if (currentWeaponScript != null)
            {
                crtWeapon.bulletsLeft = currentWeaponScript.BulletsLeft;
            }
        }
        foreach (WeaponInfo w in weapons)
        {
            if (w != weaponToActivate)
            {
                animator.ResetTrigger("To" + w.weaponType.ToString());
            }
        }

        foreach (WeaponInfo w in weapons)
            w.view.SetActive(false);

        weaponToActivate.view.SetActive(true);
        crtWeapon = weaponToActivate;
        animator.SetTrigger("To" + weaponToActivate.weaponType.ToString());

        Weapon weaponScript = weaponToActivate.view.GetComponent<Weapon>();
        if (weaponScript != null)
        {
            weaponScript.ResetWeapon();
        }
        else
        {
            Debug.LogError("Weapon script not found on the activated weapon.");
        }
        Weapon weaponScriptToActivate = weaponToActivate.view.GetComponent<Weapon>();
        if (weaponScriptToActivate != null)
        {
            weaponScriptToActivate.ResetWeapon(); // You might need to modify ResetWeapon() to accept a bullet count or create another method
            weaponScriptToActivate.SetBullets(weaponToActivate.bulletsLeft);
        }
    }

    public GameObject getCrtWeapon()
    {
        return crtWeapon.view;
    }
    public void SwitchToWeaponType(WeaponType desiredWeaponType)
    {
        foreach (WeaponInfo w in weapons)
        {
            if (w.weaponType == desiredWeaponType)
            {
                SetToWeapon(w);
                break;
            }
        }
    }

    public void SetCurrentWeaponReloadTimeMultiplier(float multiplier)
    {
        Weapon weaponScript = crtWeapon.view.GetComponent<Weapon>();
        weaponScript?.SetReloadTime(multiplier);
    }

    public void SetCurrentWeaponFireRateMultiplier(float multiplier)
    {
        Weapon weaponScript = crtWeapon.view.GetComponent<Weapon>();
        weaponScript?.SetFireRate(multiplier);
    }
}
