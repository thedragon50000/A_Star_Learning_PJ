using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRun_sc : MonoBehaviour
{
    //获取网格创建脚本
    public GridMeshCreate gridMeshCreate;

    //控制网格元素grid是障碍的概率
    [Range(0, 1)] public float probability;
    bool isCreateMap = false;
    int clickNum = 0;
    Grid_sc startGrid;
    Grid_sc endGrid;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Run();
            isCreateMap = false;
            clickNum = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AStarLookRode_sc aStarLookRode = new AStarLookRode_sc();
            aStarLookRode.Init(gridMeshCreate, startGrid, endGrid);
            StartCoroutine(aStarLookRode.OnStart());
        }
    }

    private void Run()
    {
        gridMeshCreate.gridEvent = GridEvent;
        gridMeshCreate.CreateMesh();
    }

    /// <summary>
    /// 创建grid时执行的方法，通过委托传入
    /// </summary>
    /// <param name="grid"></param>
    private void GridEvent(GameObject go, int row, int column)
    {
        //機率決定是否為障礙物
        Grid_sc grid = go.GetComponent<Grid_sc>();
        float f = Random.Range(0, 1.0f);
        Color color = f <= probability ? Color.red : Color.white;
        grid.ChangeColor(color);
        grid.isHinder = f <= probability;
        grid.posX = row;
        grid.posY = column;

        // 委派點擊Event
        grid.OnClick = () =>
        {
            if (grid.isHinder)
                return;
            clickNum++;
            switch (clickNum)
            {
                case 1:
                    startGrid = grid;
                    grid.ChangeColor(Color.yellow);
                    break;
                case 2:
                    endGrid = grid;
                    grid.ChangeColor(Color.yellow);
                    isCreateMap = true;
                    break;
                default:
                    break;
            }
        };
    }
}