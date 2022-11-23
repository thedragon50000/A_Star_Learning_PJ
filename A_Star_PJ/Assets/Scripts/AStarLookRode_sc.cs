using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarLookRode_sc : MonoBehaviour
{
    public class Grids
    {
        private int x = -1;
        private int y = -1;

        private int G = 0;
        private int H = 0;
        private int All = 0;

        private int[] parentGrid = new int[2] {0, 0};
    }

    public List<Grids> matrix = new List<Grids>();

    public Grids[,] matrixx = new Grids[20, 20];

    public List<Grids> openGrid = new List<Grids>();

    public List<Grids> closeGrid = new List<Grids>();

    public Stack<Grids> rrodes = new Stack<Grids>();

    /*
    場景中的網格地圖
    起始點
    終點
    開放列表：存儲所有下一步可移動的格子
    封閉列表：存儲所有移動過的格子
    路徑線：存儲最終尋路的路徑格子
    */

    public GridMeshCreate meshMap;

    public Grid_sc startGrid;

    public Grid_sc endGrid;


    public List<Grid_sc> openGrids;

    public List<Grid_sc> closeGrids;

    public Stack<Grid_sc> rodes;

    public void Init(GridMeshCreate meshMap, Grid_sc startGrid, Grid_sc endGrid)

    {
        this.meshMap = meshMap;

        this.startGrid = startGrid;

        this.endGrid = endGrid;

        openGrids = new List<Grid_sc>();

        closeGrids = new List<Grid_sc>();

        rodes = new Stack<Grid_sc>();
    }

    /// <summary>
    /// 遍歷座標(i,j)的格子的周圍8格，若在邊界則會少於8格
    /// 把周圍非障礙物的格子加入openGrid，並把(i,j)關閉
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    public void TraverseItem(int i, int j)
    {
        //相鄰的所有格子，座標不會超出這個xy範圍
        int xMin = Mathf.Max(i - 1, 0);
        int xMax = Mathf.Min(i + 1, meshMap.meshRange.horizontal - 1);
        int yMin = Mathf.Max(j - 1, 0);
        int yMax = Mathf.Min(j + 1, meshMap.meshRange.vertical - 1);

        Grid_sc baseGrid = meshMap.grids[i, j].GetComponent<Grid_sc>();

        //相鄰格子都做一遍
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                Grid_sc grid = meshMap.grids[x, y].GetComponent<Grid_sc>();

                #region 剛按下Q的時候不會先執行這裡，因為連openGrids都還沒加過

                //遇到自己或是已關閉的格子時，進入下一次迴圈
                if (closeGrids.Contains(grid) || (y.Equals(j) && i.Equals(x)))
                {
                    continue;
                }

                // if (openGrids.Contains(grid))
                // {
                // if (baseGrid.All > GetLength(grid, baseGrid))
                // {
                //     print("真的有人能大於自己?");
                //     baseGrid.parentGrid = grid;
                //     SetNoteData(baseGrid);
                // }

                // continue;
                // }

                #endregion

                //如果是開啟的格子且不是障礙物的話
                if (!grid.isHinder)
                {
                    openGrids.Add(grid);

                    //相鄰格子的父格子設為預定要走的路徑的最新進度
                    //第一次執行時是設定為startGrid
                    grid.parentGrid = baseGrid;
                }
            }
        }
    }

    /// <summary>
    /// 計算目前"預定"要走的最新位置其[相鄰可以走的grid]的G、H、All
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int SetNoteData(Grid_sc grid)
    {
        //已選定的路線，若還沒選到任何一格(rodes.Count == 0)則return起始點
        Grid_sc itemParent = rodes.Count == 0 ? startGrid : grid.parentGrid;

        int numG = Mathf.Abs(itemParent.posX - grid.posX) + Mathf.Abs(itemParent.posY - grid.posY);
        int n = numG == 1 ? 10 : 14;
        grid.G = itemParent.G + n;

        int numH = Mathf.Abs(endGrid.posX - grid.posX) + Mathf.Abs(endGrid.posY - grid.posY);
        grid.H = numH * 10;
        grid.All = grid.H + grid.G;
        return grid.All;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="bejinGrid"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public int GetLength(Grid_sc bejinGrid, Grid_sc grid)
    {
        // int numG = Mathf.Abs(bejinGrid.posX - grid.posX) + Mathf.Abs(bejinGrid.posY - grid.posY);
        // int n = numG == 1 ? 10 : 14;
        // int G = bejinGrid.G + n;

        // int numH = Mathf.Abs(endGrid.posX - grid.posX) + Mathf.Abs(endGrid.posY - grid.posY);
        // int H = numH * 10;
        int All = grid.H + grid.G;
        return All;
    }

    /// <summary>
    /// 在Open中選中路徑最短的點加入路徑線，同時將路徑點加入Close裡面
    /// </summary>
    public void Traverse()
    {
        if (openGrids.Count == 0)
        {
            return;
        }

        Grid_sc minLenthGrid = openGrids[0];

        //暫時的最佳路徑
        int minLength = SetNoteData(minLenthGrid);

        //所有的Open都執行一次
        for (int i = 0; i < openGrids.Count; i++)
        {
            //在這邊比大小比出最終的最佳路徑
            if (minLength > SetNoteData(openGrids[i]))
            {
                minLenthGrid = openGrids[i];
                minLength = SetNoteData(openGrids[i]);
            }
        }

        minLenthGrid.ChangeColor(Color.green);
        Debug.Log("我在寻找人生的方向" + minLenthGrid.posX + "::::" + minLenthGrid.posY);

        closeGrids.Add(minLenthGrid);
        openGrids.Remove(minLenthGrid);
        rodes.Push(minLenthGrid);
    }

    /// <summary>
    /// 確定路徑，轉成黑色
    /// </summary>
    void GetRode()
    {
        //最終路徑
        List<Grid_sc> finalRodes = new List<Grid_sc>();

        rodes.Peek().ChangeColor(Color.black); //頂端改成黑色
        finalRodes.Insert(0, rodes.Pop());
        while (rodes.Count != 0)
        {
            //由終點往前算，如果rodes的頂端不是終點的上一格(parentGrid)則直接移除
            if (finalRodes[0].parentGrid != rodes.Peek())
            {
                rodes.Pop();
            }
            //如果是，移除並插入最終路徑的第0項
            else
            {
                rodes.Peek().ChangeColor(Color.black);
                finalRodes.Insert(0, rodes.Pop());
            }
        }
    }

    public IEnumerator OnStart()
    {
        // Item itemRoot = Map.bolls[0].item;
        rodes.Push(startGrid);
        closeGrids.Add(startGrid);

        //把startGrid周圍非障礙物的格子加入openGrid，並把startGrid關閉
        TraverseItem(startGrid.posX, startGrid.posY);

        yield return new WaitForSeconds(1);
        Traverse();

        //为了避免无法完成寻路而跳不出循环的情况，使用For来规定寻路的最大步数
        for (int i = 0; i < 600; i++)
        {
            if (rodes.Peek().posX == endGrid.posX &&
                rodes.Peek().posY == endGrid.posY)
            {
                GetRode();
                break;
            }

            TraverseItem(rodes.Peek().posX, rodes.Peek().posY);
            yield return new WaitForSeconds(0.2f);
            Traverse();
        }
    }
}
/*


*/