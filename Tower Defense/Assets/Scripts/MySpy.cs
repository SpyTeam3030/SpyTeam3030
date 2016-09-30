using UnityEngine;
using System.Collections;

public class MySpy : MonoBehaviour {

	public float speed = 10f;

	public GameObject target;
	public Vector3 targetPosition;
	public static Vector3 position;
	private Vector3 startPosition;
	private NavMeshAgent agent;

	private Color defaultColor;

	public float dist;

	public int defaultHealth = 200;
	public int currentHealth;
	public int defaultAttack = 50;
	public int currentAttack;
	public float defaultAttackSpeed = 2.0f;
	public float currentAttackSpeed;

	public static bool dead = false;
	private bool attacking = false;

	// Use this for initialization
	void Start () {
		defaultColor = GetComponent<Renderer> ().material.color;
		DefaultColor ();

		currentHealth = defaultHealth;
		currentAttack = defaultAttack;
		currentAttackSpeed = defaultAttackSpeed;

		startPosition = transform.position;
		target = GameObject.Find("EnemyTurret1");
		targetPosition = EnemyTurret.turretPosition;
		agent = GetComponent<NavMeshAgent> ();
	}

	// Update is called once per frame
	void Update () {
		position = transform.position;
		if (dead && GetComponent<MeshRenderer> ().enabled) {
			//play dead animation
			GetComponent<MeshRenderer>().enabled = false;
			transform.position = startPosition;
			agent.Stop ();
			Invoke ("Respawn", 5);
		} else if (!dead && !EnemyTurret.dead && !MyTurret.dead) {
			if (attacking) {
				return;
			}
			else if (!attacking) {
				targetPosition = GetNextTarget ();
				agent.Resume ();
				agent.SetDestination (targetPosition);
			}
			dist = (targetPosition - position).magnitude;
			if (dist <= 1.2f) {
				attacking = true;
				agent.Stop ();
				//ATTACK SPY/TURRET
				if (target) {
					StartAttacking ();
				}
			}

		}
	}

	void OnCollisionEnter(Collision collision) {
		print ("collision happened");
		if (collision.gameObject.name == "EnemySpy") {
			print ("collision happened");
			attacking = true;
			agent.Stop ();
			collision.gameObject.GetComponent<EnemySpy>().TakeDamage (currentAttack);
		}
	}

	public void DefaultColor(){
		GetComponent<Renderer> ().material.color = defaultColor;
	}

	public void TakeDamage(int damage){
		if (!dead) {
			currentHealth -= damage;
			print ("took " + damage + " damage");

			GetComponent<Renderer> ().material.color = Color.red;
			Invoke ("DefaultColor", 0.5f);

			if (currentHealth <= 0) {
				dead = true;
				attacking = false;
			}
		}
	}

	public void StartAttacking(){
		GameObject go;
		if (target.tag == "EnemySpy") {
			go = GameObject.Find ("EnemySpy");
			go.GetComponent<EnemySpy>().TakeDamage (currentAttack);
			if (EnemySpy.dead) {
				attacking = false;
			}else{
				Invoke ("StartAttacking", currentAttackSpeed);
			}
		}
		else if (target.tag == "EnemyTurret") {
			go = GameObject.Find ("EnemyTurret1");
			go.GetComponent<EnemyTurret>().TakeDamage (currentAttack);
			if (EnemyTurret.dead) {
				attacking = false;
			}else{
				Invoke ("StartAttacking", currentAttackSpeed);
			}
		}
	}

	public void Respawn(){
		dead = false;
		attacking = false;

		currentHealth = defaultHealth;
		currentAttack = defaultAttack;
		currentAttackSpeed = defaultAttackSpeed;

		GetComponent<MeshRenderer>().enabled = true;
		agent.Resume ();
	}

	private Vector3 GetNextTarget(){
		Vector3 result;
		Vector3 pos = EnemySpy.position;
		if (pos.z > position.z && pos.z < EnemyTurret.turretPosition.z) {
			target = GameObject.Find("EnemySpy");
			result = pos;
		}else{
			target = GameObject.Find("EnemyTurret1");
			result = EnemyTurret.turretPosition;
		}
		return result;
	}
}
