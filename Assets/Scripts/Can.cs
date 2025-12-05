using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Can : MonoBehaviour
{
    public float force = 10f;
    public float gravity = 7f;

    private Rigidbody rb;
    private Lifes lifes;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        lifes = FindAnyObjectByType<Lifes>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
    }

    public void OnHit(Vector3 hitPoint)
    {
        Vector3 dir = (hitPoint - transform.position).normalized;

        if (dir.y > 0)
        {
            dir.y = 0;
            dir.Normalize();
        }

        Vector3 impulse = Vector3.zero;

        if (Mathf.Abs(dir.x) > 0.1f || Mathf.Abs(dir.z) > 0.1f)
        {
            Vector3 lateral = new Vector3(dir.x, 0, dir.z).normalized;
            Vector3 lateralImpulse = -lateral * (force * 0.25f);
            Vector3 upwardImpulse = Vector3.up * (force * 0.5f);

            impulse = lateralImpulse + upwardImpulse;
        }
        else
        {
            impulse = Vector3.up * force;
        }

        rb.AddForce(impulse, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (lifes != null)
            {
                lifes.LoseLife(1);
            }
        }
    }
}
