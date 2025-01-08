using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public int rowCount; // 行数
    public int colCount; // 列数
    public GameObject singleGridPrefab;
    private List<GridItem> grids;
    private GridItem currentGridItem;

    private Vector3 initPos;
    private float gridSpacing = 2f; // 格子间距

    public void Init()
    {
        initPos = transform.position;
        grids = new List<GridItem>();

        // 如果已经有子对象网格，加载它们；否则生成新的网格
        if (GetComponentsInChildren<GridItem>().Length > 0)
        {
            LoadExistingGrids();
        }
        else
        {
            SpawnNewHexGrids();
        }

        currentGridItem = grids.FirstOrDefault(); // 默认选择第一个格子
    }

    private void SpawnNewHexGrids()
    {
        grids.Clear();
        for (int r = 0; r < rowCount; r++)
        {
            for (int q = 0; q < colCount; q++)
            {
                // 六边形网格的坐标转换为世界坐标
                Vector3 position = HexToWorld(q, r);
                GameObject gridObject = Instantiate(singleGridPrefab, position, Quaternion.identity, transform);
                gridObject.transform.localEulerAngles = new Vector3(0, 30, 0);//强行旋转
                gridObject.name = $"HexGrid({q},{r})";

                GridItem gridItem = gridObject.GetComponent<GridItem>();
                gridItem.SetId(new Vector2(q, r)); // 使用轴坐标存储ID
                grids.Add(gridItem);
            }
        }
    }

    private Vector3 HexToWorld(int q, int r)
    {
        // 使用轴坐标将六边形转换为世界坐标（Odd-R 偏移布局）
        float x = gridSpacing * Mathf.Sqrt(3) * (q + 0.5f * (r % 2));
        float z = gridSpacing * 1.5f * r;
        return initPos + new Vector3(x, 0, z);
    }

    private void LoadExistingGrids()
    {
        GridItem[] items = GetComponentsInChildren<GridItem>();
        grids = items.ToList();
    }

    public GridItem GetGrid(MoveDir direction)
    {
        if (currentGridItem == null) return null;

        Vector2 curId = currentGridItem.Id;
        Vector2 targetId = curId;

        // 根据 Odd-R 偏移布局计算六边形相邻格子
        bool isOddRow = (int)curId.y % 2 != 0;

        switch (direction)
        {
            case MoveDir.UPLEFT:
                targetId = isOddRow
                    ? new Vector2(curId.x, curId.y + 1)
                    : new Vector2(curId.x - 1, curId.y + 1);
                break;
            case MoveDir.UPRIGHT:
                targetId = isOddRow
                    ? new Vector2(curId.x + 1, curId.y + 1)
                    : new Vector2(curId.x, curId.y + 1);
                break;
            case MoveDir.LEFT:
                targetId = new Vector2(curId.x - 1, curId.y);
                break;
            case MoveDir.RIGHT:
                targetId = new Vector2(curId.x + 1, curId.y);
                break;
            case MoveDir.DOWNLEFT:
                targetId = isOddRow
                    ? new Vector2(curId.x, curId.y - 1)
                    : new Vector2(curId.x - 1, curId.y - 1);
                break;
            case MoveDir.DOWNRIGHT:
                targetId = isOddRow
                    ? new Vector2(curId.x + 1, curId.y - 1)
                    : new Vector2(curId.x, curId.y - 1);
                break;
        }

        if (IsOutOfBounds(targetId)) return null;

        GridItem targetGrid = grids.Find(grid => grid.Id == targetId);
        if (targetGrid != null)
        {
            currentGridItem = targetGrid; // 更新当前位置
        }
        return targetGrid;
    }

    private bool IsOutOfBounds(Vector2 id)
    {
        return id.x < 0 || id.x >= colCount || id.y < 0 || id.y >= rowCount;
    }

    public GridItem GetGridByQR(int q, int r)
    {
        return grids.Find(grid => grid.Id == new Vector2(q, r));
    }
}




public enum MoveDir
{
    LEFT,
    RIGHT,
    UPLEFT,
    DOWNLEFT,
    UPRIGHT,
    DOWNRIGHT,
}
