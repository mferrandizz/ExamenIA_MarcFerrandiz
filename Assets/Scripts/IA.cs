using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Travelling,
        Attacking
    }

    [SerializeField] public State currentState;

    NavMeshAgent agent;

    public Transform[] destinationPoints;
    public int destinationIndex = 0;
    public Transform player;
    [SerializeField] float visionRange;
    [SerializeField] float attackRange;


    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Start()
    {
        currentState = State.Patrolling;
    }

    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Travelling:
                Travel();
            break;
            case State.Chasing:
                Chasing();
            break;
            case State.Attacking:
                Attacking();
            break;
            default:
                Patrol();
            break;
        }
    }


    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, player.position ) < visionRange )
        {
            currentState = State.Chasing;
        }

        currentState = State.Travelling;

    }

    void Travel()
    {
        if(agent.remainingDistance <= 0.5f)
        {
            destinationIndex++;
            currentState = State.Patrolling;

            if(destinationIndex == 5)
            {
                destinationIndex = 0;
            }

        }

        currentState = State.Patrolling;

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }


    }

    void Chasing()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }

        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = State.Attacking;
        }
    }

    void Attacking()
    {
        Debug.Log("Te meto una paliza");

        currentState = State.Chasing;

    }
    
void OnDrawGizmos() 
    {
        foreach(Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}


