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
            
            agent.SetDestination(patrolPoints[currentIndex].position);
           
            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
            );
            
            yield return new WaitForSeconds(Random.Range(patrolWaitMin, patrolWaitMax));
            
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
        }

        
        agent.speed = chaseSpeed;
        StartCoroutine(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {
        while (isChasing)
        {
            
            agent.SetDestination(player.position);
            yield return null;
        }
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (!isChasing && other.CompareTag("Room"))
        {
            Debug.Log("trigger");
            isChasing = true;
            
            StopAllCoroutines();
            StartCoroutine(ChaseRoutine());
        }
    }

    
    void OnCollisionEnter(Collision col)
    {
        if (isChasing && col.collider.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().NotifyPlayerDead();
        }
    }
}
