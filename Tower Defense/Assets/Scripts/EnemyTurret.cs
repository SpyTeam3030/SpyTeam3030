using UnityEngine;
using System.Collections;

public class EnemyTurret : MonoBehaviour {

	public int health = 500;
	public int attack = 100;
	public static bool dead = false;
	public bool attacking = false;

	private Color defaultColor;

	public static Vector3 turretPosition;

	// Use this for initialization
	void Start () {
		defaultColor = GetComponent<Renderer> ().material.color;
		DefaultColor ();

		turretPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
			GetComponent<MeshRenderer>().enabled = false;
			return;
		}else{
			if ((turretPosition - MySpy.position).magnitude < 1.2f) {
				StartAttacking ();
			}
		}
	}

	public void TakeDamage(int damage){
		if (!dead) {
			health -= damage;
			print ("Taken Damage");

			GetComponent<Renderer> ().material.color = Color.red;
			Invoke ("DefaultColor", 0.5f);

			if (health <= 0) {
				dead = true;
				attacking = false;
			}
		}
	}

	public void DefaultColor(){
		GetComponent<Renderer> ().material.color = defaultColor;
	}

	void StartAttacking (){
		if (!MySpy.dead) {
			GameObject.Find("MySpy").GetComponent<MySpy>().TakeDamage (attack);
		}
		if (!MySpy.dead){
			Invoke ("StartAttacking", 4);
		}
	}
}
