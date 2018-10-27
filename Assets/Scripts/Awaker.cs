using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awaker : MonoBehaviour {
	public GameObject Target;
	void Awake() {
		if (Target != null) {
			Target.SetActive(true);
		}
	}
}
