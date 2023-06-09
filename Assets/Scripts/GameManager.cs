using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private LossPanel lossPanel;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerControllerScript;
    [SerializeField] private float spawnDistance = 30f;
    [SerializeField] private float platformWidth = 3f;

    private Vector2 initialGravity;

    private float lastSpawnPositionY = 0f;
    private float lastSpawnPositionX = 0f;

    private void Awake()
    {
        initialGravity = Physics2D.gravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.gravity = initialGravity;
            Time.timeScale = 0f;
            lossPanel.Setup(playerControllerScript.score);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }

    private void Update()
    {
        if (player.position.y > lastSpawnPositionY - spawnDistance)
        {
            SpawnPlatform();
        }
        if (Physics2D.gravity != initialGravity)
        {
            StartCoroutine(ChangeGravity());
        }
    }

    private void LateUpdate()
    {
        if (player.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }

    private IEnumerator ChangeGravity()
    {
        yield return new WaitForSeconds(5f);
        Physics2D.gravity = initialGravity;
    }

    private void SpawnPlatform()
    {
        float randomX1 = GetRandomXPosition1();
        float randomX2 = GetRandomXPosition2(randomX1);
        float randomY1 = lastSpawnPositionY + Random.Range(1.5f, 3f);
        float randomY2 = randomY1 + Random.Range(0.2f, 0.5f);

        Vector2 spawnPosition1 = new Vector2(randomX1, randomY1);
        Vector2 spawnPosition2 = new Vector2(randomX2, randomY2);

        float[] probabilities1 = { 0.1f, 0f, 0.001f, 0.08f, 0f };
        float[] probabilities2 = { 0.02f, 0.07f, 0f, 0.01f, 0.003f };

        int platformIndex1 = GetRandomPlatformIndex(probabilities1);
        int platformIndex2 = GetRandomPlatformIndex(probabilities2);

        GameObject platformPrefab1 = platformPrefabs[platformIndex1];
        GameObject platformPrefab2 = platformPrefabs[platformIndex2];

        Instantiate(platformPrefab1, spawnPosition1, Quaternion.identity);
        Instantiate(platformPrefab2, spawnPosition2, Quaternion.identity);

        lastSpawnPositionY = Mathf.Max(randomY1, randomY2);
        lastSpawnPositionX = Mathf.Max(randomX1, randomX2);
    }

    private float GetRandomXPosition1()
    {
        float minPointRange1a = -7f;
        float maxPointRange1a = lastSpawnPositionX - platformWidth;
        float sizeRange1a = maxPointRange1a - minPointRange1a;
        float range1a = Random.Range(minPointRange1a, maxPointRange1a);

        float minPointRange1b = lastSpawnPositionX + platformWidth;
        float maxPointRange1b = 7f;
        float sizeRange1b = maxPointRange1b - minPointRange1b;
        float range1b = Random.Range(minPointRange1b, maxPointRange1b);

        float randomX1;

        if (sizeRange1a > 0 && sizeRange1b <= 0)
        {
            randomX1 = range1a;
        }
        else if (sizeRange1a <= 0)
        {
            randomX1 = range1b;
        }
        else
        {
            randomX1 = Random.value <= sizeRange1a / (sizeRange1a + sizeRange1b) ? range1a : range1b;
        }

        return randomX1;
    }

    private float GetRandomXPosition2(float randomX1)
    {
        float minPointRange2a = -7f;
        float maxPointRange2a = Mathf.Min(lastSpawnPositionX, randomX1) - platformWidth;
        float sizeRange2a = maxPointRange2a - minPointRange2a;
        float range2a = Random.Range(minPointRange2a, maxPointRange2a);

        float minPointRange2b = Mathf.Min(lastSpawnPositionX, randomX1) + platformWidth;
        float maxPointRange2b = Mathf.Max(lastSpawnPositionX, randomX1) - platformWidth;
        float sizeRange2b = maxPointRange2b - minPointRange2b;
        float range2b = Random.Range(minPointRange2b, maxPointRange2b);

        float minPointRange2c = Mathf.Max(lastSpawnPositionX, randomX1) + platformWidth;
        float maxPointRange2c = 7f;
        float sizeRange2c = maxPointRange2c - minPointRange2c;
        float range2c = Random.Range(minPointRange2c, maxPointRange2c);

        float randomX2 = 0f;
        int cases = 0;

        if (sizeRange2a > 0)
            cases += 1;
        if (sizeRange2b > 0)
            cases += 2;
        if (sizeRange2c > 0)
            cases += 4;

        switch (cases)
        {
            case 1: // only range2a is valid
                randomX2 = range2a;
                break;

            case 2: // only range2b is valid
                randomX2 = range2b;
                break;

            case 3: // range2a and range2b are valid
                randomX2 = Random.value > sizeRange2a / (sizeRange2a + sizeRange2b) ? range2b : range2a;
                break;

            case 4: // only range2c is valid
                randomX2 = range2c;
                break;

            case 5: // range2a and range2c are valid
                randomX2 = Random.value > sizeRange2a / (sizeRange2a + sizeRange2c) ? range2c : range2a;
                break;

            case 6: // range2b and range2c are valid
                randomX2 = Random.value > sizeRange2b / (sizeRange2b + sizeRange2c) ? range2c : range2b;
                break;

            case 7: // all three are valid
                float rnd = Random.value;
                randomX2 = rnd <= sizeRange2a / (sizeRange2a + sizeRange2b + sizeRange2c) ? range2a : rnd <= sizeRange2b / (sizeRange2a + sizeRange2b + sizeRange2c) ? range2b : range2c;
                break;

            default:
                throw new System.InvalidOperationException("Was not able to find any valid range for randomX2!");
        }

        return randomX2;
    }

    private int GetRandomPlatformIndex(float[] probabilities)
    {
        float totalProbability = 0.0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            totalProbability += probabilities[i];
        }

        if (totalProbability == 0f)
        {
            throw new System.InvalidOperationException("Total probability is zero.");
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];

            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }
        throw new System.InvalidOperationException("Failed to determine random platform index.");
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
