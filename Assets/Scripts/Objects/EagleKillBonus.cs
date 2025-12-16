using UnityEngine;

public class EagleKillBonus : MonoBehaviour
{
    private Lifes lifes;
    private bool used = false;

    private void Start()
    {
        lifes = FindAnyObjectByType<Lifes>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !used)
        {
            used = true;

            lifes.LoseLife(lifes.lifes);

            Destroy(gameObject);
        }
    }
}
