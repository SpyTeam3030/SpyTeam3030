using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct Card{
	public int id;
	public string description;

	public float attack;
	public float health;
	public float speed;
	public float attackSpeed;
	public float attackDistance;

	public Sprite image;
}

public class CardManager : MonoBehaviour {

	public Sprite[] images;

	private Stack<Card> cards;//contains everything needed for a card
	private List<int> ids;//ids from 1-30

	// Use this for initialization
	void Awake () {
		cards = new Stack<Card> ();
		ids = new List<int> ();
		for (int i = 0; i < 30; i++) {
			ids.Add(i + 1);
		}
		ids = Shuffle (ids);

		string excelPath = Application.dataPath + "/Excel/cards.xlsx";
		Excel xls = ExcelHelper.LoadExcel(excelPath);

		xls.ShowLog();


		for (int i = 0; i < ids.Count; i++){
//			Debug.Log (ids [i]);

			Card c = new Card();
			int ID = ids [i];

			c.id = ID;
			c.description = xls.Tables [0].GetValue (ID, 3).ToString();

			c.health = Convert.ToSingle(xls.Tables [0].GetValue (ID, 4));
			c.speed = Convert.ToSingle(xls.Tables [0].GetValue (ID, 5));
			c.attack = Convert.ToSingle(xls.Tables [0].GetValue (ID, 6));

			c.attackSpeed = 25f;
			c.attackDistance = 25f;
			c.image = images [ID-1];

			cards.Push (c);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextCard(GameObject go){
		go.GetComponent<DragHandeler> ().Init (cards.Pop ());
	}

	public Card ReadTop(){
		Card c = new Card();
		if (cards.Count == 0) {
			c.id = 0;
		}
		else{
			c = cards.Peek ();
		}
		return c;
	}

	public static List<int> Shuffle (List<int>aList) {

		System.Random _random = new System.Random ();

		int myGO;

		int n = aList.Count;
		for (int i = 0; i < n; i++)
		{
			int r = i + (int)(_random.NextDouble() * (n - i));
			myGO = aList[r];
			aList[r] = aList[i];
			aList[i] = myGO;
		}

		return aList;
	}
}
