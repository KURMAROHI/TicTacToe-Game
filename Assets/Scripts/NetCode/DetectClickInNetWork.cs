using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class DetectClickInNetWork : NetworkBehaviour
{

    [SerializeField] private GameObject _xObject;
    [SerializeField] private GameObject _oObject;
    private Vector3 _offsetPosition = new Vector3(-3.4f, 9f, -0.5f);
    private Camera _camera;
    private float _yPositionForAnimation = -34.5f;

    private NetworkVariable<int> _currentTurnvalue = new NetworkVariable<int>(0); //0 for host and 1 for client

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!IsplayerTurn()) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsHost && IsHostTurn())
            {
                spawnPrefabServerRpc(true, Input.mousePosition);
            }
            else if (IsClient)
            {
                spawnPrefabServerRpc(false, Input.mousePosition);
            }
        }
    }

    private bool IsplayerTurn()
    {
        if (IsHost && _currentTurnvalue.Value == 0) return true;
        if (IsClient && _currentTurnvalue.Value == 1) return true;

        return false;
    }

    private bool IsHostTurn()
    {
        return _currentTurnvalue.Value == 0;
    }


    [ServerRpc(RequireOwnership = false)]
    private void spawnPrefabServerRpc(bool isX, Vector2 position)
    {
        Ray ray = _camera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider.CompareTag("Cube"))
        {
            Vector2Int _Pos = GridManager.Instance.PosofBlock(hitInfo.collider.gameObject);
            Debug.Log("is hitting:" + hitInfo.collider.gameObject.name);
            if (GridManager.Instance.IsBlockFilled(_Pos.x, _Pos.y).IsBlockFree != false)
            {
                GameObject spwanObject = isX ? _xObject : _oObject;
                 spwanObject = Instantiate(spwanObject, hitInfo.collider.gameObject.transform.position+_offsetPosition, Quaternion.identity);
                // spwanObject.SetActive(false);
                spwanObject.GetComponent<NetworkObject>().Spawn();
                // spwanObject.SetActive(true);
                spwanObject.transform.DOLocalMoveY(_yPositionForAnimation, 0.5f).SetEase(Ease.OutBack);
                _currentTurnvalue.Value = (_currentTurnvalue.Value + 1) % 2;

            }
            else
            {
                Debug.Log("==>Spawned Object is null");
            }
        }
        else
        {
            Debug.Log("==>its not Hitting yaar");
        }
    }
}
