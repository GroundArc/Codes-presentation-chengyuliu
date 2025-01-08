using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFlower", menuName = "FlowerOwned/Flower")]
public class Flower : ScriptableObject
{
    public string flowerName = "New Flower"; // 花朵名称
    public GameObject flowerPrefab; // 花朵预制体
    public Sprite flowerImage; // 花朵图片
}
