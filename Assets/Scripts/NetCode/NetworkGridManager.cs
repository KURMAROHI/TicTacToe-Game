using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class NetworkGridManager : NetworkBehaviour
{
    public static NetworkGridManager Instance;
    // public NetworkList<GridDetails> _GridDetails = new NetworkList<GridDetails>();
    // public NetworkList<NetWorkGridDetails> gridDetailsList = new NetworkList<NetWorkGridDetails>(new NetWorkGridDetails(null, new Vector2Int(5,6)));

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }




}

[System.Serializable]
public struct NetWorkGridDetails
{
    public GameObject Block;
    public Vector2Int Blockpos;

    public NetWorkGridDetails(GameObject item, Vector2Int pos)
    {
        this.Block = item;
        this.Blockpos = pos;
    }
}
