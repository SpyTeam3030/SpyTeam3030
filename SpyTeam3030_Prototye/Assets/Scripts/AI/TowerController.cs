using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TowerController : CombatController
{
    public void InitiID(int id)
    {
        this.id = id;
    }

    // Use this for initialization
    void Start()
    {
        GetComponent<SphereCollider>().radius = attackRadius;
        counter = 0.0f;
        health = maxhealth;
        attackTargets = new List<CombatController>();
        fullHealth = healthBar.transform.localScale;
        emptyHealth = fullHealth;
        emptyHealth.x = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            return;
        if (attackTargets.Count != 0)
        {
            if (counter < attackSpeed)
            {
                counter += Time.deltaTime;
            }
            else
            {
                counter = 0.0f;
                DrawLine(transform.position, attackTargets[0].transform.position, Color.green);
                attackTargets[0].TakeDamge(attackPower);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer)
            return;
        if (other.gameObject.tag == "CombatObject")
        {
            if (!other.gameObject.GetComponent<CombatController>().IsSameTeam(id))
            {
                attackTargets.Add(other.gameObject.GetComponent<CombatController>());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isServer)
            return;
        if (other.gameObject.tag == "CombatObject")
        {
            if (!other.gameObject.GetComponent<CombatController>().IsSameTeam(id))
            {
                attackTargets.Remove(other.gameObject.GetComponent<CombatController>());
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public override void TakeDamge(float power)
    {
        if (!isServer)
            return;
        health -= power;
        RpcDisplayPopup(power.ToString(), popUpPos.position);
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
        RpcUpdateHealthBar(health / maxhealth);
    }

    public override bool IsSameTeam(int otherID)
    {
        return id == otherID;
    }

	public virtual void AttributeChange(float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f)
	{
		maxhealth += maxHealthChange;
		attackPower += attackChange;
		if (newSpeed != 0) {
			GetComponent<NavMeshAgent> ().speed = 3.5f * newSpeed;
		}
		if (newRadius != 0) {
			GetComponent<SphereCollider>().radius = attackRadius = newRadius;
		}
		if (newAttackSpeed != 0) {
			attackSpeed = newAttackSpeed;
		}
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
