using Assets.Scripts.game.InformationBoard;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct StickerGroup
{
    public StickerInformation.ID sticker1;  // 第一个 Sticker ID
    public StickerInformation.ID sticker2;  // 第二个 Sticker ID
    public StickerInformation.ID generatedSticker;  // 配对后生成的 Sticker ID
}

[System.Serializable]
public struct AutoConnectionPairs
{
    public StickerInformation.ID sticker1;  // 第一个 Sticker ID
    public StickerInformation.ID sticker2;  // 第二个 Sticker ID
}

[System.Serializable]
public struct SpecialPairs
{
    public StickerInformation.ID sticker1;  // 第一个 Sticker ID
    public StickerInformation.ID sticker2;  // 第二个 Sticker ID
    public UnityEvent onPairConnected;  // 配对成功后触发的事件
}


public class RopeManager : MonoBehaviour
{
    public static RopeManager Instance;

    public GameObject linePrefab; // 包含 LineRenderer 的预制体
    public float lineWidth = 0.1f; // Inspector 中可调节的线宽变量

    private bool waitingPair = false;
    private StickerBehaviour firstSticker;
    private Outline firstPinOutline;

    // 存储已配对的StickerID列表
    public List<(StickerInformation.ID, StickerInformation.ID)> pairedStickers = new List<(StickerInformation.ID, StickerInformation.ID)>();

    // 存储生成的线条实例以便删除
    private Dictionary<(StickerInformation.ID, StickerInformation.ID), GameObject> lineInstances = new Dictionary<(StickerInformation.ID, StickerInformation.ID), GameObject>();

    // 分支列表，每个分支有正确的Sticker配对信息
    [Header("Branches Info")]
    public List<StickerPairInformation> branches = new List<StickerPairInformation>();

    [Header("Sticker Groups")]
    public List<StickerGroup> stickerGroups = new List<StickerGroup>();

    [Header("Auto-Connection Sticker Pairs")]
    public List<AutoConnectionPairs> autoConnectionPairs = new List<AutoConnectionPairs>();

    [Header("Special Pairs")]
    public List<SpecialPairs> specialPairs = new List<SpecialPairs>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ReceivePinInfo(Transform pinTransform, StickerInformation.ID stickerId, StickerBehaviour stickerBehaviour, Outline pinOutline)
    {
        if (!waitingPair)
        {
            firstSticker = stickerBehaviour;
            waitingPair = true;

            if (pinOutline != null)
            {
                pinOutline.enabled = true;
            }
        }
        else
        {
            if (stickerId != firstSticker.id)
            {
                var pair = (firstSticker.id, stickerId);
                var reversePair = (stickerId, firstSticker.id);

                if (pairedStickers.Contains(pair) || pairedStickers.Contains(reversePair))
                {
                    RemoveExistingLine(pair, reversePair);
                }
                else
                {
                    pairedStickers.Add(pair);
                    CreateLine(firstSticker.PinPosition, pinTransform);

                    CheckSpecialPairs(pair); // 检查并触发特定事件

                    // 根据StickerGroup的逻辑来生成配对成功后需要生成的新Sticker
                    foreach (var group in stickerGroups)
                    {
                        if ((group.sticker1 == pair.Item1 && group.sticker2 == pair.Item2) ||
                            (group.sticker1 == pair.Item2 && group.sticker2 == pair.Item1))
                        {
                            CreateStickerAtMidPoint(firstSticker.PinPosition, pinTransform, group.generatedSticker);
                            break;
                        }
                    }

                    UpdateBranchesProgress(); // 更新分支进度
                }

                if (pinOutline != null)
                {
                    pinOutline.enabled = false;
                }

                waitingPair = false;
            }
            else
            {
                if (pinOutline != null)
                {
                    pinOutline.enabled = false;
                }
                waitingPair = false;
            }
        }

        return true;
    }

    private void CheckSpecialPairs((StickerInformation.ID, StickerInformation.ID) pair)
    {
        foreach (var specialPair in specialPairs)
        {
            if ((specialPair.sticker1 == pair.Item1 && specialPair.sticker2 == pair.Item2) ||
                (specialPair.sticker1 == pair.Item2 && specialPair.sticker2 == pair.Item1))
            {
                Debug.Log($"Special pair {specialPair.sticker1} and {specialPair.sticker2} connected.");
                specialPair.onPairConnected.Invoke(); // 触发特定事件
                break;
            }
        }
    }


