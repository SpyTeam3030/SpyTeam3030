using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public float speed = 10f;

	private Transform target;
	private bool dead = false;

	// Use this for initialization
	void Start () {
		target = Turret.turretWaypoint;
	}
	
	// Update is called once per frame
	void Update () {
		if(!dead){
			Vector3 dir = target.position - transform.position;
			dir.y = 0;
			if(dir.magnitude > .2){
				transform.Translate (dir.normalized * speed * Time.deltaTime);
			}
			else{
				dead = true;
			}
		}
	}
}
