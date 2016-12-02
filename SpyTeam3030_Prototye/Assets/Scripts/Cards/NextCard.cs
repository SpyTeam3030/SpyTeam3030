using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextCard : MonoBehaviour {

	public Image cardImage;
	public Text cardText;
	private Card card;

	public CardManager mCardManager;
	
	// Update is called once per frame
	void Update () {
		card = mCardManager.ReadTop ();
		if (card.id != 0) {
			cardImage.sprite = card.image;
			cardText.text = card.description;
		}
	}
}
