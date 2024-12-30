using UnityEngine;
using Unity.Netcode;
public class NetWorkSpwner : NetworkBehaviour
{
    [SerializeField] private GameObject _grid;
    [SerializeField] private NetworkGridManager networkGridManager;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GameObject spawnedObject = Instantiate(_grid);
            spawnedObject.GetComponent<NetworkObject>().Spawn(); // Spawn the object on the network
            Debug.Log("====>OnNetworkSpawn:" + (spawnedObject.GetComponent<NetworkObject>() != null ? spawnedObject.GetComponent<NetworkObject>().NetworkObjectId : "null"));
            networkGridManager?.SetGridDataServerRpc(spawnedObject.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }
}
