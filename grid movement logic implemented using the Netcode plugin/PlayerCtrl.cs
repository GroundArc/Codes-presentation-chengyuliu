using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class PlayerCtrl : NetworkBehaviour
{
    private Animator animator;
    private ActionPointSystem actionPointSystem;

    // 在本地脚本里，用于判定当前是否正在移动（播放Tween）
    private bool isMoving = false;

    // 记录玩家当前所在的格子
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
    /// 客户端调用，用来向服务器请求移动到指定格子。
    /// 服务器端做验证，若成功则广播给所有客户端执行实际移动。
    /// </summary>
    [ServerRpc]
    public void RequestMoveServerRpc(Vector2 targetGridId)
    {
        // 只有服务器 / Host 执行判定
        if (!IsServer) return;

        // 找到目标格子对象
        GridItem targetGrid = SceneController.Instance.gridSpawner.GetGridByQR((int)targetGridId.x, (int)targetGridId.y);
        if (targetGrid == null)
        {
            Debug.LogWarning($"[Server] Invalid targetGrid {targetGridId}");
            PerformMoveClientRpc(targetGridId, false); // 无效格子 -> 命令客户端显示失败或什么都不做
            return;
        }

        // 1. 检查是否相邻 (简单示例)
        if (!SceneController.Instance.IsNeighborGrid(currentGrid, targetGrid))
        {
            Debug.Log($"[Server] Grid not neighbor. Move denied.");
            PerformMoveClientRpc(targetGridId, false);
            return;
        }

        // 2. 检查格子是否已被占用
        if (targetGrid.content != null)
        {
            Debug.Log($"[Server] Grid is occupied by {targetGrid.contentType}. Move denied.");
            PerformMoveClientRpc(targetGridId, false);
            return;
        }

        // 3. 消耗行动点
        if (actionPointSystem == null || !actionPointSystem.ConsumeActionPoints(1))
        {
            Debug.Log("[Server] Not enough action points. Move denied.");
            PerformMoveClientRpc(targetGridId, false);
            return;
        }

        // 若所有检查通过 -> 广播给所有客户端“执行移动”
        PerformMoveClientRpc(targetGridId, true);
    }

    /// <summary>
    /// 服务器端调用 -> 所有客户端执行实际移动动画、更新位置。
    /// isSuccess = false 时可做一些反馈（如提示UI），此示例仅不移动。
    /// </summary>
    [ClientRpc]
    private void PerformMoveClientRpc(Vector2 targetGridId, bool isSuccess)
    {
        if (!isSuccess)
        {
            Debug.Log("[Client] Move request was denied by server.");
            return;
        }

        // 找到目标格子
        GridItem targetGrid = SceneController.Instance.gridSpawner.GetGridByQR((int)targetGridId.x, (int)targetGridId.y);
        if (targetGrid == null)
        {
            Debug.LogError("[Client] Target grid does not exist!");
            return;
        }

        // -- 开始本地移动逻辑 (和你原先 Move(...) 类似) --
        if (isMoving) return;

        // 移除旧格子上玩家信息
        if (currentGrid != null)
        {
            currentGrid.RemoveContent();
        }

        currentGrid = targetGrid;
        isMoving = true;
        UpdateAnimationState();

        // 计算目标位置，并保持玩家当前高度
        Vector3 targetPosition = targetGrid.GetTransPos();
        targetPosition.y = transform.position.y; // 不改变 y

        // 先朝向目标
        Vector3 direction = targetPosition - transform.position;
        transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);

        // 用 DOTween 移动
        transform.DOMove(targetPosition, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // 移动完成后放到新格子
                targetGrid.PlaceContent(gameObject, ItemType.Player);
                isMoving = false;
                UpdateAnimationState();
            });
    }

    /// <summary>
    /// 是否正在移动
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
    /// 更新Animator
    /// </summary>
    private void UpdateAnimationState()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    /// <summary>
    /// 在服务器或本地直接放置玩家到某个格子（无Tween）
    /// </summary>
   // 当服务器把玩家放到某个格子时调用
    public void PlacePlayerAtGrid(GridItem targetGrid)
    {
        if (targetGrid == null) return;

        transform.position = targetGrid.GetTransPos();
        currentGrid = targetGrid;
        targetGrid.PlaceContent(gameObject, ItemType.Player);
    }
    /// <summary>
    /// 当前所在格子
    /// </summary>
    public GridItem GetCurrentGrid()
    {
        return currentGrid;
    }

}
