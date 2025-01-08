using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SceneController : MonoBehaviour
{   
    public GridSpawner gridSpawner; // 网格生成脚本
    public Vector2 initialPlayerPosition = new Vector2(0, 0);
    public TextMeshProUGUI hoverInfoText; // 显示格子信息的 TextMeshPro 组件
    public GameObject gridInfoPanel; // 悬停信息的 Panel
    private Camera mainCamera; // 主摄像机
    private GridItem _lastHoveredGrid; // 记录上一次鼠标悬停的格子
    // 给其他地方访问 gridSpawner 用
    public static SceneController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        mainCamera = Camera.main;
        gridSpawner.Init();

        // 这段玩家放置逻辑交给服务器或Player脚本去处理
        // 不再由SceneController来操作

        // 初始化UI隐藏
        if (gridInfoPanel != null) gridInfoPanel.SetActive(false);
        if (hoverInfoText != null) hoverInfoText.text = "";
    }

    void Update()
    {
        // 检查NetworkManager
        if (NetworkManager.Singleton == null) return;
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost) return;

        var localPlayerNetObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        if (localPlayerNetObj == null)
        {
            Debug.LogWarning("Local player not spawned yet!");
            return;
        }

        var playerCtrl = localPlayerNetObj.GetComponent<PlayerCtrl>();
        if (playerCtrl == null)
        {
            Debug.LogWarning("Local player object doesn't have PlayerCtrl attached.");
            return;
        }

        HandleMouseHover(playerCtrl);

        if (playerCtrl.IsMoving()) return;

        if (Input.GetMouseButtonDown(1))
        {
            HandleMouseClick(playerCtrl);
        }
    }

    private void HandleMouseClick(PlayerCtrl localPlayer)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridItem clickedGrid = hit.collider.GetComponent<GridItem>();
            if (clickedGrid != null)
            {
                // 不再直接 localPlayer.Move(...),
                // 而是发起服务器RPC请求
                localPlayer.RequestMoveServerRpc(clickedGrid.Id);
            }
        }
    }

    private void HandleMouseHover(PlayerCtrl localPlayer)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridItem hoveredGrid = hit.collider.GetComponent<GridItem>();
            if (hoveredGrid != null)
            {
                if (hoveredGrid == localPlayer.GetCurrentGrid())
                {
                    HideGridInfo();
                    DisableLastGridOutline(hoveredGrid);
                    return;
                }

                ShowGridInfo(hoveredGrid, localPlayer);
                DisableLastGridOutline(hoveredGrid);
                _lastHoveredGrid = hoveredGrid;
                return;
            }
        }

        HideGridInfo();
    }

    private void HideGridInfo()
    {
        if (gridInfoPanel != null) gridInfoPanel.SetActive(false);
        if (hoverInfoText != null) hoverInfoText.text = "";
        DisableLastGridOutline(null);
    }

    private void ShowGridInfo(GridItem hoveredGrid, PlayerCtrl localPlayer)
    {
        if (gridInfoPanel != null && !gridInfoPanel.activeSelf)
        {
            gridInfoPanel.SetActive(true);
        }
        UpdateHoverInfo(hoveredGrid, localPlayer);
    }



    // 把上一次悬停的格子的 Outline 关闭（如果它与当前格子不同） 
    private void DisableLastGridOutline(GridItem newHoveredGrid)
    {
        if (_lastHoveredGrid != null && _lastHoveredGrid != newHoveredGrid)
        {
            Outline lastOutline = _lastHoveredGrid.GetComponent<Outline>();
            if (lastOutline != null)
            {
                lastOutline.enabled = false;
            }
            _lastHoveredGrid = null;
        }
    }

    private void UpdateHoverInfo(GridItem grid, PlayerCtrl localPlayer)
    {
        if (hoverInfoText == null) return;
        // 检查是否可以到达
        bool isReachable = IsNeighborGrid(localPlayer.GetCurrentGrid(), grid);

        // 判断格子的状态
        if (grid.contentType == ItemType.Chest && IsNeighborGrid(localPlayer.GetCurrentGrid(), grid))
        {
            hoverInfoText.text = "Chest";
            hoverInfoText.color = Color.blue; // 蓝色
        }
        else if (grid.contentType == ItemType.Monster)
        {
            hoverInfoText.text = "Monster";
            hoverInfoText.color = Color.magenta; // 紫色
        }
        else if (grid.contentType == ItemType.Player)
        {
            hoverInfoText.text = "Player";
            hoverInfoText.color = Color.yellow; // 黄色
        }
        else
        {
            
            hoverInfoText.text = isReachable ? "Reachable" : "OverStep";
            hoverInfoText.color = isReachable ? Color.green : Color.red; // 黄色表示可达，红色表示不可达
        }
        // 接下来，给该格子的 Outline 设置颜色并启用
        Outline outline = grid.GetComponent<Outline>();
        if (outline != null)
        {
            // 启用 Outline
            outline.enabled = true;

            // 根据同样的逻辑设定 Outline 颜色
            if (!isReachable)
            {
                // 不可达 -> 红色
                outline.OutlineColor = Color.red;
            }
            else
            {
                // 可达
                if (grid.contentType == ItemType.Chest)
                {
                    // 可达且有宝箱 -> 淡蓝色
                    // 可以用 new Color(r, g, b, a) 指定半透明
                    outline.OutlineColor = new Color(0.5f, 0.8f, 1f, 1f);
                }
                else if (grid.contentType == ItemType.Player)
                {
                    // 可达且格子上有玩家 -> 橙色
                    outline.OutlineColor = new Color(1f, 0.5f, 0f, 1f);
                }
                else if (grid.contentType == ItemType.Monster)
                {
                    // 可达且格子上有怪物 -> 紫色
                    outline.OutlineColor = Color.magenta;
                }
                else
                {
                    // 可达但上面什么都没有 -> 绿色
                    outline.OutlineColor = Color.green;
                }
            }
        }
    }


    public bool IsNeighborGrid(GridItem currentGrid, GridItem targetGrid)
    {
        if (currentGrid == null || targetGrid == null) return false;

        Vector2 currentId = currentGrid.Id; // 当前格子ID
        Vector2 targetId = targetGrid.Id;   // 目标格子ID

        bool isOddRow = (int)currentId.y % 2 != 0; // 判断当前行是否为奇数行

        // Odd-R 相邻格子计算规则
        if (isOddRow)
        {
            return
                (targetId == new Vector2(currentId.x - 1, currentId.y)) ||     // 左
                (targetId == new Vector2(currentId.x + 1, currentId.y)) ||     // 右
                (targetId == new Vector2(currentId.x + 1, currentId.y + 1)) ||     // 右上
                (targetId == new Vector2(currentId.x , currentId.y + 1)) || // 左上
                (targetId == new Vector2(currentId.x + 1, currentId.y - 1)) ||     // 右下
                (targetId == new Vector2(currentId.x , currentId.y - 1));   // 左下
        }
        else // 偶数行 (Even Rows)
        {
            return
                (targetId == new Vector2(currentId.x - 1, currentId.y)) ||     // 左
                (targetId == new Vector2(currentId.x + 1, currentId.y)) ||     // 右
                (targetId == new Vector2(currentId.x, currentId.y + 1)) || // 右上
                (targetId == new Vector2(currentId.x - 1, currentId.y + 1)) ||     // 左上
                (targetId == new Vector2(currentId.x, currentId.y - 1)) || // 右下
                (targetId == new Vector2(currentId.x - 1 , currentId.y - 1));       // 左下
        }
    }


    //获取指定格子的所有有效邻居格子（有些坐标可能越界，不返回）。
    public List<GridItem> GetNeighborGrids(GridItem centerGrid)
    {
        List<GridItem> neighbors = new List<GridItem>();
        if (centerGrid == null) return neighbors;

        Vector2 currentId = centerGrid.Id;
        bool isOddRow = ((int)currentId.y % 2 != 0);

        // Odd行 (isOddRow == true) 相邻坐标
        Vector2[] oddOffsets = new Vector2[]
        {
            new Vector2(currentId.x - 1, currentId.y),     // 左
            new Vector2(currentId.x + 1, currentId.y),     // 右
            new Vector2(currentId.x + 1, currentId.y + 1), // 右上
            new Vector2(currentId.x,     currentId.y + 1), // 左上
            new Vector2(currentId.x + 1, currentId.y - 1), // 右下
            new Vector2(currentId.x,     currentId.y - 1), // 左下
        };

        // Even行 (isOddRow == false) 相邻坐标
        Vector2[] evenOffsets = new Vector2[]
        {
            new Vector2(currentId.x - 1, currentId.y),     // 左
            new Vector2(currentId.x + 1, currentId.y),     // 右
            new Vector2(currentId.x,     currentId.y + 1), // 右上
            new Vector2(currentId.x - 1, currentId.y + 1), // 左上
            new Vector2(currentId.x,     currentId.y - 1), // 右下
            new Vector2(currentId.x - 1, currentId.y - 1), // 左下
        };

        // 选取对应偏移
        Vector2[] neighborOffsets = isOddRow ? oddOffsets : evenOffsets;

        // 尝试获取每一个邻居格子，如果不存在则跳过
        foreach (var offset in neighborOffsets)
        {
            GridItem neighbor = gridSpawner.GetGridByQR((int)offset.x, (int)offset.y);
            if (neighbor != null) // 这个格子存在
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

}
