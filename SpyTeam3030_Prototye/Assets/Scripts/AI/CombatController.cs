using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CombatController : NetworkBehaviour
{


    [Header("Combat Attributes")]
    public float maxhealth;
    public float attackPower;
    public float attackRadius;
    public float attackSpeed;

    [Header("Combat Display")]
    public GameObject healthBar;
    public Transform popUpPos;
    private Vector3 fullHealth;
    private Vector3 emptyHealth;

    private List<CombatController> attackTargets;
    SpyController mSpyController;
    private float counter;
    private float health;
    private int id;


    // Use this for initialization
    void Start () {
        GetComponent<SphereCollider>().radius = attackRadius;
        mSpyController = GetComponent<SpyController>();
        id = mSpyController.GetTeamID();
        counter = 0.0f;
        health = maxhealth;
        attackTargets = new List<CombatController>();
        fullHealth = healthBar.transform.localScale;
        emptyHealth = fullHealth;
        emptyHealth.x = 0.0f;
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
            if (!other.gameObject.GetComponent<CombatController>().IsSameTeam(id))
            {
                Debug.Log("Adding Enemy\n");
                attackTargets.Add(other.gameObject.GetComponent<CombatController>());
                if (attackTargets.Count == 1)
                {
                    mSpyController.onCombat();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "CombatObject")
        {
            if (!other.gameObject.GetComponent<CombatController>().IsSameTeam(id))
            {
                attackTargets.Remove(other.gameObject.GetComponent<CombatController>());
                if (attackTargets.Count == 0)
                {
                    mSpyController.leaveCombat();
                }
            }
        }
    }

    public void TakeDamge(float power)
    {
        health -= power;
        RpcDisplayPopup(power.ToString(), popUpPos.position);
        if(health <= 0.0f)
        {
            health = maxhealth;
            attackTargets.Clear();
            counter = 0.0f;
            mSpyController.Respawn();
            healthBar.transform.localScale = fullHealth;
        }
        RpcUpdateHealthBar(health / maxhealth);
    }

    public bool IsSameTeam(int otherID)
    {
        return id == otherID;
    }

    [ClientRpc]
    void RpcDisplayPopup(string value, Vector3 location)
    {
        Debug.Log("pop up");
        PopupController.DisplayPopup(value, location);
    }

    [ClientRpc]
    void RpcUpdateHealthBar(float ratio)
    {
        healthBar.transform.localScale = Vector3.Lerp(emptyHealth, fullHealth, ratio);
    }
}
