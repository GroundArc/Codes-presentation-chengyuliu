using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class PlayerCtrl : NetworkBehaviour
{
    private Animator animator;
    private ActionPointSystem actionPointSystem;

    // �ڱ��ؽű�������ж���ǰ�Ƿ������ƶ�������Tween��
    private bool isMoving = false;

    // ��¼��ҵ�ǰ���ڵĸ���
    private GridItem currentGrid;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Player!");
        }

        actionPointSystem = GetComponent<ActionPointSystem>();
        if (actionPointSystem == null)
        {
            Debug.LogError("ActionPointSystem component is missing on the Player!");
        }
    }

    /// <summary>
    /// �ͻ��˵��ã�����������������ƶ���ָ�����ӡ�
    /// ������������֤�����ɹ���㲥�����пͻ���ִ��ʵ���ƶ���
    /// </summary>
    [ServerRpc]
    public void RequestMoveServerRpc(Vector2 targetGridId)
    {
        // ֻ�з����� / Host ִ���ж�
        if (!IsServer) return;

        // �ҵ�Ŀ����Ӷ���
        GridItem targetGrid = SceneController.Instance.gridSpawner.GetGridByQR((int)targetGridId.x, (int)targetGridId.y);
        if (targetGrid == null)
        {
            Debug.LogWarning($"[Server] Invalid targetGrid {targetGridId}");
            PerformMoveClientRpc(targetGridId, false); // ��Ч���� -> ����ͻ�����ʾʧ�ܻ�ʲô������
            return;
        }

        // 1. ����Ƿ����� (��ʾ��)
        if (!SceneController.Instance.IsNeighborGrid(currentGrid, targetGrid))
        {
            Debug.Log($"[Server] Grid not neighbor. Move denied.");
            PerformMoveClientRpc(targetGridId, false);
            return;
        }

        // 2. �������Ƿ��ѱ�ռ��
        if (targetGrid.content != null)
        {
            Debug.Log($"[Server] Grid is occupied by {targetGrid.contentType}. Move denied.");
            PerformMoveClientRpc(targetGridId, false);
            return;
        }

        // 3. �����ж���
        if (actionPointSystem == null || !actionPointSystem.ConsumeActionPoints(1))
        {
            Debug.Log("[Server] Not enough action points. Move denied.");
            PerformMoveClientRpc(targetGridId, false);
            return;
        }

        // �����м��ͨ�� -> �㲥�����пͻ��ˡ�ִ���ƶ���
        PerformMoveClientRpc(targetGridId, true);
    }

    /// <summary>
    /// �������˵��� -> ���пͻ���ִ��ʵ���ƶ�����������λ�á�
    /// isSuccess = false ʱ����һЩ����������ʾUI������ʾ�������ƶ���
    /// </summary>
    [ClientRpc]
    private void PerformMoveClientRpc(Vector2 targetGridId, bool isSuccess)
    {
        if (!isSuccess)
        {
            Debug.Log("[Client] Move request was denied by server.");
            return;
        }

        // �ҵ�Ŀ�����
        GridItem targetGrid = SceneController.Instance.gridSpawner.GetGridByQR((int)targetGridId.x, (int)targetGridId.y);
        if (targetGrid == null)
        {
            Debug.LogError("[Client] Target grid does not exist!");
            return;
        }

        // -- ��ʼ�����ƶ��߼� (����ԭ�� Move(...) ����) --
        if (isMoving) return;

        // �Ƴ��ɸ����������Ϣ
        if (currentGrid != null)
        {
            currentGrid.RemoveContent();
        }

        currentGrid = targetGrid;
        isMoving = true;
        UpdateAnimationState();

        // ����Ŀ��λ�ã���������ҵ�ǰ�߶�
        Vector3 targetPosition = targetGrid.GetTransPos();
        targetPosition.y = transform.position.y; // ���ı� y

        // �ȳ���Ŀ��
        Vector3 direction = targetPosition - transform.position;
        transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);

        // �� DOTween �ƶ�
        transform.DOMove(targetPosition, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // �ƶ���ɺ�ŵ��¸���
                targetGrid.PlaceContent(gameObject, ItemType.Player);
                isMoving = false;
                UpdateAnimationState();
            });
    }

    /// <summary>
    /// �Ƿ������ƶ�
    /// </summary>
    public bool IsMoving()
    {
        return isMoving;
    }

    [ClientRpc]
    public void InitializeClientPositionClientRpc(Vector2 gridId)
    {
        if (!IsOwner) return;

        GridItem grid = SceneController.Instance.gridSpawner.GetGridByQR((int)gridId.x, (int)gridId.y);
        if (grid != null)
        {
            PlacePlayerAtGrid(grid);
        }
    }


    /// <summary>
    /// ����Animator
    /// </summary>
    private void UpdateAnimationState()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    /// <summary>
    /// �ڷ������򱾵�ֱ�ӷ�����ҵ�ĳ�����ӣ���Tween��
    /// </summary>
   // ������������ҷŵ�ĳ������ʱ����
    public void PlacePlayerAtGrid(GridItem targetGrid)
    {
        if (targetGrid == null) return;

        transform.position = targetGrid.GetTransPos();
        currentGrid = targetGrid;
        targetGrid.PlaceContent(gameObject, ItemType.Player);
    }
    /// <summary>
    /// ��ǰ���ڸ���
    /// </summary>
    public GridItem GetCurrentGrid()
    {
        return currentGrid;
    }

}
