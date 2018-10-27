using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

using TimeSpan = System.TimeSpan;

namespace DeepDark.Client
{
public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public GameObject Prefab;
	public Transform MyCharacterPosition;
	public Transform OpCharacterPosition;
	public Transform MyPosHandPosition;
	public Transform MyNegHandPosition;

	void Awake() {
		DOTween.Init();
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		Observable.Interval(TimeSpan.FromSeconds(1))
			.Take(6)
			.Subscribe(_ => {
				SpawnCard(_ < 3);
			});
			
		StartCoroutine(DirectionLoop());
	}

	List<PlayCard> PosHands = new List<PlayCard>();
	List<PlayCard> NegHands = new List<PlayCard>();
	void SpawnCard(bool isPositive) {
		var cardInst = Object.Instantiate(Prefab);
		var playCard = cardInst.GetComponent<PlayCard>();

		(isPositive ? PosHands : NegHands).Add(playCard);
		playCard.transform.SetParent(MyPosHandPosition);
		playCard.transform.localScale = Vector3.one;
		AlignCards();
	}

	Queue<IEnumerator> DirectionQueue = new Queue<IEnumerator>();

	IEnumerator DirectionLoop() {
		while (true) {
			yield return null;
			if (DirectionQueue.Count == 0)
				continue;
			var direction = DirectionQueue.Dequeue();
			yield return StartCoroutine(direction);
		}
	}

	void AlignCards() {
		const float angleStep = 23.0f;
		const float distance = 700.0f;
		var distanceAdjust = new float[]{-20, 0, -20, 0, -20};
		{
			var idx = 0;
			float beginAngle = (PosHands.Count - 1) / 2.0f * angleStep;
			PosHands.ForEach(card => {
				var pos = MyPosHandPosition.position + Quaternion.AngleAxis(+90 + beginAngle - angleStep * idx, Vector3.forward) * new Vector3(distance + distanceAdjust[idx], 0, 0);
				card.transform.DOMove(pos, 0.5f);
				card.transform.DORotateQuaternion(Quaternion.AngleAxis(beginAngle - angleStep * idx, Vector3.forward), 0.5f);
				idx++;
			});
		}
		{
			var idx = 0;
			float beginAngle = (NegHands.Count - 1) / 2.0f * angleStep;
			NegHands.ForEach(card => {
				var pos = MyNegHandPosition.position + Quaternion.AngleAxis(+90 + beginAngle - angleStep * idx, Vector3.forward) * new Vector3(distance + distanceAdjust[idx], 0, 0);
				card.transform.DOMove(pos, 0.5f);
				card.transform.DORotateQuaternion(Quaternion.AngleAxis(beginAngle - angleStep * idx, Vector3.forward), 0.5f);
				idx++;
			});
		}
	}

	public void UseCard(PlayCard card) {
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
}
