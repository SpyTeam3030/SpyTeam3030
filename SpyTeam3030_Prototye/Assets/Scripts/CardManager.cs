using UnityEngine;
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
	public TextAsset json;
	public Sprite[] images;
	int num_cards = 30;

	private Stack<Card> cards;//contains everything needed for a card
	private List<int> ids;//ids from 1-30
	public List<Card> hand;

	// Use this for initialization
	void Awake () {
		cards = new Stack<Card> ();
		ids = new List<int> ();
		hand = new List<Card> ();
		for (int i = 0; i < num_cards; i++) {
			ids.Add(i + 1);
		}
		ids = Shuffle (ids);

		Card[] cardDeck = JsonHelper.getJsonArray<Card> (json.text);
		for (int i = 0; i < ids.Count; i++){

			int ID = ids [i];
			if (ID  < 25 || ID == 30) {
				Card c = cardDeck[ID - 1];
				c.image = images [ID - 1];

				cards.Push (c);
			}
		}
	}

	public void NextCard(GameObject go, int cardID){
		for (int i = 0; i < hand.Count; i++) {
			if (cardID == hand[i].id) {
				hand.Remove(hand [i]);
			}
		}
		Card new_card = cards.Pop ();
		go.GetComponent<DragHandeler> ().Init (new_card);
		hand.Add (new_card);
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

	public List<Card> GetHand(){
		return hand;
	}
}
