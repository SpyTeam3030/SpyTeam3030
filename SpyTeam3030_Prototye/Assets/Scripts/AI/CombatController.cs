using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
	[SyncVar]
	public int cardID;
    [SyncVar]
    public float restoreHealth;
    [SyncVar]
    public bool changed;

	public string m_name;

    [Header("Combat Display")] 
    public GameObject healthBar;
    public Transform popUpPos;
    protected Vector3 fullHealth;
    protected Vector3 emptyHealth;

    protected List<CombatController> attackTargets;
    protected SpyController mSpyController;
    protected float counter;
    public float health;
    protected int id;
    protected int m_id;
	protected bool flash = false;


    // Use this for initialization
    void Start () 
    {
        GetComponent<SphereCollider>().radius = attackRadius;
        mSpyController = GetComponent<SpyController>();
        id = mSpyController.GetTeamID();
        m_id = mSpyController.GetMyID();
        counter = 0.0f;
		maxhealth = originalHealth;
		health = maxhealth;
		card = false;
		cardID = 0;
        attackTargets = new List<CombatController>();
        fullHealth = healthBar.transform.localScale;
        emptyHealth = fullHealth;
        emptyHealth.x = 0.0f;
        restoreHealth = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
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
				if(flash)
				{
					flash = false;
					GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = mSpyController.maxMovementSpeed;
				}
                if (attackTargets[0].TakeDamge(attackPower))
                {
                    attackTargets.RemoveAt(0);                
                }
            }
        } 
        else 
        {
			GetComponent<Animator> ().SetBool ("Attack", false);
		}

        health += restoreHealth;
        health = Mathf.Min(health, maxhealth);
	}

	public int getID(){
		return id;
	}

    public int getMyID()
    {
        return m_id;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer)
            return;

        //TODO: restore the speed to original one if the card is gripping gun
        
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

    public virtual bool TakeDamge(float power)
    {
        if (!isServer)
            return false;

        health -= power;
        RpcDisplayPopup(power.ToString(), popUpPos.position, Color.red);
        if (health <= 0.0f)
        {
			RpcDie ();

			card = false;
			cardID = 0;
			maxhealth = originalHealth;
			health = maxhealth;
            restoreHealth = 0.0f;
			GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = mSpyController.maxMovementSpeed;
			attackPower = 10f;
			attackRadius = 4.5f;
			attackSpeed = 1f;

			attackTargets.Clear();
			counter = 0.0f;
			healthBar.transform.localScale = fullHealth;

			mSpyController.Respawn();
		}
        RpcUpdateHealthBar(health / maxhealth);
        return false;
    }

    public virtual bool TakeHealth(float power = -1.0f)
    {
        if (!isServer)
            return false;

        if (power == -1.0f)
        {
            health = maxhealth;
        }
        else
        {
            health += power;
            health = Mathf.Min(health, maxhealth);
        }

        RpcDisplayPopup(power.ToString(), popUpPos.position, Color.green);
        RpcUpdateHealthBar(health / maxhealth);
        return false;
    }

    public virtual bool IsSameTeam(int otherID)
    {
        return id == otherID;
    }

    public virtual bool isEqualByID(int otherID)
    {
        return m_id == otherID;
    }

	[ClientRpc]
	public void RpcDie(){
		mSpyController.mesh.SetActive (false);
	}

	[ClientRpc]
	public void RpcRespawn(){
		mSpyController.mesh.SetActive (true);
	}

    public virtual bool AttributeChange(int cardID, float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f, float rHealthChange = 0.0f)
	{
            
        Debug.Log("attribute change");
		if (maxHealthChange > 900) 
        {
			health = maxhealth;
			return true;
		}

		if (this.cardID != 0) {
			return false;
		}

		if (card) 
        {
			return false;
		}

        Debug.Log("attribute change doing real job");
        Debug.Log(maxHealthChange.ToString() + ":" + attackChange.ToString() + ":" + newSpeed.ToString() + ":" + newRadius.ToString() + ":" + newAttackSpeed.ToString() + ":" + rHealthChange.ToString());
		card = true;
		this.cardID = cardID;

		maxhealth += maxHealthChange;
		health += maxHealthChange;
		attackPower += attackChange;
        restoreHealth = rHealthChange;
        if (newSpeed != 0) 
        {
			GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = newSpeed * mSpyController.maxMovementSpeed;
		}
		if (newRadius != 0) 
        {
			GetComponent<SphereCollider>().radius = attackRadius = newRadius;
		}
		if (newAttackSpeed != 0) 
        {
			attackSpeed = newAttackSpeed;
		}

		healthBar.transform.localScale = Vector3.Lerp(emptyHealth, fullHealth, health / maxhealth);

		return true;
	}

	public virtual void Flash(float newSpeedFactor)
	{
		GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = newSpeedFactor * mSpyController.maxMovementSpeed;
		flash = true;
	}


    [ClientRpc]
    void RpcDisplayPopup(string value, Vector3 location, Color c)
    {
//        Debug.Log("pop up");
        PopupController.DisplayPopup(value, location, c);
    }

    [ClientRpc]
    void RpcUpdateHealthBar(float ratio)
    {
        healthBar.transform.localScale = Vector3.Lerp(emptyHealth, fullHealth, ratio);
    }
}
