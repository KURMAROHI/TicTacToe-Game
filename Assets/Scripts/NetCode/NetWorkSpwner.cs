using UnityEngine;
using Unity.Netcode;
public class NetWorkSpwner : NetworkBehaviour
{
    [SerializeField] private GameObject _grid;

    public override void OnNetworkSpawn()
    {
        Debug.Log("====>OnNetworkSpawn");
        if (IsServer)
        {
            GameObject spawnedObject = Instantiate(_grid);
            spawnedObject.GetComponent<NetworkObject>().Spawn(); // Spawn the object on the network
        }
    }
}
