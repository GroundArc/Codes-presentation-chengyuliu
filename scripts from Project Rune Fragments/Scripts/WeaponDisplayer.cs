using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class WeaponDisplayer : MonoBehaviour
{
    private SwitchWeapon weaponSwitcher; // Reference to the weapon switcher component
    public TextMeshProUGUI ammoText; // Reference to the UI Text component
    public TextMeshProUGUI weaponText; // Reference to the UI Text component

    private void Update()
    {
        if (!weaponSwitcher)
        {
            weaponSwitcher = GameObject.FindObjectOfType<SwitchWeapon>();
            return;
        }

        Weapon currentWeapon = weaponSwitcher.crtWeapon.view.GetComponent<Weapon>();

        if (currentWeapon)
        {
            // Get the bullets left in the magazine
            int bulletsLeft = currentWeapon.BulletsLeft;
            int totalAmmo = currentWeapon.TotalAmmo;

            // Update the UI text based on the ammo type
            if (SwitchWeapon.WeaponType.Pistol == currentWeapon.weaponType)
            {
                // You can adjust this part as needed
                ammoText.text = bulletsLeft + "/∞";
                HideWeaponText();
                if (currentWeapon.isReloading)
                {
                    ShowReloadMessage();
                }
            }
            else
            {
                ammoText.text = bulletsLeft + "/" + totalAmmo;
                HideWeaponText();
                if (currentWeapon.isReloading)
                {
                    ShowReloadMessage();
                }
            }
        }
    }

    public void ShowNoAmmoMessage()
    {
        weaponText.text = "Out of Ammo!";

    }
    public void ShowReloadMessage()
    {
        weaponText.text = "Reloading...";
    }

    public void HideWeaponText()
    {
        weaponText.text = "";
    }

}
