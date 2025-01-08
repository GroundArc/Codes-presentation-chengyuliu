using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class BosswWeaponManager : MonoBehaviour
{
    private PlayerCharacter _playerCharacter;
    private SwitchWeapon _switchWeapon;
    private PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerInventory.Instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        this._playerCharacter = FindObjectOfType<PlayerCharacter>();
        this._switchWeapon = FindObjectOfType<SwitchWeapon>();
    }

    void OnTriggerEnter(Collider other)
    {


        if (GameManager.isGameOver == false && other.gameObject == _playerCharacter.gameObject && this.gameObject.tag == "DroppedWeapon")
        {
            Weapon enemyWeapon = GetComponent<Weapon>();
            if (enemyWeapon != null)
            {
                // Get the type of the enemy's weapon.
                SwitchWeapon.WeaponType enemyWeaponType = enemyWeapon.weaponType;

                // Activate the weapon that matches the enemy's weapon.
                foreach (SwitchWeapon.WeaponInfo playerWeaponInfo in _switchWeapon.weapons)
                {
                    if (playerWeaponInfo.weaponType == enemyWeaponType)
                    {
                        playerWeaponInfo.isActivated = true;  // Activate the weapon
                        break;
                    }
                }

                // Switch to the enemy weapon type
                _switchWeapon.SwitchToWeaponType(enemyWeaponType);
                GameObject playerWeaponObject = _switchWeapon.getCrtWeapon();
                Weapon playerWeapon = playerWeaponObject.GetComponent<Weapon>();
                playerWeapon.magazineSize = enemyWeapon.magazineSize;
                playerWeapon.bulletsLeft = playerWeapon.magazineSize;
                playerWeapon.totalAmmo += enemyWeapon.magazineSize * 2;
                playerWeapon.canReload = true;
                playerInventory.BossWeaponCollected(playerWeapon.GetWeaponID());
                Destroy(gameObject);
            }
        }
    }


}
