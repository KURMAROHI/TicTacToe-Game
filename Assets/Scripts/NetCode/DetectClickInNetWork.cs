using Unity.Netcode;
using UnityEngine;

public class DetectClickInNetWork : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        Debug.Log("====>OnNetworkSpawn");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsServer)
            {
                Debug.Log("Space Clicked IsServer");
            }
            else
            {
                Debug.LogError("Not aaa Server");
            }
        }
    }
}
