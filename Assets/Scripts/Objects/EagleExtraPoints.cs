using UnityEngine;

public class EagleExtraPoints : MonoBehaviour
{
    private Points points;
    private bool alreadyScored = false;

    private void Start()
    {
        points = FindAnyObjectByType<Points>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !alreadyScored)
        {
            alreadyScored = true;
            for (int i = 0; i < 50; i++)
            {
                points.AddPoints();
            }

            Destroy(gameObject);
        }
    }
}
