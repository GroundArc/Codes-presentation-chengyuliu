using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab; // Ԥ�����Ϲ���NetworkObject

    // ���ֶ���Inspector���������һЩ���ܵĳ�������
    // ���� (0,0), (1,0), (2,0), ...
    // �����ô����ʼ��
    [SerializeField] private List<Vector2> spawnPositions = new List<Vector2>();

    void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        GridItem spawnGrid = FindEmptyGrid();
        if (spawnGrid == null)
        {
            Debug.LogError("All spawn positions are occupied, cannot place new player!");
            return;
        }

        // ʵ����PlayerԤ����
        GameObject playerObj = Instantiate(playerPrefab);

        // ��ȡNetworkObject�����Spawn
        NetworkObject netObj = playerObj.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId);

        // ���������ָ������
        playerObj.transform.position = spawnGrid.GetTransPos();
        var playerCtrl = playerObj.GetComponent<PlayerCtrl>();
        if (playerCtrl != null)
        {
            playerCtrl.PlacePlayerAtGrid(spawnGrid);

            // �㲥���ͻ��ˣ���ʼ��λ��
            playerCtrl.InitializeClientPositionClientRpc(spawnGrid.Id);
        }
    }


    /// <summary>
    /// �� spawnPositions �б��У���ǰ����Ѱ��һ�� "content == null" �Ŀո��ӡ�
    /// ����ҵ��ͷ��ض�Ӧ GridItem�����򷵻� null��
    /// </summary>
    private GridItem FindEmptyGrid()
    {
        if (SceneController.Instance == null) return null;

        foreach (var pos in spawnPositions)
        {
            var grid = SceneController.Instance.gridSpawner.GetGridByQR((int)pos.x, (int)pos.y);
            if (grid == null)
            {
                // ����������Ч������
                continue;
            }
            // ����Ƿ�ռ��
            if (grid.content == null)
            {
                // �ҵ�һ���ո���
                return grid;
            }
        }

        // û���κοո���
        return null;
    }
}