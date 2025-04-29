// GameManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject customerPrefab;
    public Transform[] customerSpawnPoints; // a, b, c, d 四个点
    public int totalOrders = 15;            // 总订单数
    public int ordersToWin = 10;            // 完成多少单算胜利
    public int maxConcurrentCustomers = 4;  // 同时最多几个顾客
    public float minSpawnInterval = 2f;     // 刷新间隔最小
    public float maxSpawnInterval = 10f;    // 刷新间隔最大

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
    /// 交单接口，不再 Destroy 顾客，只处理计数和补位
    /// </summary>
    public void ServeOrder(ItemType deliveredType, bool correct)
    {
        if (correct)
        {
            ordersCompleted++;
            Debug.Log("complete order! done=" + ordersCompleted);
        }
        else
        {
            Debug.Log("incompleted order");
        }
        TrySpawnCustomer();
        CheckGameOver();
    }

    public void NotifyMonsterEntered()
    {
        foreach (var c in FindObjectsOfType<Customer>())
            Destroy(c.gameObject);
        Debug.Log("怪物进店，所有顾客消失");
    }

    public void NotifyPlayerDead()
    {
        if (playerDead) return;
        playerDead = true;
        Debug.Log("玩家被抓，游戏结束");
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (playerDead)
            Debug.Log($"Game Over: Dead  completed={ordersCompleted}/{ordersToWin}");
        else if (ordersCompleted >= ordersToWin)
            Debug.Log($"You Win! completed={ordersCompleted}/{ordersToWin}");
        else if (ordersSpawned >= totalOrders && ordersCompleted < ordersToWin)
            Debug.Log($"Game Over: orders not enough  completed={ordersCompleted}/{ordersToWin}");
    }
}
