// GameManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject customerPrefab;
    public Transform[] customerSpawnPoints; // a, b, c, d �ĸ���
    public int totalOrders = 15;            // �ܶ�����
    public int ordersToWin = 10;            // ��ɶ��ٵ���ʤ��
    public int maxConcurrentCustomers = 4;  // ͬʱ��༸���˿�
    public float minSpawnInterval = 2f;     // ˢ�¼����С
    public float maxSpawnInterval = 10f;    // ˢ�¼�����

    private int ordersSpawned = 0;
    private int ordersCompleted = 0;
    private bool playerDead = false;

    void Start()
    {
        // ��ʼˢ��
        for (int i = 0; i < maxConcurrentCustomers; i++)
            TrySpawnCustomer();
        // �����Զ�ˢ��
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
    /// �����ӿڣ����� Destroy �˿ͣ�ֻ��������Ͳ�λ
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
        Debug.Log("������꣬���й˿���ʧ");
    }

    public void NotifyPlayerDead()
    {
        if (playerDead) return;
        playerDead = true;
        Debug.Log("��ұ�ץ����Ϸ����");
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
