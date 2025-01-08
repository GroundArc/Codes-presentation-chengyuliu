using System.Collections.Generic;
using UnityEngine;

public class FlowerBehaviour : MonoBehaviour
{
    public Flower flowerData;
    public List<Renderer> colorableParts; // 可以更改材质的部分（子对象）
    public Item currentPigmentItem; // 颜料块对应的Item

}
