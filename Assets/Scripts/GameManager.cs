using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

using TimeSpan = System.TimeSpan;

namespace DeepDark.Client
{

[System.Serializable]
public class CardImageReferer
{
	public int CardId;
	public Sprite Sprite;
}
public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public GameObject Prefab;
	public Transform MyCharacterPosition;
	public Transform OpCharacterPosition;
	public Transform MyPosHandPosition;
	public Transform MyNegHandPosition;

	[SerializeField]
	public List<CardImageReferer> CardImages;

	public Text OpCostText;
	public Text MyCostText;

	void Awake() {
		DOTween.Init();
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		StartCoroutine(DirectionLoop());

		NetworkManager.Instance.onGameStartedNotifier
		.Subscribe(msg => {
			NetworkUI.Instance.gameObject.SetActive(false);
			MyCostText.text = $"{msg.cost}";
			OpCostText.text = $"{msg.enemyCost}";
			msg.negativeHand.Concat(msg.positiveHand).ToObservable()
				.Zip(Observable.Interval(TimeSpan.FromSeconds(0.6f))
				, (id, _) => id)
				.Subscribe(id => {
					SpawnCard(id);
				});
		});
	}

	public void TestGame() {
		var samples = new int[]{101, 102, 103, 301, 302, 303};
		Observable.Interval(TimeSpan.FromSeconds(0.7f))
			.Take(6)
			.Subscribe(_ => {
				SpawnCard(samples[Random.Range(0, samples.Count())]);
			});
	}

	List<PlayCard> OpCharacters = new List<PlayCard>();
	List<PlayCard> MyCharacters = new List<PlayCard>();

	List<PlayCard> PosHands = new List<PlayCard>();
	List<PlayCard> NegHands = new List<PlayCard>();
	void SpawnCard(int cardId) {
		var cardInst = Object.Instantiate(Prefab);
		var playCard = cardInst.GetComponent<PlayCard>();

		var card = CardManager.GetCard(cardId);
		playCard.Card = card;

		Debug.Log($"SpawnCard {cardId} neg {card.IsNegative} ");
		var imageReferer = CardImages.FirstOrDefault(c => c.CardId == cardId);
		if (imageReferer != null)
			playCard.Image.sprite = imageReferer.Sprite;
		playCard.Title.text = $"<b>{card.Name}</b>";
		playCard.Description.text = card.Description;
		playCard.Power.text = $"<b>{card.Power}</b>";
		playCard.HP.text = $"<b>{card.HP}</b>";
		playCard.Cost.text = $"<b>{card.Cost}</b>";
		playCard.Glow.color = card.IsNegative ? Color.red : Color.blue;

		(card.IsNegative ? NegHands : PosHands).Add(playCard);
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

	public void AlignCards() {
		const float angleStep = 23.0f;
		const float distance = 700.0f;
		var distanceAdjust = new float[]{-20, 0, -20, 0, -20};
		{
			var idx = 0;
			float beginAngle = (PosHands.Count - 1) / 2.0f * angleStep;
			PosHands.ForEach(card => {
				var pos = MyPosHandPosition.position + new Vector3(0, -distance, 0) + Quaternion.AngleAxis(+90 + beginAngle - angleStep * idx, Vector3.forward) * new Vector3(distance + distanceAdjust[idx], 0, 0);
				card.transform.DOMove(pos, 0.5f);
				card.transform.DORotateQuaternion(Quaternion.AngleAxis(beginAngle - angleStep * idx, Vector3.forward), 0.5f);
				card.transform.DOScale(Vector3.one, 0.5f);
				idx++;
			});
		}
		{
			var idx = 0;
			float beginAngle = (NegHands.Count - 1) / 2.0f * angleStep;
			NegHands.ForEach(card => {
				var pos = MyNegHandPosition.position + new Vector3(0, -distance, 0) + Quaternion.AngleAxis(+90 + beginAngle - angleStep * idx, Vector3.forward) * new Vector3(distance + distanceAdjust[idx], 0, 0);
				card.transform.DOMove(pos, 0.5f);
				card.transform.DORotateQuaternion(Quaternion.AngleAxis(beginAngle - angleStep * idx, Vector3.forward), 0.5f);
				card.transform.DOScale(Vector3.one, 0.5f);
				idx++;
			});
		}
		{
			const float step = 150;
			var idx = 0;
			MyCharacters.ForEach(card => {
				var pos = MyCharacterPosition.position - new Vector3((MyCharacters.Count - 1) / 2.0f * step, 0, 0);
				card.transform.DOMove(pos + new Vector3(step * idx, 0, 0), 0.5f);
				card.transform.DORotateQuaternion(Quaternion.identity, 0.5f);
				card.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
				idx++;
			});
		}
		{
			const float step = 150;
			var idx = 0;
			OpCharacters.ForEach(card => {
				var pos = OpCharacterPosition.position - new Vector3((OpCharacters.Count - 1) / 2.0f * step, 0, 0);
				card.transform.DOMove(pos + new Vector3(step * idx, 0, 0), 0.5f);
				card.transform.DORotateQuaternion(Quaternion.AngleAxis(180, Vector3.forward), 0.5f);
				card.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
				idx++;
			});
		}
	}

	bool waitActionResponse;
	public void UseCard(PlayCard card) {
		if (waitActionResponse) return;
		waitActionResponse = true;
		NetworkManager.Instance.sendUseCard(card.Card.Id);
	}
	
}
}
