using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    //monster states
    public enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    [Header("Patrol Settings")]
    [SerializeField] float waitTimeOnWaypoint = 1.0f;
    [SerializeField] MonsterPath path;

    [Header("Chase Settings")]
    [SerializeField] float detectionRange = 10.0f; 
    [SerializeField] float chaseSpeedMultiplier = 1.5f; 
    [SerializeField] LayerMask playerLayer; 

    NavMeshAgent agent;
    Animator animator;
    float patrolWaitTime = 0.1f;
    Transform playerTarget;
    float baseSpeed;

    float time = 0.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        baseSpeed = agent.speed;

        // Find the player object by tag
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }
    void Start()
    {
        
            agent.destination = path.GetCurrentWayPoint();
           
    }

    void Update()
    {
        // update animation by speed 
        animator.SetFloat("speed", agent.velocity.magnitude);

        // check if player is in detection range
        if (playerTarget != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        

            if (distanceToPlayer <= detectionRange)
            {
                // if player is within detection range: update state to Chase
                currentState = State.Chase;
            }
            else if (currentState == State.Chase)
            {
                // if player is out of range and currently chasing, switch back to Patrol
                currentState = State.Patrol;

                // reset speed to base speed
                agent.speed = baseSpeed;

                // agent heads to the next waypoint
                agent.destination = path.GetNextWaypoint();
                
            }
        }

        // State Machine
        if (currentState == State.Patrol)
        {
            PatrolState();
        }
        else if (currentState == State.Chase)
        {
            ChaseState();
        }

    }

        void PatrolState() { 
            if (agent.remainingDistance <= 0.1f)
            {
                time += Time.deltaTime;
                if (time >= waitTimeOnWaypoint)
                {
                    time = 0.0f;
                    agent.destination = path.GetNextWaypoint();
                }
            }
        }

    void ChaseState()
    {
        // increase speed for chasing
        agent.speed = baseSpeed * chaseSpeedMultiplier;

        // set destination to player position
        agent.destination = playerTarget.position;
    }
}
