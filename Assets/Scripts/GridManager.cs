
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GridManager Instance;
    public Vector2Int GridSize = new Vector2Int(3, 3);
    public Blockinfo[,] BlocksInfo;
    public List<GridDetails> _GridDetails = new List<GridDetails>();
    private Camera _camera;

    [SerializeField] int WinningCount = 3;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        _GridDetails.Clear();
        //  Debug.LogError("==>Setting Grid Details");
        BlocksInfo = new Blockinfo[GridSize.x, GridSize.y];
        int Row = 0;
        foreach (Transform item in this.transform)
        {
            // Debug.LogError("==>Setting Grid Details 1|" + item.name);
            if (item.CompareTag("Row"))
            {
                int Column = 0;
                foreach (Transform item2 in item)
                {
                    //Debug.LogError("==>Setting Grid Details 2|" + item2.name);
                    BlocksInfo[Row, Column].IsBlockFree = true;
                    _GridDetails.Add(new GridDetails(item2.gameObject, new Vector2Int(Row, Column)));
                    Column++;
                }
                Row++;
            }
        }
    }

    void Start()
    {
        PlayerInfo.Instance.WinningCount = WinningCount;
        _camera = Camera.main;
    }

  


    public Vector2Int PosofBlock(GameObject _Block)
    {
        //Debug.Log("==>Block Name|" + _Block.name + "::" + _GridDetails.Count);
        //foreach (var item in _GridDetails)
        for (int i = 0; i < _GridDetails.Count; i++)
        {
            GridDetails item = _GridDetails[i];
            //Debug.Log("==>Block Name|" + item.Block.name);
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
        Debug.Log("==>|" + Pos);
        BlocksInfo[Pos.x, Pos.y].IsBlockFree = false;
        BlocksInfo[Pos.x, Pos.y].IsPlayer1Occupied = ISPlayer1;
    }

    public bool CheckValidposition(int i, int j)
    {
        return (i < 0 || j < 0 || i >= GridSize.x || j >= GridSize.x) ? false : true;
    }

}




