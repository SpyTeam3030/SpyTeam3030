using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class CombatController : NetworkBehaviour
{


    [Header("Combat Attributes")]
	[SyncVar]
	public float originalHealth;
    [SyncVar]
    public float maxhealth;
    [SyncVar]
    public float attackPower;
    [SyncVar]
    public float attackRadius;
    [SyncVar]
    public float attackSpeed;
	[SyncVar]
	public bool card;

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
		maxhealth = originalHealth;
		health = maxhealth;
		card = false;
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
			GetComponent<Animator> ().SetBool ("Attack", true);
            if(counter < attackSpeed)
            {
                counter += Time.deltaTime;
            }
            else
            {
                counter = 0.0f;
                attackTargets[0].TakeDamge(attackPower);
            }
        } else {
			GetComponent<Animator> ().SetBool ("Attack", false);
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
			GetComponent<Animator> ().SetTrigger ("Dead");
			card = false;
			maxhealth = originalHealth;
			health = maxhealth;
			GetComponent<NavMeshAgent> ().speed = mSpyController.maxMovementSpeed;
			attackPower = 10f;
			attackRadius = 4.5f;
			attackSpeed = 1f;

			attackTargets.Clear();
			counter = 0.0f;
			healthBar.transform.localScale = fullHealth;

			Invoke ("RpcDie", 2.1f);
		}
        RpcUpdateHealthBar(health / maxhealth);
    }

    public virtual bool IsSameTeam(int otherID)
    {
        return id == otherID;
    }

	[ClientRpc]
	public void RpcDie(){
		mSpyController.Respawn();

	}

	public virtual bool AttributeChange(float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f)
	{
		if (maxHealthChange > 900) {
			health = maxhealth;
			return true;
		}

		if (card) {
			return false;
		}

		card = true;

		maxhealth += maxHealthChange;
		health += maxHealthChange;
		attackPower += attackChange;
		if (newSpeed != 0) {
			GetComponent<NavMeshAgent> ().speed = newSpeed * mSpyController.maxMovementSpeed;
		}
		if (newRadius != 0) {
			GetComponent<SphereCollider>().radius = attackRadius = newRadius;
		}
		if (newAttackSpeed != 0) {
			attackSpeed = newAttackSpeed;
		}
		return true;
	}


    [ClientRpc]
    void RpcDisplayPopup(string value, Vector3 location)
    {
//        Debug.Log("pop up");
        PopupController.DisplayPopup(value, location);
    }

    [ClientRpc]
    void RpcUpdateHealthBar(float ratio)
    {
        healthBar.transform.localScale = Vector3.Lerp(emptyHealth, fullHealth, ratio);
    }
}
