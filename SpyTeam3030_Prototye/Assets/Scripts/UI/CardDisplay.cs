using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDisplay : MonoBehaviour {
	public CardStatus[] cards;
	public GameplayServer gs;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		List<GameObject> spys = gs.alltheSpys;
		if (spys.Count == 0) {
			return;
		}
		for(int i = 0; i < 6; i++){
			CombatController cc = spys[i].GetComponent<CombatController>();
			cards [i].SetStats (cc.attackPower, cc.health, cc.attackSpeed);//plug in character's stats
		}
	}


}
