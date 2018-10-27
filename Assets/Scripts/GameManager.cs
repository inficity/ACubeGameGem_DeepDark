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

public class Direction
{
	public bool NeedWaitPreviousDirection;
	public System.Action<System.Action> Action;
}

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	public GameObject Prefab;
	public Transform MyCharacterPosition;
	public Transform OpCharacterPosition;
	public Transform MyPosHandPosition;
	public Transform MyNegHandPosition;
	public RectTransform UseCardArea;

	[SerializeField]
	public List<CardImageReferer> CardImages;

	public Text OpCostText;
	public Text MyCostText;
	public Text OpHpText;
	public Text MyHpText;

	public GameObject MyTurn;
	public GameObject OpTurn;
	public GameObject PunchEnd;


	void Awake() {
		DOTween.Init();
		Instance = this;
	}

	void Timer(float seconds, System.Action action)
	{
		Observable.Timer(TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => action());
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(DirectionLoop());

		PunchEnd.SetActive(false);

		NetworkManager.Instance.onGameStartedNotifier
		.Subscribe(msg => {
			NetworkUI.Instance.gameObject.SetActive(false);
			MyCostText.text = $"{msg.cost}";
			OpCostText.text = $"{msg.enemyCost}";
			MyHpText.text = $"{msg.hp}";
			OpHpText.text = $"{msg.enemyHP}";
			msg.negativeHand.Concat(msg.positiveHand).ToObservable()
				.Subscribe(id => {
					AddDirection(true, close => {
						var card = SpawnCard(id);
						(card.Card.IsNegative ? NegHands : PosHands).Add(card);
						AlignCards();
						Timer(0.6f, close);
					});
				});
		});
		NetworkManager.Instance.onTurnStartedNotifier
		.Subscribe(msg => {
			var obj = (msg.clientId == NetworkManager.Instance.clientId) ? MyTurn : OpTurn;
			AddDirection(true, close => {
				obj.SetActive(true);
				Timer(2f, () => {
					obj.SetActive(false);
					close();
				});
			});
		});
		NetworkManager.Instance.onTurnActionRespondedNotifier
		.Subscribe(msg => {
			if (msg.approved) {
				if (pendingCard != null)
					(pendingCard.Card.IsNegative ? NegHands : PosHands).Remove(pendingCard);
			}
			else {
				pendingCard = null;
				AlignCards();
			}
			waitActionResponse = false;
		});
		NetworkManager.Instance.onTurnActionEventNotifier
		.Subscribe(msg => {
			switch (msg.turnActionEvent)
			{
				case TurnActionEvent.Instantiated:
				{
					if (pendingCard != null && pendingCard.Card.Id == msg.cardId)
					{
						var card = pendingCard;
						pendingCard = null;
						card.IsCharacterCard = true;
						card.InstanceId = msg.instanceId;
						card.SetHP(msg.hp);
						card.SetAttack(msg.attack);
						(msg.playerId == NetworkManager.Instance.clientId ? MyCharacters : OpCharacters)
							.Add(card);
						AddDirection(true, close => {
							AlignCards();
							Timer(0.6f, close);
						});
					}
					else
					{
						var card = SpawnCard(msg.cardId);
						card.IsCharacterCard = true;
						card.InstanceId = msg.instanceId;
						card.SetHP(msg.hp);
						card.SetAttack(msg.attack);
						(msg.playerId == NetworkManager.Instance.clientId ? MyCharacters : OpCharacters)
							.Add(card);
						AddDirection(true, close => {
							AlignCards();
							Timer(0.6f, close);
						});
					}
				}
				break;
				case TurnActionEvent.StateChanged:
				{
					if (msg.playerId == NetworkManager.Instance.clientId) {
						MyHpText.text = $"<b>{msg.hp}</b>";
						MyCostText.text = $"<b>{msg.cost}</b>";
					}
					else {
						OpHpText.text = $"<b>{msg.hp}</b>";
						OpCostText.text = $"<b>{msg.cost}</b>";
					}
				}
				break;
				case TurnActionEvent.CharacterStateChanged:
				{
					var card = OpCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"CharacterStateChanged op {card.Card.Name} {msg.hp} {msg.attack}");
						card.SetHP(msg.hp);
						card.SetAttack(msg.attack);

						// var baseX = card.transform.localScale.x;
						// AddDirection(true, close => {
						// 	card.transform.DOScale(Vector3.one * baseX / 2, 0.1f);
						// 	Timer(0.1f, close);
						// });
						// AddDirection(true, close => {
						// 	card.SetHP(msg.hp);
						// 	card.SetAttack(msg.attack);
						// 	card.transform.DOScale(Vector3.one * baseX, 0.1f);
						// 	Timer(0.1f, close);
						// });
					}
					card = MyCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"CharacterStateChanged me {card.Card.Name} {msg.hp} {msg.attack}");
						card.SetHP(msg.hp);
						card.SetAttack(msg.attack);
						// var baseX = card.transform.localScale.x;
						// AddDirection(true, close => {
						// 	card.transform.DOScale(Vector3.one * baseX / 2, 0.1f);
						// 	Timer(0.1f, close);
						// });
						// AddDirection(true, close => {
						// 	card.SetHP(msg.hp);
						// 	card.SetAttack(msg.attack);
						// 	card.transform.DOScale(Vector3.one * baseX, 0.1f);
						// 	Timer(0.1f, close);
						// });
					}
				}
				break;
				case TurnActionEvent.Destroyed:
				{
					var card = OpCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"Destroyed op {card.Card.Name}");
						OpCharacters.Remove(card);
						AddDirection(true, close => {
							card.transform.DOScale(Vector3.one * 0.1f, 0.5f);
							Timer(0.6f, close);
						});
						AddDirection(true, close => {
							AlignCards();
							Timer(0.6f, close);
						});
					}
					card = MyCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"Destroyed me {card.Card.Name}");
						MyCharacters.Remove(card);
						AddDirection(true, close => {
							card.transform.DOScale(Vector3.one * 0.1f, 0.5f);
							Timer(0.6f, close);
						});
						AddDirection(true, close => {
							AlignCards();
							Timer(0.6f, close);
						});
					}
				}
				break;
				case TurnActionEvent.BuffAttached:
				{
					
				}
				break;
				case TurnActionEvent.BuffRemoved:
				{
					
				}
				break;
			}
		});
	}

	public void TestGame() {
		var samples = new int[]{101, 102, 103, 301, 302, 303};
		samples.ToObservable()
			.Subscribe(id => {
				var card = SpawnCard(id);
				card.IsCharacterCard = true;
				card.InstanceId = id;
				card._Attack = 1;
				(card.Card.IsNegative ? OpCharacters : MyCharacters).Add(card);
			});
		AlignCards();
	}

	[System.NonSerialized]
	public List<PlayCard> OpCharacters = new List<PlayCard>();
	[System.NonSerialized]
	public List<PlayCard> MyCharacters = new List<PlayCard>();

	[System.NonSerialized]
	public List<PlayCard> PosHands = new List<PlayCard>();
	[System.NonSerialized]
	public List<PlayCard> NegHands = new List<PlayCard>();

	PlayCard SpawnCard(int cardId) {
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

		playCard.transform.SetParent(MyPosHandPosition);
		playCard.transform.localScale = Vector3.one;

		return playCard;
	}

	Queue<Direction> DirectionQueue = new Queue<Direction>();
	int ActiveDirectionCount = 0;

	void AddDirection(bool needWaitPreviousDirection, System.Action<System.Action> direction)
	{
		var d = new Direction();
		d.NeedWaitPreviousDirection = needWaitPreviousDirection;
		d.Action = close => {
			++ActiveDirectionCount;
			direction(close);
		};
		DirectionQueue.Enqueue(d);
	}

	IEnumerator DirectionLoop() {
		while (true) {
			yield return null;
			if (DirectionQueue.Count == 0)
				continue;
			var peek = DirectionQueue.Peek();
			if (peek.NeedWaitPreviousDirection && ActiveDirectionCount > 0)
				continue;
			DirectionQueue.Dequeue();
			peek.Action(() => { --ActiveDirectionCount; });
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

	public bool CanAction { get {
		return !waitActionResponse;
	}}

	bool waitActionResponse;
	PlayCard pendingCard;
	public void UseCard(PlayCard card) {
		if (waitActionResponse) return;
		waitActionResponse = true;
		pendingCard = card;
		NetworkManager.Instance.sendUseCard(card.Card.Id);
	}
	public void AttackCard(PlayCard attacker, PlayCard attackee) {
		if (waitActionResponse) return;
		waitActionResponse = true;
		NetworkManager.Instance.sendAttackCharacter(attacker.InstanceId, attackee.InstanceId);
	}

	public void EndTurn() {
		if (waitActionResponse) return;
		waitActionResponse = true;
		pendingCard = null;
		NetworkManager.Instance.sendEndTurn();
	}
	
}
}
