using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFlower", menuName = "FlowerOwned/Flower")]
public class Flower : ScriptableObject
{
    public string flowerName = "New Flower"; // ��������
    public GameObject flowerPrefab; // ����Ԥ����
    public Sprite flowerImage; // ����ͼƬ
}
