using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab; // 预制体上挂有NetworkObject

    // 你手动在Inspector面板里设置一些可能的出生坐标
    // 比如 (0,0), (1,0), (2,0), ...
    // 或者用代码初始化
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

        // 实例化Player预制体
        GameObject playerObj = Instantiate(playerPrefab);

        // 获取NetworkObject组件并Spawn
        NetworkObject netObj = playerObj.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId);

        // 放置玩家在指定格子
        playerObj.transform.position = spawnGrid.GetTransPos();
        var playerCtrl = playerObj.GetComponent<PlayerCtrl>();
        if (playerCtrl != null)
        {
            playerCtrl.PlacePlayerAtGrid(spawnGrid);

            // 广播给客户端，初始化位置
            playerCtrl.InitializeClientPositionClientRpc(spawnGrid.Id);
        }
    }


    /// <summary>
    /// 在 spawnPositions 列表中，从前到后寻找一个 "content == null" 的空格子。
    /// 如果找到就返回对应 GridItem，否则返回 null。
    /// </summary>
    private GridItem FindEmptyGrid()
    {
        if (SceneController.Instance == null) return null;

        foreach (var pos in spawnPositions)
        {
            var grid = SceneController.Instance.gridSpawner.GetGridByQR((int)pos.x, (int)pos.y);
            if (grid == null)
            {
                // 这个坐标点无效，跳过
                continue;
            }
            // 检查是否被占用
            if (grid.content == null)
            {
                // 找到一个空格子
                return grid;
            }
        }

        // 没有任何空格子
        return null;
    }
}