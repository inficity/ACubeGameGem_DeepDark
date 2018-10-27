using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public GameObject Prefab;
	public Transform MyCharacterPosition;
	public Transform OpCharacterPosition;
	public Transform MyHandPosition;

	void Awake() {
		DOTween.Init();
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		var card = Object.Instantiate(Prefab);
		card.transform.SetParent(this.transform);
		card.transform.DOMove(MyHandPosition.position, 3).SetEase(Ease.InBounce);
	}

	public void UseCard(Card card) {
		Debug.Log("asdasd");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void JsonTest() {
		// var a = new Newtonsoft.Json.JsonSerializer();
		var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new {
			A = 10,
		});
		Newtonsoft.Json.JsonConvert.DeserializeObject<GameManager>("{}");
		// a.Serialize()
	}
}
