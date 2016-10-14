using UnityEngine;
using System.Collections;

public class SpyController : MonoBehaviour {


    [Header("Spy Attribute")]
    public float health;
    public float attackPower;
    public float attackRadius;
    public float attackFrequency;
    public float maxMovementSpeed;
    public float moveAcceleration;

    private Vector3 spawnPosition = Vector3.zero;
    private Vector3 enemyBase = Vector3.zero;
    private NavMeshAgent agent;

    public void InitilizeSpy(Vector3 spawnpos, Vector3 basepos)
    {
        spawnPosition = spawnpos;
        enemyBase = basepos;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.Stop();
    }

    void OnMouseDown()
    {
        Debug.Log(enemyBase);
    }

    //// Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    void Update()
    {
        if(enemyBase != Vector3.zero)
        {
            agent.SetDestination(enemyBase);
        }
    }
}
