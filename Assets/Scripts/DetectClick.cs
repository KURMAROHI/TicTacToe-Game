using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class DetectClick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _XObject, _OObject;
    Vector3 _position = new Vector3(-3.4f, 12f, -0.5f);
    Camera camera;
    public static UnityAction<Vector2Int> CheckWinngCondition;
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && PlayerInfo.Instance.PlayerInput)
        {
            // Debug.Log("==>Mouse button down");
            PlayerInfo.Instance.PlayerInput = false;
            DetectObjectOnPosition(Input.mousePosition);

        }
#else
        if(Input.touchCount > 0 && PlayerInfo.Instance.PlayerInput)
        {
            PlayerInfo.Instance.PlayerInput=false;
            Touch touch=Input.GetTouch(0);
            DetectObjectOnPosition(touch.position);
        }
#endif
    }



    void DetectObjectOnPosition(Vector3 Position)
    {
        Ray ray = camera.ScreenPointToRay(Position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider.CompareTag("Cube"))
        {
            Vector2Int _Pos = GridManager.Instance.PosofBlock(hitInfo.collider.gameObject);
            if (GridManager.Instance.IsBlockFilled(_Pos.x, _Pos.y).IsBlockFree != false)
            {
                GameObject Spawnobject = PlayerInfo.Instance._Player == Players.Player1 ? _XObject : _OObject;
                Spawnobject = Instantiate(Spawnobject, hitInfo.collider.gameObject.transform.position, Quaternion.identity, hitInfo.collider.gameObject.transform);
                Spawnobject.transform.localPosition = _position;// Spawnobject.tr
                Spawnobject.transform.DOLocalMoveY(15f, 0.5f).SetEase(Ease.OutBack);
                CheckWinngCondition?.Invoke(_Pos);

            }
        }
        else
        {
            // Debug.Log("==>its not Hittin yaar");
        }

        PlayerInfo.Instance.PlayerInput = true;
    }
}
