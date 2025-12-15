using System.Collections.Generic;
using UnityEngine;

public class CanCreator : MonoBehaviour
{
    public Can originalCan;
    public GameObject canPrefab;

    private List<Can> spawnedCans = new List<Can>();
    private Vector3 originalStartPos;
    private Quaternion originalStartRot;

    private int lastSpawnedAtPoints = 0;

    private void Awake()
    {
        originalStartPos = originalCan.transform.position;
        originalStartRot = originalCan.transform.rotation;
    }

    public void CheckSpawn(int currentPoints)
    {
        if (currentPoints >= lastSpawnedAtPoints + 100)
        {
            SpawnCan();
            lastSpawnedAtPoints += 100;
        }
    }

    void SpawnCan()
    {
        GameObject newCanObj = Instantiate(canPrefab, originalStartPos, Quaternion.identity);

        spawnedCans.Add(newCanObj.GetComponent<Can>());
    }

    public void ResetAllCans()
    {
        foreach (Can can in spawnedCans)
        {
            if (can != null)
                Destroy(can.gameObject);
        }

        spawnedCans.Clear();

        Rigidbody rb = originalCan.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        originalCan.transform.position = originalStartPos;
        originalCan.transform.rotation = originalStartRot;
    }
}
