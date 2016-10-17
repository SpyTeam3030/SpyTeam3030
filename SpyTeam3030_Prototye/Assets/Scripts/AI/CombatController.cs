using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour {


    [Header("Combat Attributes")]
    public float maxhealth;
    public float attackPower;
    public float attackRadius;
    public float attackSpeed;

    private List<CombatController> attackTargets;
    SpyController mSpyController;
    private float counter;
    private float health;


    // Use this for initialization
    void Start () {
        GetComponent<SphereCollider>().radius = attackRadius;
        mSpyController = GetComponent<SpyController>();
        counter = 0.0f;
        health = maxhealth;
        attackTargets = new List<CombatController>();
	}
	
	// Update is called once per frame
	void Update () {
        if(attackTargets.Count != 0)
        {
            if(counter < attackSpeed)
            {
                counter += Time.deltaTime;
            }
            else
            {
                counter = 0.0f;
                attackTargets[0].TakeDamge(attackPower);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CombatObject")
        {
            attackTargets.Add(other.gameObject.GetComponent<CombatController>());
            if(attackTargets.Count == 1)
            {
                mSpyController.onCombat();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "CombatObject")
        {
            attackTargets.Remove(other.gameObject.GetComponent<CombatController>());
            if(attackTargets.Count == 0)
            {
                mSpyController.leaveCombat();
            }
        }
    }

    public void TakeDamge(float power)
    {
        health -= power;
        if(health <= 0.0f)
        {
            health = maxhealth;
            attackTargets.Clear();
            counter = 0.0f;
            mSpyController.Respawn();
        }
    }
}
