using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
  
    public GameObject customerPrefab;
    public Transform[] customerSpawnPoints;
    public int totalOrders = 15;
    public int ordersToWin = 8;
    public int maxConcurrentCustomers = 4;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 10f;

    
    public float gameTime = 400f;
    public string winSceneName;
    public string loseSceneName;

   
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI orderProgressText;

    private int ordersSpawned = 0;
    private int ordersCompleted = 0;
    private bool playerDead = false;

    void Start()
    {
        // Ԥ��ˢ���˿�
        for (int i = 0; i < maxConcurrentCustomers; i++)
            TrySpawnCustomer();

        StartCoroutine(SpawnCustomerRoutine());
        UpdateUI();
    }

    void Update()
    {
        if (playerDead) return;

        // ����ʱ
        gameTime -= Time.deltaTime;
        UpdateUI();

        if (gameTime <= 0f)
        {
            playerDead = true;
            // ʱ�䵽�����Ѵ�Ŀ����ʤ��������ʧ��
            if (ordersCompleted >= ordersToWin)
                SceneManager.LoadScene(winSceneName);
            else
                SceneManager.LoadScene(loseSceneName);
        }

        // R ���¿�ʼ����ѡ������
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator SpawnCustomerRoutine()
    {
        while (!playerDead && ordersSpawned < totalOrders)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            TrySpawnCustomer();
        }
    }

    void TrySpawnCustomer()
    {
        if (ordersSpawned >= totalOrders) return;
        if (FindObjectsOfType<Customer>().Length >= maxConcurrentCustomers) return;

        var existing = FindObjectsOfType<Customer>();
        var freeIdx = new List<int>();
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            bool occupied = false;
            foreach (var c in existing)
                if (Vector3.Distance(c.transform.position, customerSpawnPoints[i].position) < 0.1f)
                    occupied = true;
            if (!occupied) freeIdx.Add(i);
        }
        if (freeIdx.Count == 0) return;

        int idx = freeIdx[Random.Range(0, freeIdx.Count)];
        Instantiate(customerPrefab,
                    customerSpawnPoints[idx].position,
                    customerSpawnPoints[idx].rotation);
        ordersSpawned++;
    }

    /// <summary>
    /// ��������correct=true ��ʾ������ȷ
    /// </summary>
    public void ServeOrder(ItemType deliveredType, bool correct)
    {
        if (playerDead) return;

        if (correct)
        {
            ordersCompleted++;
            UpdateUI();
            Debug.Log($"Order completed: {ordersCompleted}/{ordersToWin}");

            // һ����꣬������תʤ������
            if (ordersCompleted >= ordersToWin)
            {
                SceneManager.LoadScene(winSceneName);
                return;
            }
        }
        else
        {
            Debug.Log("Incorrect order.");
        }

        // ���굥�����ˢ��
        TrySpawnCustomer();
    }

    /// <summary>
    /// ��ұ�����ץ��
    /// </summary>
    public void NotifyPlayerDead()
    {
        if (playerDead) return;
        playerDead = true;
        SceneManager.LoadScene(loseSceneName);
    }

    void UpdateUI()
    {
        if (timerText != null)
        {
            int m = Mathf.FloorToInt(gameTime / 60f);
            int s = Mathf.FloorToInt(gameTime % 60f);
            timerText.text = $"Time: {m:D2}:{s:D2}";
        }

        if (orderProgressText != null)
            orderProgressText.text = $"Orders: {ordersCompleted}/{ordersToWin}";
    }
}
