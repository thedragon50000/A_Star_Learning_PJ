using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 地圖的class，Grid_sc是每一個格子各自的class
/// </summary>
public class GridMeshCreate : MonoBehaviour
{
    [Serializable]
    public class MeshRange
    {
        public int horizontal;
        public int vertical;
    }

    [Header("網格地圖範圍")] public MeshRange meshRange;
    [Header("網格地圖起始點")] private Vector3 startPos;
    [Header("地圖網格的父物件")] public Transform parentTran;
    [Header("網格的prefab")] public GameObject gridPre;
    [Header("網格間隔大小")] public Vector2 scale;


    private GameObject[,] m_grids;

    public GameObject[,] grids
    {
        get { return m_grids; }
    }

    //注册模板事件
    public Action<GameObject, int, int> gridEvent;

    /// <summary>
    /// 基于挂载组件的初始数据创建网格
    /// </summary>
    public void CreateMesh()
    {
        if (meshRange.horizontal == 0 || meshRange.vertical == 0)
        {
            return;
        }

        ClearMesh();
        m_grids = new GameObject[meshRange.horizontal, meshRange.vertical];
        for (int i = 0; i < meshRange.horizontal; i++)
        {
            for (int j = 0; j < meshRange.vertical; j++)
            {
                CreateGrid(i, j);
            }
        }
    }

    /// <summary>
    /// 重载，基于传入宽高数据来创建网格
    /// </summary>
    /// <param name="height"></param>
    /// <param name="widght"></param>
    public void CreateMesh(int height, int widght)
    {
        if (widght == 0 || height == 0)
        {
            return;
        }

        ClearMesh();
        m_grids = new GameObject[widght, height];
        for (int i = 0; i < widght; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CreateGrid(i, j);
            }
        }
    }

    /// <summary>
    /// 根据位置创建一个基本的Grid物体
    /// </summary>
    /// <param name="row">x轴坐标</param>
    /// <param name="column">y轴坐标</param>
    public void CreateGrid(int row, int column)
    {
        GameObject go = GameObject.Instantiate(gridPre, parentTran);
        //T grid = go.GetComponent<T>();

        float posX = startPos.x + scale.x * row;
        float posZ = startPos.z + scale.y * column;
        go.transform.position = new Vector3(posX, startPos.y, posZ);
        go.SetActive(true);
        m_grids[row, column] = go;
        gridEvent?.Invoke(go, row, column);
    }

    /// <summary>
    /// 删除网格地图，并清除缓存数据
    /// </summary>
    public void ClearMesh()
    {
        if (m_grids == null || m_grids.Length == 0)
        {
            return;
        }

        foreach (GameObject go in m_grids)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }

        Array.Clear(m_grids, 0, m_grids.Length);
    }
}