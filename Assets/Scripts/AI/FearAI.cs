using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FearAI : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask playerLayer;

    //Patrol
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    bool isPatroling;
    [SerializeField] private float walkDistance;

    //States
    [SerializeField] private bool activeAtStart = true;
    private bool isActive;
    [SerializeField] private bool useAlternateAttack;
    [SerializeField] private float alternateAttackDamage;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    private bool playerInSightRange, playerInAttackRange;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private AudioSource ambient;
    [SerializeField] private AudioClip chaseClip;
    [SerializeField] private AudioClip labClip;
    [SerializeField] private GameObject signalDestination;
    
    private GameManager gameManager;

    private bool isWaiting = false;
    private bool isDead = false;

    private void Awake() {
        player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Start() {
        if (activeAtStart) {
            isActive = true;
            gameObject.SetActive(true);
        }
        else {
            isActive = false;
            gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (!isActive) return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) {
            animator.SetBool("IsChasing", false);
            StartCoroutine(Patroling());
            if (ambient.clip == chaseClip && labClip != null) {
                ambient.clip = labClip;
            }
            if (!ambient.isPlaying) {
                ambient.Play();
            }
        }
        else if (playerInSightRange && !playerInAttackRange) {
            if (ambient.clip != chaseClip && chaseClip != null) {
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

    private IEnumerator Patroling() {
        if (isPatroling) yield break;
        isPatroling = true;
        if (!walkPointSet && !isWaiting) { 
            animator.SetBool("IsPatroling", false);
            isWaiting = true;
            yield return new WaitForSeconds(5);
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
        isPatroling = false;
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkDistance, walkDistance);
        float randomX = Random.Range(-walkDistance, walkDistance);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshPath navMeshPath= new NavMeshPath();
        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && agent.CalculatePath(walkPoint, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete) {
            Debug.Log(Physics.Raycast(walkPoint, -transform.up, 2f, ground));
            Debug.Log(agent.CalculatePath(walkPoint, navMeshPath));
            Debug.Log(navMeshPath.status == NavMeshPathStatus.PathComplete);
            walkPointSet = true;
        }
        Debug.Log(Physics.Raycast(walkPoint, -transform.up, 2f, ground));
        Debug.Log(agent.CalculatePath(walkPoint, navMeshPath));
        Debug.Log(navMeshPath.status == NavMeshPathStatus.PathComplete);
    }
    
    private void ChasePlayer() {
        animator.SetBool("IsChasing", true);
        animator.SetBool("IsPatroling", false);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer() {
        if (useAlternateAttack) {
            gameManager.DealDamage(alternateAttackDamage);
            FindFirstObjectByType<Fader>().AutoFade(Color.black, 1f, 0.5f, 3f);
            gameObject.SetActive(false);
            isActive = false;
        }
        animator.SetTrigger("Attack");
        //gameManager.Respawn(Color.black, 1f, true, true);
    }

    public void KillPlayer() {
        gameManager.Respawn(Color.black, 1f, true, true);
    }

    public void DealDamage(float damage)
    {
        Die();
    }

    public void Die() {
        isDead = true;
        animator.SetTrigger("Die");
        if (signalDestination != null) {
            signalDestination.GetComponent<Interactable>().Interact();
        }
    }

    public bool IsDead() {
        return isDead;
    }

    public void SetDestination(GameObject dest) {
        signalDestination = dest;
    }

    public void Wake(bool showAnimation) {
        gameObject.SetActive(true);
        if (showAnimation) {
            animator.SetTrigger("Activate");
        }
        else {
            Activate();
        }
    }

    public void Disappear() {
        Deactivate();
        gameObject.SetActive(false);
    }

    public void Activate() {
        isActive = true;
    }

    public void Deactivate() {
        isActive = false;
    }
}
