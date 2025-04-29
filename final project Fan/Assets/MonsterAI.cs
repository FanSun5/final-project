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
            // ȥ��һ��Ѳ�ߵ�
            agent.SetDestination(patrolPoints[currentIndex].position);
            // �ȴ�����
            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
            );
            // ͣ�����ʱ��
            yield return new WaitForSeconds(Random.Range(patrolWaitMin, patrolWaitMax));
            // ��һ����
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
        }

        // �е�׷��ģʽ
        agent.speed = chaseSpeed;
        StartCoroutine(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {
        while (isChasing)
        {
            // ���ϰ�Ŀ����Ϊ���λ��
            agent.SetDestination(player.position);
            yield return null;
        }
    }

    // ��������뷿�����򣨷��䴥��������� tag="Room"��
    void OnTriggerEnter(Collider other)
    {
        if (!isChasing && other.CompareTag("Room"))
        {
            isChasing = true;
            // ͣ��Ѳ��Э�̣���ʼ׷��
            StopAllCoroutines();
            StartCoroutine(ChaseRoutine());
        }
    }

    // ��ѡ������ұ�ץ
    void OnCollisionEnter(Collision col)
    {
        if (isChasing && col.collider.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().NotifyPlayerDead();
        }
    }
}
