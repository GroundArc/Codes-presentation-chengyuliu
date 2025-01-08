using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMoney : MonoBehaviour
{
    private PlayerCharacter _playerCharacter;


    // Start is called before the first frame update
    void Awake()
    {
        this._playerCharacter = FindObjectOfType<PlayerCharacter>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (GameManager.isGameOver)
        {
            return;
        }

        PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();

        if (other.gameObject == _playerCharacter.gameObject)
        {
            playerInventory.MoneyCollected(1);
            Destroy(transform.parent.gameObject);

        }
    }
}
