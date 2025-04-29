using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    
    public Transform[] patrolPoints;
    
    public float patrolWaitMin = 2f;
    public float patrolWaitMax = 10f;
    
    public float patrolSpeed = 3.5f;

   
    public float chaseSpeed = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private int currentIndex = 0;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = patrolSpeed;
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (!isChasing)
        {
            // 去下一个巡逻点
            agent.SetDestination(patrolPoints[currentIndex].position);
            // 等待到达
            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
            );
            // 停留随机时间
            yield return new WaitForSeconds(Random.Range(patrolWaitMin, patrolWaitMax));
            // 下一个点
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
        }

        // 切到追逐模式
        agent.speed = chaseSpeed;
        StartCoroutine(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {
        while (isChasing)
        {
            // 不断把目标设为玩家位置
            agent.SetDestination(player.position);
            yield return null;
        }
    }

    // 当怪物进入房间区域（房间触发器必须打 tag="Room"）
    void OnTriggerEnter(Collider other)
    {
        if (!isChasing && other.CompareTag("Room"))
        {
            isChasing = true;
            // 停掉巡逻协程，开始追逐
            StopAllCoroutines();
            StartCoroutine(ChaseRoutine());
        }
    }

    // 可选：当玩家被抓
    void OnCollisionEnter(Collision col)
    {
        if (isChasing && col.collider.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().NotifyPlayerDead();
        }
    }
}
