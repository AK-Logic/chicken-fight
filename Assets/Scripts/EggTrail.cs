using UnityEngine;

public class EggTrail : MonoBehaviour
{
    [SerializeField]
    private GameObject eggPrefab;

    [SerializeField]
    public float spawnInterval = 0.2f; // Time interval between spawning eggs
    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEgg();
            timer = spawnInterval;
        }
    }

    private void SpawnEgg()
    {
        GameObject egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
        // You may want to set the position and rotation of the egg based on your specific requirements.
    }
}
