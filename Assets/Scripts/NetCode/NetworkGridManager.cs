using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System;

public class NetworkGridManager : NetworkBehaviour
{
    public Vector2Int GridSize = new Vector2Int(3, 3);
    public NetworkList<NetWorkGridDetails> gridDetailsList = new NetworkList<NetWorkGridDetails>();



    // [ServerRpc(RequireOwnership = false)]
    [ServerRpc]
    public void SetGridDataServerRpc(ulong networkObjectId)
    {
        // if (!IsServer)
        // {
        Debug.LogWarning("SetGridData can only be called on the server." + networkObjectId + "::" + NetworkManager.Singleton.SpawnManager.SpawnedObjects?.Count);
        //     return;
        // }

        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];

        if (networkObject != null)
        {
            Debug.Log("Calling SetGrid data:" + networkObject.transform.name);
            int Row = 0;
            int Counter = 1;
            foreach (Transform item in networkObject.transform)
            {
                Debug.LogError("==>Setting Grid Details 1|" + item.name);
                if (item.CompareTag("Row"))
                {
                    int Column = 0;
                    foreach (Transform item2 in item)
                    {
                        item2.name = Counter.ToString();
                        gridDetailsList.Add(new NetWorkGridDetails(Counter, new Vector2Int(Row, Column), false, false));
                        Counter++;
                        Column++;
                    }
                    Row++;
                }
            }


            GridDataWrapper gridDataWrapper = new GridDataWrapper(gridDetailsList);
            UpdateGridDataClientRpc(gridDataWrapper);
        }
    }



    [ClientRpc]
    // public void UpdateGridDataClientRpc(NetworkList<NetWorkGridDetails> newGridData)
    public void UpdateGridDataClientRpc(GridDataWrapper newGridData)
    {
        // Update the grid data on the client side
        gridDetailsList.Clear();
        foreach (var gridDetail in newGridData.GridData)
        {
            gridDetailsList.Add(gridDetail);
        }

        Debug.Log("Grid data updated on the client side.");
    }



}


[System.Serializable]
public struct NetWorkGridDetails : INetworkSerializable, IEquatable<NetWorkGridDetails>
{
    public int BlockId;
    public Vector2Int Blockpos;
    public bool IsBlockFree;
    public bool IsPlayer1Occupied;

    public NetWorkGridDetails(int blockId, Vector2Int blockpos, bool isBlockFree, bool isPlayer1Occupied)
    {
        BlockId = blockId;
        Blockpos = blockpos;
        IsBlockFree = isBlockFree;
        IsPlayer1Occupied = isPlayer1Occupied;
    }

    public bool Equals(NetWorkGridDetails other)
    {
        throw new NotImplementedException();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref BlockId);
        serializer.SerializeValue(ref Blockpos);
        serializer.SerializeValue(ref IsBlockFree);
        serializer.SerializeValue(ref IsPlayer1Occupied);
    }

}

[System.Serializable]
public struct GridDataWrapper : INetworkSerializable
{
    public NetWorkGridDetails[] GridData;

    public GridDataWrapper(NetworkList<NetWorkGridDetails> gridData)
    {
        GridData = new NetWorkGridDetails[gridData.Count];
        for (int i = 0; i < gridData.Count; i++)
        {
            GridData[i] = gridData[i];
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref GridData);
    }
}


