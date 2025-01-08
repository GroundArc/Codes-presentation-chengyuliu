using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentsConrtoller : MonoBehaviour
{
    [SerializeField] private GameObject greedFragment;
    [SerializeField] private GameObject gluttonyFragment;

    [SerializeField] private GameObject envyFragment;
    [SerializeField] private GameObject wrathFragment;
    [SerializeField] private GameObject slothFragment;

    private PlayerCharacter _playerCharacter;


    private void Awake()
    {
        _playerCharacter = FindObjectOfType<PlayerCharacter>();
        if (_playerCharacter == null)
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerCharacter == null)
        {
            return;
        }

        PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();

        if (other.gameObject == _playerCharacter.gameObject && playerInventory != null)
        {
            if (this.gameObject == greedFragment)
            {
                Debug.Log("Greed fragment collected");
                playerInventory.GreedAbilityCollect();
            }
            else if (this.gameObject == gluttonyFragment)
            {
                Debug.Log("Gluttony fragment collected");
                playerInventory.GluttonyAbilityCollect();
            }
            else if (this.gameObject == envyFragment)
            {
                Debug.Log("Envy fragment collected");
                playerInventory.EnvyAbilityCollect();
            }
            else if (this.gameObject == wrathFragment)
            {
                Debug.Log("Wrath fragment collected");
                playerInventory.WrathAbilityCollect();
            }
            else if (this.gameObject == slothFragment)
            {
                Debug.Log("Sloth fragment collected");
                playerInventory.SlothAbilityCollect();
            }
            else
            {
                Debug.Log("Fragment not found");
            }
            playerInventory.FragementsCollected();
            Destroy(this.gameObject);
        }
    }
}
