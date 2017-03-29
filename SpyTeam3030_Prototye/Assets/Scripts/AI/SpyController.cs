using UnityEngine;
using System.Collections;

public class SpyController : MonoBehaviour {


    [Header("Spy Attribute")]
    public float maxMovementSpeed;
    public float moveAcceleration;

	public GameObject mesh;

    private Vector3 spawnPosition = Vector3.zero;
    private Vector3 enemyBase = Vector3.zero;
    private UnityEngine.AI.NavMeshAgent agent;
    private bool combat;
    private bool isServer = false;
    private int teamID;
    private Vector3 hidePos = Vector3.one * -100;
    private int m_spyID;

    public void InitilizeSpy(Vector3 spawnpos, Vector3 basepos, int tid, int sid)
    {
        spawnPosition = spawnpos;
        enemyBase = basepos;
        teamID = tid;
        m_spyID = sid;
        isServer = true;
        Debug.Log("spy init");
    }

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
        agent.Stop();
        agent.velocity *= 0.2f;
        combat = true;
    }

    public void leaveCombat()
    {
        if (agent.enabled)
        {
            agent.Resume();
            combat = false;   
        }
    }
    
    public void Respawn()
    {
        GetComponent<Transform>().position = spawnPosition;  
        leaveCombat();
        StartCoroutine(WaitForRespawn());
    }

    IEnumerator WaitForRespawn()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(10.0f);
        GetComponent<Transform>().position = spawnPosition;

		GetComponent<CombatController> ().RpcRespawn ();
        agent.enabled = true;
        agent.Resume();
        combat = false;
    }

    public int GetTeamID()
    {
        return teamID;
    }

    public int GetMyID()
    {
        return m_spyID;
    }
}
