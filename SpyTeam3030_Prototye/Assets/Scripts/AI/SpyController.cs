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

    private Vector3 spawnPosition;
    private Vector3 enemyBase;

    public void InitilizeSpy(Vector3 spawnpos, Vector3 basepos)
    {
        spawnPosition = spawnpos;
        enemyBase = basepos;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
