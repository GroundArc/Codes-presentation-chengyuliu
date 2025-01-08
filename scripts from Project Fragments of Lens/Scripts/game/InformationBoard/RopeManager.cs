using Assets.Scripts.game.InformationBoard;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct StickerGroup
{
    public StickerInformation.ID sticker1;  // ��һ�� Sticker ID
    public StickerInformation.ID sticker2;  // �ڶ��� Sticker ID
    public StickerInformation.ID generatedSticker;  // ��Ժ����ɵ� Sticker ID
}

[System.Serializable]
public struct AutoConnectionPairs
{
    public StickerInformation.ID sticker1;  // ��һ�� Sticker ID
    public StickerInformation.ID sticker2;  // �ڶ��� Sticker ID
}

[System.Serializable]
public struct SpecialPairs
{
    public StickerInformation.ID sticker1;  // ��һ�� Sticker ID
    public StickerInformation.ID sticker2;  // �ڶ��� Sticker ID
    public UnityEvent onPairConnected;  // ��Գɹ��󴥷����¼�
}


public class RopeManager : MonoBehaviour
{
    public static RopeManager Instance;

    public GameObject linePrefab; // ���� LineRenderer ��Ԥ����
    public float lineWidth = 0.1f; // Inspector �пɵ��ڵ��߿����

    private bool waitingPair = false;
    private StickerBehaviour firstSticker;
    private Outline firstPinOutline;

    // �洢����Ե�StickerID�б�
    public List<(StickerInformation.ID, StickerInformation.ID)> pairedStickers = new List<(StickerInformation.ID, StickerInformation.ID)>();

    // �洢���ɵ�����ʵ���Ա�ɾ��
    private Dictionary<(StickerInformation.ID, StickerInformation.ID), GameObject> lineInstances = new Dictionary<(StickerInformation.ID, StickerInformation.ID), GameObject>();

    // ��֧�б�ÿ����֧����ȷ��Sticker�����Ϣ
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

                    CheckSpecialPairs(pair); // ��鲢�����ض��¼�

                    // ����StickerGroup���߼���������Գɹ�����Ҫ���ɵ���Sticker
                    foreach (var group in stickerGroups)
                    {
                        if ((group.sticker1 == pair.Item1 && group.sticker2 == pair.Item2) ||
                            (group.sticker1 == pair.Item2 && group.sticker2 == pair.Item1))
                        {
                            CreateStickerAtMidPoint(firstSticker.PinPosition, pinTransform, group.generatedSticker);
                            break;
                        }
                    }

                    UpdateBranchesProgress(); // ���·�֧����
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
                specialPair.onPairConnected.Invoke(); // �����ض��¼�
                break;
            }
        }
    }


    private void CreateStickerAtMidPoint(Transform pin1, Transform pin2, StickerInformation.ID generatedStickerId)
    {
        // ��������Pin֮����м��
        Vector3 midPoint = (pin1.position + pin2.position) / 2;

        // ���� InformationBoardSystem �������� Sticker
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
            UpdateBranchesProgress(); // ���·�֧����
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

        // �洢���ɵ���ʵ�����Ա㽫��ɾ��
        var pair = (startPoint.GetComponentInParent<StickerBehaviour>().id, endPoint.GetComponentInParent<StickerBehaviour>().id);
        lineInstances[pair] = lineInstance;
    }

    public void TryAutoConnectStickers()
    {

        foreach (var pair in autoConnectionPairs)
        {
            Debug.Log($"Checking pair: {pair.sticker1} and {pair.sticker2}");

            // ������� Sticker �Ƿ�������
            bool isSticker1Generated = InformationBoardSystem.instance.IsStickerGenerated(pair.sticker1);
            bool isSticker2Generated = InformationBoardSystem.instance.IsStickerGenerated(pair.sticker2);

            Debug.Log($"Sticker {pair.sticker1} generated: {isSticker1Generated}");
            Debug.Log($"Sticker {pair.sticker2} generated: {isSticker2Generated}");

            if (isSticker1Generated && isSticker2Generated)
            {
                // �ҵ� Sticker ����Ϊ���
                var sticker1 = InformationBoardSystem.instance.FindStickerExist(pair.sticker1);
                var sticker2 = InformationBoardSystem.instance.FindStickerExist(pair.sticker2);

                if (sticker1 != null && sticker2 != null)
                {
                    var newPair = (pair.sticker1, pair.sticker2);

                    // ����Ƿ��Ѵ�������
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
        // ����һ����ɾ���������б������ڱ���ʱ�޸ļ���
        var linesToRemove = new List<(StickerInformation.ID, StickerInformation.ID)>();

        foreach (var pair in pairedStickers)
        {
            if (pair.Item1 == stickerId || pair.Item2 == stickerId)
            {
                linesToRemove.Add(pair);
            }
        }

        // ɾ��������߼���ʵ��
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

    // �������з�֧����ɶ�
    private void UpdateBranchesProgress()
    {
        foreach (var branch in branches)
        {
            branch.UpdateProgress(pairedStickers); // ���÷�֧�ĸ��·���
        }
    }
}


