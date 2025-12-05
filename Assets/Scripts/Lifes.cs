using UnityEngine;

public class Lifes : MonoBehaviour
{
    public int lifes = 3;

    public void LoseLife(int amount)
    {
        lifes -= amount;

        if( lifes <= 0)
        {
            Debug.Log("¡Sin vidas!");
        }
    }
}
