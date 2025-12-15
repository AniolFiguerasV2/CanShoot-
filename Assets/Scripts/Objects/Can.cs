using UnityEngine;
using UnityEngine.Rendering;

public class Can : MonoBehaviour
{
    public float force = 10f;
    public float gravity = 7f;

    public float bounceMultiplier = 0.9f;

    public bool isOriginal;

    private Rigidbody rb;
    private Lifes lifes;
    private Points points;

    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        lifes = FindAnyObjectByType<Lifes>();
        points = FindAnyObjectByType<Points>();

        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
    }

    public void OnHit(Vector3 hitPoint)
    {

        if (points != null)
        {
            points.AddPoints();
        }

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

            CanCreator creator = FindAnyObjectByType<CanCreator>();
            if (creator != null)
            {
                creator.ResetAllCans();
            }
        }

        if (collision.gameObject.CompareTag("Border"))
        {
            Bounce(collision);
        }
    }

    private void ResetCan()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
    }

    private void Bounce(Collision collision)
    {
        if (collision.contactCount == 0) { return; }

        Vector3 normal = collision.contacts[0].normal;

        Vector3 incomingVelocity = rb.linearVelocity;

        Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);

        rb.linearVelocity = reflectedVelocity * bounceMultiplier;
    }
}
