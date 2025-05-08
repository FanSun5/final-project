// GameManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("�˿�ˢ������")]
    public GameObject customerPrefab;
    public Transform[] customerSpawnPoints; // a, b, c, d �ĸ���
    public int totalOrders = 15;            // �ܶ�����
    public int ordersToWin = 8;             // ��ɶ��ٵ���ʤ��
    public int maxConcurrentCustomers = 4;  // ͬʱ��༸���˿�
    public float minSpawnInterval = 2f;     // ˢ�¼����С
    public float maxSpawnInterval = 10f;    // ˢ�¼�����

    [Header("��Ϸʱ�� & ������")]
    public float gameTime = 400f;           // 400 �뵹��ʱ
    public string winSceneName;             // ʤ������
    public string loseSceneName;            // ��ʱʧ�ܳ���
    public string caughtSceneName;          // ������ץ����ʧ�ܳ���

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

    void Update()
    {
        // ����Ѿ�ʤ�����������Ͳ��ٵ���ʱ
        if (playerDead || ordersCompleted >= ordersToWin) return;

        // ������ʱ
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
    /// ��ҽ����ӿ�
    /// </summary>
    public void ServeOrder(ItemType deliveredType, bool correct)
    {
        if (playerDead) return;

        if (correct)
        {
            ordersCompleted++;
            Debug.Log($"complete order! done={ordersCompleted}");
            // ��꣬ʤ��
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

        // ��λ + ��飨��ʱ������� Update �ﴦ��
        TrySpawnCustomer();
    }

    /// <summary>
    /// �������ӿ�
    /// </summary>
    public void NotifyMonsterEntered()
    {
        foreach (var c in FindObjectsOfType<Customer>())
            Destroy(c.gameObject);
        Debug.Log("������꣬���й˿���ʧ");
    }

    /// <summary>
    /// ��ұ�ץ�ӿ�
    /// </summary>
    public void NotifyPlayerDead()
    {
        if (playerDead) return;
        playerDead = true;
        Debug.Log("��ұ�ץ����Ϸ����");
        SceneManager.LoadScene(caughtSceneName);
    }
}
