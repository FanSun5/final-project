using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] patrolPointsA;
    public Transform[] patrolPointsB;
    [Tooltip("到这个索引时开始追逐")]
    public int chaseTriggerIndex = 2;

    public float patrolWaitMin = 2f;
    public float patrolWaitMax = 10f;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float giveUpTime = 5f;

    public AudioClip chaseClip;
    public AudioClip[] patrolClips;

    enum State { Patrol, Chase, GiveUp }
    State state = State.Patrol;

    NavMeshAgent agent;
    playerController player;
    AudioSource audioSource;
    Transform[] patrolPoints;
    int currentIndex;
    float waitTimer;
    float giveUpTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<playerController>();
        audioSource = GetComponent<AudioSource>();

        patrolPoints = (Random.value < 0.5f ? patrolPointsA : patrolPointsB);
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("MonsterAI: 请在 Inspector 填入巡逻点数组！");
            enabled = false;
            return;
        }

        currentIndex = 0;
        agent.speed = patrolSpeed;
        GoToPoint(currentIndex);
        waitTimer = Random.Range(patrolWaitMin, patrolWaitMax);
    }

    void Update()
    {
        switch (state)
        {
            case State.Patrol: PatrolUpdate(); break;
            case State.Chase: ChaseUpdate(); break;
            case State.GiveUp: GiveUpUpdate(); break;
        }
    }

    void PatrolUpdate()
    {
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                if (currentIndex == chaseTriggerIndex)
                {
                    state = State.Chase;
                    if (chaseClip != null)
                        audioSource.PlayOneShot(chaseClip);
                    return;
                }

                // 去下一个点
                currentIndex = (currentIndex + 1) % patrolPoints.Length;
                GoToPoint(currentIndex);
                waitTimer = Random.Range(patrolWaitMin, patrolWaitMax);
                if (patrolClips != null &&
                    currentIndex < patrolClips.Length &&
                    patrolClips[currentIndex] != null)
                {
                    audioSource.PlayOneShot(patrolClips[currentIndex]);
                }
            }
        }
    }

    void ChaseUpdate()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);

        if (player.isHiding)
        {
            state = State.GiveUp;
            giveUpTimer = giveUpTime;
            // 立刻停止当前路径，让 NavMeshAgent 放弃追逐路线
            agent.ResetPath();
        }
    }

    void GiveUpUpdate()
    {
        giveUpTimer -= Time.deltaTime;
        if (giveUpTimer <= 0f)
        {
            state = State.Patrol;
            agent.speed = patrolSpeed;
            
            agent.ResetPath();
            currentIndex = 0;
            GoToPoint(0);
            waitTimer = Random.Range(patrolWaitMin, patrolWaitMax);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (state == State.Chase &&
            !player.isHiding &&
            col.collider.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().NotifyPlayerDead();
        }
    }

    void GoToPoint(int idx)
    {
        agent.SetDestination(patrolPoints[idx].position);
    }
}
