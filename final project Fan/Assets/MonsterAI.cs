using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] patrolPointsA;
    public Transform[] patrolPointsB;
    
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

        // 初次随机选择巡逻路线
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
                // 触发追逐的判定保持不变
                if (currentIndex == chaseTriggerIndex)
                {
                    state = State.Chase;
                    if (chaseClip != null)
                        audioSource.PlayOneShot(chaseClip);
                    return;
                }

                
                if (patrolClips != null &&
                    currentIndex < patrolClips.Length &&
                    patrolClips[currentIndex] != null)
                {
                    audioSource.PlayOneShot(patrolClips[currentIndex]);
                }
                // —— 播放完毕，下面再切到下一个巡逻点 —— 

                // 去下一个巡逻点
                currentIndex = (currentIndex + 1) % patrolPoints.Length;
                GoToPoint(currentIndex);
                waitTimer = Random.Range(patrolWaitMin, patrolWaitMax);

         
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
            agent.ResetPath();
        }
    }

    void GiveUpUpdate()
    {
        giveUpTimer -= Time.deltaTime;
        if (giveUpTimer <= 0f)
        {
            // 切回巡逻状态
            state = State.Patrol;
            agent.speed = patrolSpeed;
            agent.ResetPath();

            // —— 关键：每次放弃追逐后重新随机路线 —— 
            patrolPoints = (Random.value < 0.5f ? patrolPointsA : patrolPointsB);
            if (patrolPoints.Length == 0)
            {
                Debug.LogError("MonsterAI: 请在 Inspector 填入巡逻点数组！");
                enabled = false;
                return;
            }

            currentIndex = 0;
            GoToPoint(currentIndex);
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