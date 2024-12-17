using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _serverButon;
    [SerializeField] private Button _hostButon;
    [SerializeField] private Button _clientButon;

    private void Awake()
    {
        _serverButon.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        _hostButon.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        _clientButon.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
