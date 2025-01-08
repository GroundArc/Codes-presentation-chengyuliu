using UnityEngine;

public enum ActionType
{
    AddFlower,
    RemoveFlower,
    ApplyColor,
    RemoveColor
}

public class FlowerAction
{
    public ActionType ActionType { get; private set; }
    public GameObject Flower { get; private set; }
    public int PartIndex { get; private set; }
    public Item AppliedItem { get; private set; }

    public FlowerAction(ActionType actionType, GameObject flower, int partIndex = -1, Item appliedItem = null)
    {
        ActionType = actionType;
        Flower = flower;
        PartIndex = partIndex;
        AppliedItem = appliedItem;
    }
}
