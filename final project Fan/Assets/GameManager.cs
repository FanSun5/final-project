// GameManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("顾客刷入设置")]
    public GameObject customerPrefab;
    public Transform[] customerSpawnPoints; // a, b, c, d 四个点
    public int totalOrders = 15;            // 总订单数
    public int ordersToWin = 8;             // 完成多少单算胜利
    public int maxConcurrentCustomers = 4;  // 同时最多几个顾客
    public float minSpawnInterval = 2f;     // 刷新间隔最小
    public float maxSpawnInterval = 10f;    // 刷新间隔最大

    [Header("游戏时长 & 场景名")]
    public float gameTime = 400f;           // 400 秒倒计时
    public string winSceneName;             // 胜利场景
    public string loseSceneName;            // 超时失败场景
    public string caughtSceneName;          // 被怪物抓到的失败场景

    private int ordersSpawned = 0;
    private int ordersCompleted = 0;
    private bool playerDead = false;

    void Start()
    {
        // 初始刷满
        for (int i = 0; i < maxConcurrentCustomers; i++)
            TrySpawnCustomer();
        // 启动自动刷新
        StartCoroutine(SpawnCustomerRoutine());
    }

    void Update()
    {
        // 如果已经胜利或阵亡，就不再倒计时
        if (playerDead || ordersCompleted >= ordersToWin) return;

        // 做倒计时
        gameTime -= Time.deltaTime;
        if (gameTime <= 0f)
        {
            Debug.Log("Time up!");
            SceneManager.LoadScene(loseSceneName);
        }
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
        List<int> freeIdx = new List<int>();
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
    /// 玩家交单接口
    /// </summary>
    public void ServeOrder(ItemType deliveredType, bool correct)
    {
        if (playerDead) return;

        if (correct)
        {
            ordersCompleted++;
            Debug.Log($"complete order! done={ordersCompleted}");
            // 达标，胜利
            if (ordersCompleted >= ordersToWin)
            {
                SceneManager.LoadScene(winSceneName);
                return;
            }
        }
        else
        {
            Debug.Log("incompleted order");
        }

        // 补位 + 检查（超时的情况在 Update 里处理）
        TrySpawnCustomer();
    }

    /// <summary>
    /// 怪物进店接口
    /// </summary>
    public void NotifyMonsterEntered()
    {
        foreach (var c in FindObjectsOfType<Customer>())
            Destroy(c.gameObject);
        Debug.Log("怪物进店，所有顾客消失");
    }

    /// <summary>
    /// 玩家被抓接口
    /// </summary>
    public void NotifyPlayerDead()
    {
        if (playerDead) return;
        playerDead = true;
        Debug.Log("玩家被抓，游戏结束");
        SceneManager.LoadScene(caughtSceneName);
    }
}
