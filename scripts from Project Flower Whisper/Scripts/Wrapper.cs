using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWrapper", menuName = "WrapperOwned/Wrapper")]
public class Wrapper : ScriptableObject
{
    public string wrapperName = "New Wrapper"; // ����
    public GameObject wrapperPrefab; // Ԥ����
    //public Sprite flowerImage; // ͼƬ
}
