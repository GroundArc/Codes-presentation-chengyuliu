using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int weaponID;
    public float damage;
    public float fireRate;
    private float originalFireRate;
    public float bulletSpeed;
    private float fireInterval
    {
        get { return 1f / fireRate; }
    }
    public GameObject bullet;
    public GameObject bribedBullet;
    public Transform muzzle;
    private float _nextFireTime;

    public enum AmmunitionType { Unlimited, Limited }
    public AmmunitionType ammoType;
    public int magazineSize; // This value will be set for each weapon when equipped
    public int bulletsLeft;
    public int totalAmmo;
    public bool isReloading = false;
    [SerializeField] private float reloadTime;
    private float originalReloadTime;
    private Coroutine reloadCoroutine;
    public SwitchWeapon.WeaponType weaponType;
    private SwitchWeapon switchWeapon;
    public Boolean canReload;
    [SerializeField] private bool isShotgun;
    [SerializeField] private bool isPistol;
    [SerializeField] private int pelletsCount = 8;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private bool isPlayerWeapon;
    public bool hasBeenCopied = false;
    private PlayerInventory playerInventory;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip reloadingSound;
    [SerializeField] private AudioClip cannotReloadSound;



    void Start()
    {
        _nextFireTime = Time.time;
        bulletsLeft = magazineSize; // Initialize bulletsLeft at the start
        switchWeapon = GetComponentInParent<SwitchWeapon>();
        originalFireRate = fireRate;
        originalReloadTime = reloadTime;
        playerInventory = PlayerInventory.Instance;
    }

    public int GetWeaponID()
    {
        return weaponID;
    }

    public void TryFire(Vector3 direction)
    {
        if (isReloading) // Prevent firing while reloading
            return;

        if (bulletsLeft <= 0) // Check if need to reload
        {
            if (canReload)
            {
                Reload();
            }
            else if (weaponType != SwitchWeapon.WeaponType.Pistol)
            {
                this.gameObject.SetActive(false);
                switchWeapon.SetToWeapon(switchWeapon.weapons[0]);
                playerInventory.AmmoExhausted(0);
            }
            return;
        }

        if (Time.time > _nextFireTime)
        {
            if (isPistol == false)
            {
                _nextFireTime = Time.time + fireInterval;
            }
            else
            {
                _nextFireTime = Time.time;
            }
            Fire(direction);
        }
    }

    public void Fire(Vector3 direction)
    {
        if (shootingSound != null)
        {
            if (weaponID == 5)
            {
                PlayShootingSound(0.3f);
            }
            if ((weaponID == 0 || weaponID == 3) && isPlayerWeapon)
            {
                PlayShootingSound(1.2f);
            }
            else
            {
                if (isPlayerWeapon)
                {
                    PlayShootingSound(1.0f);
                }
                else
                {
                    PlayShootingSound(0.6f);
                }
            }
        }

        bulletsLeft--;

        direction.y = 0;

        if (isShotgun)
        {
            for (int i = 0; i < pelletsCount; i++)
            {
                Vector3 spread = direction;
                spread = Quaternion.Euler(0, UnityEngine.Random.Range(-spreadAngle, spreadAngle), 0) * spread;
                Quaternion pelletRotation = Quaternion.LookRotation(spread);
                InstantiateBullet(pelletRotation);
            }
        }
        else
        {
            Quaternion bulletRotation = Quaternion.LookRotation(direction);
            InstantiateBullet(bulletRotation);
        }
    }

    void InstantiateBullet(Quaternion rotation)
    {
        if (muzzle == null)
        {
            Debug.LogWarning("Muzzle is null or destroyed");
            return;
        }
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, rotation);
        bulletInstance?.GetComponent<ProjectileManager>().InitializeBulletAttributes(damage, bulletSpeed);
        if (isPlayerWeapon)
        {
            bulletInstance?.GetComponent<ProjectileManager>().SetProjectileDamageWithMultiplier(WrathAbility.wrathMultiplier);
            bulletInstance?.GetComponent<ProjectileManager>().SetProjectileSpeedWithMultiplier(WrathAbility.wrathMultiplier);
        }
        else
        {
            bulletInstance?.GetComponent<ProjectileManager>().SetProjectileDamageWithMultiplier(GlobalEnemyEvents.Instance.GetCurrentDamageMultiplier());
            bulletInstance?.GetComponent<ProjectileManager>().SetProjectileSpeedWithMultiplier(GlobalEnemyEvents.Instance.GetCurrentSpeedMultiplier());
        }
    }

    private void PlayShootingSound(float volume)
    {
        AudioSource.PlayClipAtPoint(shootingSound, transform.position, volume);
    }

    private void PlayReloadingSound(float volume)
    {
        AudioSource.PlayClipAtPoint(reloadingSound, transform.position, volume);
    }

    public void PlayCannotReloadSound(float volume)
    {
        AudioSource.PlayClipAtPoint(cannotReloadSound, transform.position, volume);
    }


    public Vector3 GetMuzzlePosition()
    {
        return muzzle.position;
    }

    public void Reload()
    {
        if (isReloading)
            return;

        if (totalAmmo == 0 && ammoType == AmmunitionType.Limited && cannotReloadSound != null)
        {
            PlayCannotReloadSound(1.0f);
            canReload = false;
            return;
        }

        if (bulletsLeft == magazineSize && cannotReloadSound != null)
        {
            PlayCannotReloadSound(1.0f);
            return;
        }

        if (reloadingSound != null && (totalAmmo > 0 || ammoType == AmmunitionType.Unlimited))
        {
            PlayReloadingSound(1.0f);
        }

        switch (ammoType)
        {
            case AmmunitionType.Unlimited:
                // For unlimited ammunition
                isReloading = true;
                reloadCoroutine = StartCoroutine(UnlimitedReloadCoroutine());
                break;
            case AmmunitionType.Limited:
                // For limited ammunition
                isReloading = true;
                reloadCoroutine = StartCoroutine(LimitedReloadCoroutine());
                break;
        }

    }

    IEnumerator LimitedReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime); // Wait for reloadTime seconds
        int bulletsNeeded = magazineSize - bulletsLeft;
        int bulletsToReload;

        // If there are enough bullets in totalAmmo to reload magazineSize bullets, reload magazineSize bullets
        if (totalAmmo >= bulletsNeeded)
        {
            bulletsToReload = bulletsNeeded;
        }
        else
        {
            bulletsToReload = totalAmmo;
        }

        bulletsLeft += bulletsToReload;
        totalAmmo -= bulletsToReload;
        if (totalAmmo == 0)
        {
            canReload = false;
        }
        isReloading = false;
    }

    IEnumerator UnlimitedReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    public int BulletsLeft { get { return bulletsLeft; } } // Added property to get bulletsLeft

    public int TotalAmmo { get { return totalAmmo; } } // Added property to get totalAmmo

    public void SetReloadTime(float multiplier)
    {
        this.reloadTime = originalReloadTime * multiplier;
    }

    public void SetFireRate(float multiplier)
    {
        this.fireRate = originalFireRate * multiplier;
    }

    public void ResetWeapon()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        isReloading = false;
        _nextFireTime = Time.time;
        bulletsLeft = magazineSize;
    }
    public void SetBullets(int count)
    {
        bulletsLeft = count;
    }
}


