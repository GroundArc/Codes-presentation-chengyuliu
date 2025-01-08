using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SceneController : MonoBehaviour
{   
    public GridSpawner gridSpawner; // �������ɽű�
    public Vector2 initialPlayerPosition = new Vector2(0, 0);
    public TextMeshProUGUI hoverInfoText; // ��ʾ������Ϣ�� TextMeshPro ���
    public GameObject gridInfoPanel; // ��ͣ��Ϣ�� Panel
    private Camera mainCamera; // �������
    private GridItem _lastHoveredGrid; // ��¼��һ�������ͣ�ĸ���
    // �������ط����� gridSpawner ��
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

        // �����ҷ����߼�������������Player�ű�ȥ����
        // ������SceneController������

        // ��ʼ��UI����
        if (gridInfoPanel != null) gridInfoPanel.SetActive(false);
        if (hoverInfoText != null) hoverInfoText.text = "";
    }

    void Update()
    {
        // ���NetworkManager
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
                // ����ֱ�� localPlayer.Move(...),
                // ���Ƿ��������RPC����
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



    // ����һ����ͣ�ĸ��ӵ� Outline �رգ�������뵱ǰ���Ӳ�ͬ�� 
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
        // ����Ƿ���Ե���
        bool isReachable = IsNeighborGrid(localPlayer.GetCurrentGrid(), grid);

        // �жϸ��ӵ�״̬
        if (grid.contentType == ItemType.Chest && IsNeighborGrid(localPlayer.GetCurrentGrid(), grid))
        {
            hoverInfoText.text = "Chest";
            hoverInfoText.color = Color.blue; // ��ɫ
        }
        else if (grid.contentType == ItemType.Monster)
        {
            hoverInfoText.text = "Monster";
            hoverInfoText.color = Color.magenta; // ��ɫ
        }
        else if (grid.contentType == ItemType.Player)
        {
            hoverInfoText.text = "Player";
            hoverInfoText.color = Color.yellow; // ��ɫ
        }
        else
        {
            
            hoverInfoText.text = isReachable ? "Reachable" : "OverStep";
            hoverInfoText.color = isReachable ? Color.green : Color.red; // ��ɫ��ʾ�ɴ��ɫ��ʾ���ɴ�
        }
        // �����������ø��ӵ� Outline ������ɫ������
        Outline outline = grid.GetComponent<Outline>();
        if (outline != null)
        {
            // ���� Outline
            outline.enabled = true;

            // ����ͬ�����߼��趨 Outline ��ɫ
            if (!isReachable)
            {
                // ���ɴ� -> ��ɫ
                outline.OutlineColor = Color.red;
            }
            else
            {
                // �ɴ�
                if (grid.contentType == ItemType.Chest)
                {
                    // �ɴ����б��� -> ����ɫ
                    // ������ new Color(r, g, b, a) ָ����͸��
                    outline.OutlineColor = new Color(0.5f, 0.8f, 1f, 1f);
                }
                else if (grid.contentType == ItemType.Player)
                {
                    // �ɴ��Ҹ���������� -> ��ɫ
                    outline.OutlineColor = new Color(1f, 0.5f, 0f, 1f);
                }
                else if (grid.contentType == ItemType.Monster)
                {
                    // �ɴ��Ҹ������й��� -> ��ɫ
                    outline.OutlineColor = Color.magenta;
                }
                else
                {
                    // �ɴﵫ����ʲô��û�� -> ��ɫ
                    outline.OutlineColor = Color.green;
                }
            }
        }
    }


    public bool IsNeighborGrid(GridItem currentGrid, GridItem targetGrid)
    {
        if (currentGrid == null || targetGrid == null) return false;

        Vector2 currentId = currentGrid.Id; // ��ǰ����ID
        Vector2 targetId = targetGrid.Id;   // Ŀ�����ID

        bool isOddRow = (int)currentId.y % 2 != 0; // �жϵ�ǰ���Ƿ�Ϊ������

        // Odd-R ���ڸ��Ӽ������
        if (isOddRow)
        {
            return
                (targetId == new Vector2(currentId.x - 1, currentId.y)) ||     // ��
                (targetId == new Vector2(currentId.x + 1, currentId.y)) ||     // ��
                (targetId == new Vector2(currentId.x + 1, currentId.y + 1)) ||     // ����
                (targetId == new Vector2(currentId.x , currentId.y + 1)) || // ����
                (targetId == new Vector2(currentId.x + 1, currentId.y - 1)) ||     // ����
                (targetId == new Vector2(currentId.x , currentId.y - 1));   // ����
        }
        else // ż���� (Even Rows)
        {
            return
                (targetId == new Vector2(currentId.x - 1, currentId.y)) ||     // ��
                (targetId == new Vector2(currentId.x + 1, currentId.y)) ||     // ��
                (targetId == new Vector2(currentId.x, currentId.y + 1)) || // ����
                (targetId == new Vector2(currentId.x - 1, currentId.y + 1)) ||     // ����
                (targetId == new Vector2(currentId.x, currentId.y - 1)) || // ����
                (targetId == new Vector2(currentId.x - 1 , currentId.y - 1));       // ����
        }
    }


    //��ȡָ�����ӵ�������Ч�ھӸ��ӣ���Щ�������Խ�磬�����أ���
    public List<GridItem> GetNeighborGrids(GridItem centerGrid)
    {
        List<GridItem> neighbors = new List<GridItem>();
        if (centerGrid == null) return neighbors;

        Vector2 currentId = centerGrid.Id;
        bool isOddRow = ((int)currentId.y % 2 != 0);

        // Odd�� (isOddRow == true) ��������
        Vector2[] oddOffsets = new Vector2[]
        {
            new Vector2(currentId.x - 1, currentId.y),     // ��
            new Vector2(currentId.x + 1, currentId.y),     // ��
            new Vector2(currentId.x + 1, currentId.y + 1), // ����
            new Vector2(currentId.x,     currentId.y + 1), // ����
            new Vector2(currentId.x + 1, currentId.y - 1), // ����
            new Vector2(currentId.x,     currentId.y - 1), // ����
        };

        // Even�� (isOddRow == false) ��������
        Vector2[] evenOffsets = new Vector2[]
        {
            new Vector2(currentId.x - 1, currentId.y),     // ��
            new Vector2(currentId.x + 1, currentId.y),     // ��
            new Vector2(currentId.x,     currentId.y + 1), // ����
            new Vector2(currentId.x - 1, currentId.y + 1), // ����
            new Vector2(currentId.x,     currentId.y - 1), // ����
            new Vector2(currentId.x - 1, currentId.y - 1), // ����
        };

        // ѡȡ��Ӧƫ��
        Vector2[] neighborOffsets = isOddRow ? oddOffsets : evenOffsets;

        // ���Ի�ȡÿһ���ھӸ��ӣ����������������
        foreach (var offset in neighborOffsets)
        {
            GridItem neighbor = gridSpawner.GetGridByQR((int)offset.x, (int)offset.y);
            if (neighbor != null) // ������Ӵ���
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

}
