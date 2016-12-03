﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class JsonHelper
{
	public static T[] getJsonArray<T>(string json)
	{
		string newJson = "{ \"array\": " + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
		return wrapper.array;
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] array;
	}
}

[System.Serializable]
public struct Card{
	public int id;
	public string type;
	public string description;

	public float health;
	public float speed;
	public float attack;

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
		for (int i = 0; i < 32; i++) {
			ids.Add(i + 1);
		}
		ids = Shuffle (ids);

		string jsonString = File.ReadAllText (Application.dataPath + "/Cards.json");
		Card[] cardDeck = JsonHelper.getJsonArray<Card> (jsonString);

		for (int i = 0; i < ids.Count; i++){

			int ID = ids [i];
			if (ID > 16) {
				ID -= 16;
			}
			Card c = cardDeck[ID - 1];
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
