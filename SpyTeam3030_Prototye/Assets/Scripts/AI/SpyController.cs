using UnityEngine;
using System.Collections;

public class SpyController : MonoBehaviour {


    [Header("Spy Attribute")]
    public float maxMovementSpeed;
    public float moveAcceleration;

    private Vector3 spawnPosition = Vector3.zero;
    private Vector3 enemyBase = Vector3.zero;
    private NavMeshAgent agent;
    private bool combat;

    public void InitilizeSpy(Vector3 spawnpos, Vector3 basepos)
    {
        spawnPosition = spawnpos;
        enemyBase = basepos;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = maxMovementSpeed;
        agent.acceleration = moveAcceleration;
    }

    void OnMouseDown()
    {
        Debug.Log(enemyBase);
    }

    // Update is called once per frame
    void Update()
    {
        if(combat)
        {
            return;
        }
        else if(enemyBase != Vector3.zero)
        {
            agent.SetDestination(enemyBase);
        }
    }

    public void onCombat()
    {
        agent.Stop();
        combat = true;
    }

    public void leaveCombat()
    {
        agent.Resume();
        combat = false;
    }
    
    public void Respawn()
    {
        GetComponent<Transform>().position = spawnPosition;
        agent.Resume();
        combat = false;
    }
}
