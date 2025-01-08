using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public int rowCount; // ����
    public int colCount; // ����
    public GameObject singleGridPrefab;
    private List<GridItem> grids;
    private GridItem currentGridItem;

    private Vector3 initPos;
    private float gridSpacing = 2f; // ���Ӽ��

    public void Init()
    {
        initPos = transform.position;
        grids = new List<GridItem>();

        // ����Ѿ����Ӷ������񣬼������ǣ����������µ�����
        if (GetComponentsInChildren<GridItem>().Length > 0)
        {
            LoadExistingGrids();
        }
        else
        {
            SpawnNewHexGrids();
        }

        currentGridItem = grids.FirstOrDefault(); // Ĭ��ѡ���һ������
    }

    private void SpawnNewHexGrids()
    {
        grids.Clear();
        for (int r = 0; r < rowCount; r++)
        {
            for (int q = 0; q < colCount; q++)
            {
                // ���������������ת��Ϊ��������
                Vector3 position = HexToWorld(q, r);
                GameObject gridObject = Instantiate(singleGridPrefab, position, Quaternion.identity, transform);
                gridObject.transform.localEulerAngles = new Vector3(0, 30, 0);//ǿ����ת
                gridObject.name = $"HexGrid({q},{r})";

                GridItem gridItem = gridObject.GetComponent<GridItem>();
                gridItem.SetId(new Vector2(q, r)); // ʹ��������洢ID
                grids.Add(gridItem);
            }
        }
    }

    private Vector3 HexToWorld(int q, int r)
    {
        // ʹ�������꽫������ת��Ϊ�������꣨Odd-R ƫ�Ʋ��֣�
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

        // ���� Odd-R ƫ�Ʋ��ּ������������ڸ���
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
            currentGridItem = targetGrid; // ���µ�ǰλ��
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
