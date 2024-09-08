using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DetectClick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _XObject, _OObject;
    Vector3 _position = new Vector3(-3.4f, 15f, -0.5f);
    Camera camera;
    public static UnityAction<GameObject> CheckWinngCondition;
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("==>Mouse button down");

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider.CompareTag("Cube"))
            {
                Debug.Log("==>Mouse button down 1|" + hitInfo.collider.gameObject.name);
                GameObject Spawnobject = PlayerInfo.Instance._Player == Players.Player1 ? _XObject : _OObject;
                Spawnobject = Instantiate(Spawnobject, hitInfo.collider.gameObject.transform.position, Quaternion.identity, hitInfo.collider.gameObject.transform);
                Spawnobject.transform.localPosition = _position;
                CheckWinngCondition?.Invoke(hitInfo.collider.gameObject);
            }
            else
            {
              // Debug.Log("==>its not Hittin yaar");
            }
        }
    }
}