    private void CreateStickerAtMidPoint(Transform pin1, Transform pin2, StickerInformation.ID generatedStickerId)
    {
        // 计算两个Pin之间的中间点
        Vector3 midPoint = (pin1.position + pin2.position) / 2;

        // 调用 InformationBoardSystem 来生成新 Sticker
        InformationBoardSystem.instance.DiscoverNewStickerAtPosition(generatedStickerId, midPoint);
    }

    private void RemoveExistingLine((StickerInformation.ID, StickerInformation.ID) pair, (StickerInformation.ID, StickerInformation.ID) reversePair)
    {
        if (lineInstances.TryGetValue(pair, out GameObject existingLine) || lineInstances.TryGetValue(reversePair, out existingLine))
        {
            Destroy(existingLine);
            lineInstances.Remove(pair);
            lineInstances.Remove(reversePair);
            pairedStickers.Remove(pair);
            pairedStickers.Remove(reversePair);
            Debug.Log($"Removed existing pair between {pair.Item1} and {pair.Item2}.");
            UpdateBranchesProgress(); // 更新分支进度
        }
    }

    private void CreateLine(Transform startPoint, Transform endPoint)
    {
        if (linePrefab == null)
        {
            Debug.LogError("Line prefab is not assigned.");
            return;
        }

        // Instantiate Line prefab
        GameObject lineInstance = Instantiate(linePrefab);

        // Get the LineRenderer component
        LineRenderer lineRenderer = lineInstance.GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not found on the Line prefab.");
            return;
        }

        // Set the start and end points of the line
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);

        // Set the line width based on the inspector value
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Attach a script to dynamically update the line
        LineUpdater lineUpdater = lineInstance.AddComponent<LineUpdater>();
        lineUpdater.SetTargets(startPoint, endPoint);

        // 存储生成的线实例，以便将来删除
        var pair = (startPoint.GetComponentInParent<StickerBehaviour>().id, endPoint.GetComponentInParent<StickerBehaviour>().id);
        lineInstances[pair] = lineInstance;
    }

    public void TryAutoConnectStickers()
    {

        foreach (var pair in autoConnectionPairs)
        {
            Debug.Log($"Checking pair: {pair.sticker1} and {pair.sticker2}");

            // 检查两个 Sticker 是否都已生成
            bool isSticker1Generated = InformationBoardSystem.instance.IsStickerGenerated(pair.sticker1);
            bool isSticker2Generated = InformationBoardSystem.instance.IsStickerGenerated(pair.sticker2);

            Debug.Log($"Sticker {pair.sticker1} generated: {isSticker1Generated}");
            Debug.Log($"Sticker {pair.sticker2} generated: {isSticker2Generated}");

            if (isSticker1Generated && isSticker2Generated)
            {
                // 找到 Sticker 的行为组件
                var sticker1 = InformationBoardSystem.instance.FindStickerExist(pair.sticker1);
                var sticker2 = InformationBoardSystem.instance.FindStickerExist(pair.sticker2);

                if (sticker1 != null && sticker2 != null)
                {
                    var newPair = (pair.sticker1, pair.sticker2);

                    // 检查是否已存在连线
                    if (!pairedStickers.Contains(newPair))
                    {
                        Debug.Log($"Creating line between {pair.sticker1} and {pair.sticker2}");

                        pairedStickers.Add(newPair);
                        CreateLine(sticker1.PinPosition, sticker2.PinPosition);

                        Debug.Log($"Auto-connected {pair.sticker1} and {pair.sticker2}");
                    }
                    else
                    {
                        Debug.Log($"Pair {pair.sticker1} and {pair.sticker2} is already connected.");
                    }
                }
            }
        }

        Debug.Log("Auto-connection check complete.");
    }


    public void RemoveLinesConnectedToSticker(StickerInformation.ID stickerId)
    {
        // 创建一个待删除的连线列表，避免在遍历时修改集合
        var linesToRemove = new List<(StickerInformation.ID, StickerInformation.ID)>();

        foreach (var pair in pairedStickers)
        {
            if (pair.Item1 == stickerId || pair.Item2 == stickerId)
            {
                linesToRemove.Add(pair);
            }
        }

        // 删除相关连线及其实例
        foreach (var pair in linesToRemove)
        {
            if (lineInstances.TryGetValue(pair, out GameObject existingLine))
            {
                Destroy(existingLine);
                lineInstances.Remove(pair);
                pairedStickers.Remove(pair);
                Debug.Log($"Removed line connected to Sticker {stickerId}.");
            }
        }
    }

    // 更新所有分支的完成度
    private void UpdateBranchesProgress()
    {
        foreach (var branch in branches)
        {
            branch.UpdateProgress(pairedStickers); // 调用分支的更新方法
        }
    }
}


