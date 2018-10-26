using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public GameObject Prefab;
	public Transform MyHandPosition;

	void Awake() {
		DOTween.Init();
	}
	// Use this for initialization
	void Start () {
		var card = Object.Instantiate(Prefab);
		card.transform.SetParent(this.transform);
		card.transform.DOMove(MyHandPosition.position, 3).SetEase(Ease.InBounce);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
