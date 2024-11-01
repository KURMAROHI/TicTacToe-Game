using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleAttarctionRange : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField] Transform Hole, TargetPosition;
    public float MinimumDistnce = 60f;
    public float fallingSpeed = 5f;
    public bool IsAttracting = false;

    public float Mass = 10f;

    void OnEnable()
    {
        IsAttracting = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float Dist = Vector3.Distance(Hole.position, transform.position);
        if (Dist <= MinimumDistnce && IsAttracting)
        {
            //  Debug.Log("==>Distance between Hole|" + Dist + "::" + rb.velocity + "::" + rb.velocity.magnitude);
            // rb.isKinematic = true;
            rb.useGravity = true;
            rb.AddForce(Physics.gravity * Mass);
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition.position, fallingSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, Hole.position) <= 30f)
            {
                //IsAttracting = !IsAttracting;
                // rb.isKinematic = false;
                // Perform actions like resetting the Striker or adding points
                Debug.Log("Striker has fallen into the hole!");
            }

        }

    }
}
