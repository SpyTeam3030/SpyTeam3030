using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text minute;
	public Text second;
	public Text milisecond;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateTime(float time){
//        Debug.LogFormat("update time in local with {0}", time);
		int min, sec, mili;
		min = (int)Mathf.Floor(time / 60);
		sec = (int)Mathf.Floor (time % 60);
		mili = (int)Mathf.RoundToInt ((time * 100) % 100);

		minute.text = min.ToString ();
		second.text = sec.ToString ("00");
		milisecond.text = mili.ToString ("00");
	}
}
