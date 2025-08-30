using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetVisualEffectScript : MonoBehaviour
{
    [Header("References")]
    public GameObject Magnet;
    public GameObject EffectPrefab;
    [Header("Variables")]
    private int numberOfEffects = 3;
    [Header("Arrays")]
    private GameObject[] effectsArray;
    [Header("Timer")]
    private float spawnTimer = 0f;
    private float timeBetweenSpawns = 0.3f;
    // Start is called before the first frame update
    void Awake()
    {
        SetUpArrays();
    }
    private void SetUpArrays()
    {
        effectsArray = new GameObject[numberOfEffects];
        for (int i = 0; i < numberOfEffects; i++)
        {
            GameObject effect = Instantiate(EffectPrefab);
            effectsArray[i] = effect;
            effect.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
    }

    public void StartMagnetEffect(bool repel)
    {
        if (spawnTimer > timeBetweenSpawns)
        {
            spawnTimer = 0f;
            HandleOrientation();
            CreateEffect(repel);
        }
    }
    private void HandleOrientation()
    {
        if (Magnet == null) return;
        Vector2 magnetRelativePosition = (Vector2)(Magnet.transform.position - transform.position);
        transform.right = magnetRelativePosition;
    }
    private void CreateEffect(bool repel)
    {
        float forward = repel ? -1f : 1f;
        Vector2 forwardPosition = (Vector2)(transform.position + (1.9f - forward * .6f) * transform.right);
        Vector2 leftPosition = forwardPosition + (Vector2)(.4f * transform.up);
        Vector2 rightPosition = forwardPosition - (Vector2)(.4f * transform.up);
        StartCoroutine(CreateAndKillEffect(effectsArray[0], forwardPosition, forward));
        StartCoroutine(CreateAndKillEffect(effectsArray[1], leftPosition, forward));
        StartCoroutine(CreateAndKillEffect(effectsArray[2], rightPosition, forward));
    }
    public IEnumerator CreateAndKillEffect(GameObject effect, Vector2 position, float forward)
    {
        effect.transform.position = position;
        effect.transform.right = forward * transform.right;
        effect.SetActive(true);
        yield return new WaitForSeconds(timeBetweenSpawns -.1f);
        effect.SetActive(false);
    }
}
