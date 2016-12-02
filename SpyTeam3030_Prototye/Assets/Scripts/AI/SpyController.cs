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
    private bool isServer = false;
    private int teamID;
    private Vector3 hidePos = Vector3.one * -100;

    public void InitilizeSpy(Vector3 spawnpos, Vector3 basepos, int id)
    {
        spawnPosition = spawnpos;
        enemyBase = basepos;
        teamID = id;
        isServer = true;
        Debug.Log("spy init");
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
        if (!isServer)
            return;
        
        if(combat)
        {
            return;
        }
        else if(enemyBase != Vector3.zero && agent.enabled)
        {
            agent.SetDestination(enemyBase);
        }
    }

    public void onCombat()
    {
        Debug.Log("spy 1");
        agent.Stop();
        agent.velocity *= 0.2f;
        combat = true;
    }

    public void leaveCombat()
    {
        Debug.Log("spy 2");
        if (agent.enabled)
        {
            agent.Resume();
            combat = false;   
        }
    }
    
    public void Respawn()
    {

        Debug.Log("spy 3");
        GetComponent<Transform>().position = spawnPosition;  
        leaveCombat();
        //StartCoroutine(WaitForRespawn());
    }
        

    IEnumerator WaitForRespawn()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(3.0f);
        GetComponent<Transform>().position = spawnPosition;
        agent.enabled = true;
        agent.Resume();
        combat = false;
    }

    public int GetTeamID()
    {
        return teamID;
    }
}
