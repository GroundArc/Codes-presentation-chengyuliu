using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWrapper", menuName = "WrapperOwned/Wrapper")]
public class Wrapper : ScriptableObject
{
    public string wrapperName = "New Wrapper"; // Ãû³Æ
    public GameObject wrapperPrefab; // Ô¤ÖÆÌå
    //public Sprite flowerImage; // Í¼Æ¬
}
