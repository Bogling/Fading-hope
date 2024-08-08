using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FearAI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask playerLayer;

    //Patrol
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] private float walkDistance;

    //States
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    private bool playerInSightRange, playerInAttackRange;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private AudioSource ambient;
    [SerializeField] private AudioClip chaseClip;
    [SerializeField] private AudioClip labClip;
    private GameManager gameManager;

    private bool isWaiting = false;

    private void Awake() {
        player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) {
            animator.SetBool("IsChasing", false);
            Patroling();
            if (ambient.clip == chaseClip) {
                ambient.clip = labClip;
            }
            if (!ambient.isPlaying) {
                ambient.Play();
            }
        }
        else if (playerInSightRange && !playerInAttackRange) {
            if (ambient.clip != chaseClip) {
                ambient.clip = chaseClip;
            }
            if (!ambient.isPlaying) {
                ambient.Play();
            }
            
            ChasePlayer();
        }
        else {
            AttackPlayer();
        }
    }

    private async void Patroling() {
        if (!walkPointSet && !isWaiting) { 
            animator.SetBool("IsPatroling", false);
            isWaiting = true;
            await Task.Delay(5000);
            isWaiting = false;
            SearchWalkPoint();
        }

        if (walkPointSet) {
            animator.SetBool("IsPatroling", true);
            agent.speed = patrolSpeed;
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkDistance, walkDistance);
        float randomX = Random.Range(-walkDistance, walkDistance);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshPath navMeshPath= new NavMeshPath();
        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && agent.CalculatePath(walkPoint, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete) {
            walkPointSet = true;
        }
    }
    
    private void ChasePlayer() {
        animator.SetBool("IsChasing", true);
        animator.SetBool("IsPatroling", false);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer() {
        gameManager.Respawn(Color.black, 1f, true, true);
    }
}
