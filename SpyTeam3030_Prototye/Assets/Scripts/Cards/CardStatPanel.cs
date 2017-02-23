using UnityEngine;
using System.Collections;

public class CardStatPanel : MonoBehaviour {

	public GameObject[] cardHandelers;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HideCardStats(){
		for (int i = 0; i < cardHandelers.Length; i++) {
			cardHandelers [i].GetComponent<DragHandeler> ().clicked = false;
		}
	}

}
