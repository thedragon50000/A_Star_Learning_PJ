using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Grid_sc : MonoBehaviour
{
    // public float gridWidght;
    // public float girdHeight;

    //座標
    public int posX;
    public int posY;

    //是否為障礙物
    public bool isHinder;

    public Color color;

    //計算預估路徑長度三個值
    public int G = 0;
    public int H = 0;
    public int All = 0;

    /// <summary>
    /// 記錄在尋路過程中，處理中的格子從哪個格子來的
    /// 也就是它的上一格
    /// </summary>
    public Grid_sc parentGrid;

    public Action OnClick;

    public void ChangeColor(Color _color)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = _color;
    }

    //滑鼠點一下，執行委派的內容
    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}