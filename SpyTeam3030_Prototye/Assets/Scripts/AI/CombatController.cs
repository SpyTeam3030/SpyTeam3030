using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CombatController : NetworkBehaviour
{


    [Header("Combat Attributes")]
    [SyncVar]
    public float maxhealth;
    [SyncVar]
    public float attackPower;
    [SyncVar]
    public float attackRadius;
    [SyncVar]
    public float attackSpeed;

    [Header("Combat Display")]
    public GameObject healthBar;
    public Transform popUpPos;
    protected Vector3 fullHealth;
    protected Vector3 emptyHealth;

    protected List<CombatController> attackTargets;
    protected SpyController mSpyController;
    protected float counter;
    protected float health;
    protected int id;


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
        if (!isServer)
            return;
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
        if (!isServer)
            return;
        
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
        if (!isServer)
            return;
        
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

    public virtual void TakeDamge(float power)
    {
        if (!isServer)
            return;
        
        health -= power;
        RpcDisplayPopup(power.ToString(), popUpPos.position);
        if (health <= 0.0f)
        {
            health = maxhealth;
            attackTargets.Clear();
            counter = 0.0f;
            mSpyController.Respawn();
            healthBar.transform.localScale = fullHealth;
        }
        RpcUpdateHealthBar(health / maxhealth);
    }

    public virtual bool IsSameTeam(int otherID)
    {
        return id == otherID;
    }

    public virtual void AttributeChange(float maxHealthChange = 0.0f, float powerChange = 0.0f, float radiusChange = 0.0f, float speedChange = 0.0f)
    {
        maxhealth += maxHealthChange;
        attackPower += powerChange;
        GetComponent<SphereCollider>().radius = attackRadius = radiusChange;
        attackSpeed = speedChange;
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
