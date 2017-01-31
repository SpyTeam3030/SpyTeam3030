using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDisplay : MonoBehaviour {
	public CharacterIcon[] icons;
	public GameplayServer gs;
	public List<GameObject> spys;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSpys(){
		spys = gs.alltheSpys;
		if (spys.Count == 0) {
			return;
		}
		for(int i = 0; i < 6; i++){
			icons [i].SetSpy (spys [i].GetComponent<CombatController>());
		}
	}


}
