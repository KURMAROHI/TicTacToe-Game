using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CoinDragAndRelease : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 dragStartPos;      // Position where the drag started
    public Vector3 dragEndPos;        // Position where the drag ended
    public Vector3 dragDirection;     // Direction of drag
    public float dragTime;            // Time the drag took
    public bool isDragging = false;   // Is the user dragging the coin

    public float Speed = 100f;
    [SerializeField] Transform Board;
    private float strikerFixedY;
    public float DirMultiFlier = 1f;

    public float MaxForce = 600f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        strikerFixedY = transform.position.y;
    }

    void Update()
    {
        // Detect touch or mouse drag
       // float Dist = Vector3.Distance(Hole.position, transform.position);

        if (Input.GetMouseButtonDown(0)) // On finger/click press
        {
            // Start the drag
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y; // Distance from the camera to the board

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            dragStartPos = Camera.main.ScreenToWorldPoint(mousePos);
            dragStartPos = Board.InverseTransformPoint(dragStartPos);
            dragStartPos.y = strikerFixedY;
            Debug.Log("==>dragStartPos|" + dragStartPos + "::" + Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                Debug.Log(":==>|" + hit.transform.name);
                isDragging = true;
                rb.isKinematic = true;     // Stop physics movement during draggingf
            }
        }

        // if (Input.GetMouseButton(0) && isDragging)
        // {
        //     //dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     // Debug.Log("==>dragEndPos|" + dragEndPos);
        // }


        if (Input.GetMouseButtonUp(0) && isDragging) // On finger/click release
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y;
            dragEndPos = Camera.main.ScreenToWorldPoint(mousePos);
            dragEndPos = Board.InverseTransformPoint(dragEndPos);
            dragEndPos.y = strikerFixedY;

            {
                dragDirection = (dragEndPos - dragStartPos).normalized;  // Calculate drag direction
                                                                         //  dragDirection = new Vector3(dragDirection.x, 0, dragDirection.y).normalized;
                float dragDistance = Vector3.Distance(dragStartPos, dragEndPos);

                Debug.Log("==>|" + dragDistance + "::" + dragDirection + "::" + dragStartPos + "::" + dragEndPos);
                dragTime = Time.time;

                rb.isKinematic = false;   // Enable physics
                Vector3 localforce = new Vector3(dragDirection.x, 0, dragDirection.z) * dragDistance * Speed;
                Vector3 worldForce = Board.TransformDirection(localforce) * DirMultiFlier;

                Vector3 clampedForce = Vector3.ClampMagnitude(worldForce, MaxForce);
                Debug.LogError("==>Force|" + localforce + "::" + worldForce + "::" + clampedForce);

                rb.AddForce(clampedForce, ForceMode.Impulse); // Apply force (adjust multiplier as needed)
                                                              // rb.AddForce(dragDirection * dragDistance * Speed, ForceMode.Impulse); // Apply force (adjust multiplier as needed)
            }

            isDragging = false;  // Stop dragging
        }
    }



    public void Refresh()
    {
        Debug.Log("==>refresh");
        // string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
