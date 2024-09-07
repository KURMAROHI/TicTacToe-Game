using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GridManager Instance;
    public Vector2Int GridSize = new Vector2Int(3, 3);
    public Blockinfo[,] BlocksInfo;
    public List<GridDetails> _GridDetails = new List<GridDetails>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        BlocksInfo = new Blockinfo[GridSize.x, GridSize.y];
        // for (int i = 0; i < GridSize.x; i++)
        // {
        //     for (int j = 0; j < GridSize.y; j++)
        //     {
        //         BlocksInfo[i, j].IsBlockFree = true;
        //     }
        // }
        int Row = 0;
        foreach (Transform item in transform)
        {
            int Column = 0;
            foreach (Transform item2 in item)
            {
                BlocksInfo[Row, Column].IsBlockFree = true;
                _GridDetails.Add(new GridDetails(item2.gameObject, new Vector2Int(Row, Column)));
                Column++;
            }
            Row++;
        }
    }


    public Vector2Int PosofBlock(GameObject _Block)
    {
        foreach (var item in _GridDetails)
        {
            if (_Block == item.Block)
            {
                return item.Blockpos;
            }
        }

        return new Vector2Int(-1, -1);
    }

    public Blockinfo IsBlockFilled(int i, int j)
    {
        return BlocksInfo[i, j];
    }

    public void UpdateDetails(Vector2Int Pos, bool ISPlayer1 = false)
    {
        BlocksInfo[Pos.x, Pos.y].IsBlockFree = false;
        BlocksInfo[Pos.x, Pos.y].IsPlayer1Occupied = ISPlayer1;
    }

    public bool CheckValidposition(int i, int j)
    {
        return (i < 0 || j < 0 || i >= GridSize.x || j >= GridSize.x) ? false : true;
    }



    [Serializable]
    public struct GridDetails
    {
        public GameObject Block;
        public Vector2Int Blockpos;

        public GridDetails(GameObject item, Vector2Int pos)
        {
            this.Block = item;
            this.Blockpos = pos;
        }
    }
}


[Serializable]
public struct Blockinfo
{
    public bool IsBlockFree;
    public bool IsPlayer1Occupied;
}
