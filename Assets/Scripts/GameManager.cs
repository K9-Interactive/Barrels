using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ConveyorLineManager[] conveyorLines;
    [SerializeField] private GameObject barrelsFloor;
    [SerializeField] private GlobalSettings globalSettings;
    [SerializeField] private BreakableObject referenceBarrel;

    [SerializeField] private TextMeshProUGUI redScoreBoard;
    [SerializeField] private TextMeshProUGUI blueScoreBoard;
    [SerializeField] private TextMeshProUGUI winner;

    [SerializeField] private GameTimer timer;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject startButton;

    private BreakableObject[] barrels;

    float barrelY = 4.22f;
    float blueBarrelX = -10.96f;
    float redBarrelX = 10.83f;
    float barrelZ = 4.4f;

    private bool isPlaying = false;

    private int redScore = 0;
    private int blueScore = 0;

    private float initialScrollSpeed;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }
    void Update()
    {
        if (barrels == null || conveyorLines == null) return;

        for (int i = 0; i < barrels.Length; i++)
        {
            if (barrels[i] == null) continue;
            if (i >= conveyorLines.Length || conveyorLines[i] == null) continue;

            Rigidbody rb = barrels[i].GetComponent<Rigidbody>();
            if (rb == null) continue;

            bool isRight = conveyorLines[i].getCurrentDirection();
            float direction = isRight ? -1f : 1f;

            Vector3 move = Vector3.right * direction * (globalSettings.globalScrollSpeed + 2) * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            Debug.Log($"Barrel {i} moved in direction: {(isRight ? "Right" : "Left")} to position: {rb.position}");
        }
    }

    public void StartGame()
    {
        gameOverPanel.SetActive(false);
        startButton.SetActive(false);
        barrels = new BreakableObject[conveyorLines.Length];
        redScore = 0;
        blueScore = 0;
        redScoreBoard.text = redScore.ToString();
        blueScoreBoard.text = blueScore.ToString();

        initialScrollSpeed = globalSettings.globalScrollSpeed;

        for (int i = 0; i < conveyorLines.Length; i++)
        {
            conveyorLines[i].OnGameStart();

            bool isLookingRight = conveyorLines[i].getCurrentDirection();
            float x = isLookingRight ? redBarrelX : blueBarrelX;
            Vector3 spawnPos = new Vector3(x, barrelY, barrelZ * i);
            Quaternion spawnRot = Quaternion.identity;

            BreakableObject barrel = Instantiate(referenceBarrel, spawnPos, spawnRot, transform);
            barrel.onDestroyed += OnBarrelDestroyed;
            barrels[i] = barrel;

            barrel.transform.localScale = referenceBarrel.transform.localScale * 2.5f;
            Debug.Log($"Spawned Barrel {i} at position: {spawnPos} with scale: {barrel.transform.localScale}");
        }

        isPlaying = true;
        StartCoroutine(IncreaseScrollSpeedRoutine());

        timer.OnTimerFinished += EndGame;
        timer.StartTimer();
    }

    private void OnBarrelDestroyed(BreakableObject destroyedBarrel)
    {
        int destroyedIndex = -1;

        for (int i = 0; i < barrels.Length; i++)
        {
            if (barrels[i] == destroyedBarrel)
            {
                Debug.Log($"Barrel {i} destroyed. Respawning in 0.5s...");
                barrels[i] = null;
                destroyedIndex = i;

                if (isPlaying)
                {
                    StartCoroutine(RespawnBarrelAfterDelay(i, 0.5f));
                }
                break;
            }
        }

        if (!isPlaying || destroyedIndex == -1) return;

        bool isLookingRight = conveyorLines[destroyedIndex].getCurrentDirection();
        if (isLookingRight)
        {
            blueScore++;
        }
        else
        {
            redScore++;
        }

        redScoreBoard.text = redScore.ToString();
        blueScoreBoard.text = blueScore.ToString();
    }

    private IEnumerator IncreaseScrollSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (isPlaying)
            {
                globalSettings.globalScrollSpeed += 0.3f;
                Debug.Log($"Scroll speed increased to: {globalSettings.globalScrollSpeed}");
            }
        }
    }

    private IEnumerator RespawnBarrelAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isPlaying) yield break;

        bool isRight = conveyorLines[index].getCurrentDirection();
        float x = isRight ? redBarrelX : blueBarrelX;
        Vector3 spawnPos = new Vector3(x, barrelY, barrelZ * index);
        Quaternion spawnRot = Quaternion.identity;

        BreakableObject newBarrel = Instantiate(referenceBarrel, spawnPos, spawnRot, transform);
        newBarrel.transform.localScale = referenceBarrel.transform.localScale * 2.5f;
        newBarrel.onDestroyed += OnBarrelDestroyed;

        barrels[index] = newBarrel;

        Debug.Log($"Respawned Barrel {index} at {spawnPos}");
    }

    public void EndGame()
    {
        isPlaying = false;
        globalSettings.globalScrollSpeed = initialScrollSpeed;
        timer.OnTimerFinished -= EndGame;

        foreach (var barrel in barrels)
        {
            if (barrel != null)
            {
                Destroy(barrel.gameObject);
            }
        }

        // Corrección: Evaluación correcta del ganador considerando empate
        if (redScore > blueScore)
        {
            winner.text = "Ganó Azul";
        }
        else if (blueScore > redScore)
        {
            winner.text = "Ganó Rojo";
        }
        else
        {
            winner.text = "Empate";
        }
        gameOverPanel.SetActive(true);
        startButton.SetActive(true);

        Debug.Log($"Game ended. Scroll speed reset to: {globalSettings.globalScrollSpeed}");
    }
}