using UnityEngine;
using System.Collections;

public class HealthBarRotate : MonoBehaviour {

	void Start()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }
}
