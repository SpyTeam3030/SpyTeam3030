using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct Card{
	public int id;

	public float attack;
	public float health;
	public float speed;
	public float attackSpeed;
	public float attackDistance;

	public Sprite image;
}

public class CardManager : MonoBehaviour {

	public Sprite[] pictures;

	private Stack<Card> cards;
	private List<int> ids;

	// Use this for initialization
	void Awake () {
		cards = new Stack<Card> ();
		ids = new List<int> ();
		for (int i = 0; i < 30; i++) {
			ids.Add(i + 1);
		}
		ids = Shuffle (ids);

		for (int i = 0; i < ids.Count; i++){
//			Debug.Log (ids [i]);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextCard(GameObject go){
		go.GetComponent<DragHandeler> ().Init (cards.Pop ());
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
