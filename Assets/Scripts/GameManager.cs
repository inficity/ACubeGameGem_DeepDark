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
	int MyHp;
	int OpHp;
	public Transform OpDmgPos;
	public Transform MyDmgPos;

	public GameObject MyTurn;
	public GameObject OpTurn;
	public GameObject PunchEnd;

	public GameObject DmgEffect;
	public Text DmgText;
	public GameObject GameUI;
	public PlayCard DetailCard;

	public GameObject WinUI;
	public GameObject LoseUI;

	[SerializeField]
	public List<GameObject> OpBuffs;
	[SerializeField]
	public List<GameObject> MeBuffs;

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
		DmgEffect.SetActive(false);
		DetailCard.gameObject.SetActive(false);
		OpBuffs.Concat(MeBuffs).ToObservable()
			.Subscribe(buff => buff.SetActive(false));

		NetworkManager.Instance.onGameStartedNotifier
		.Subscribe(msg => {
			NetworkUI.Instance.gameObject.SetActive(false);
			WinUI.SetActive(false);
			LoseUI.SetActive(false);
			MyCostText.text = $"{msg.cost}";
			OpCostText.text = $"{msg.enemyCost}";
			MyHp = msg.hp;
			OpHp = msg.enemyHP;
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
			msg.negative.Concat(msg.positive).ToObservable()
				.Subscribe(id => {
					Debug.Log($"draw card {id}");
					AddDirection(true, close => {
						var card = SpawnCard(id);
						(card.Card.IsNegative ? NegHands : PosHands).Add(card);
						AlignCards();
						Timer(0.6f, close);
					});
				});
			AddDirection(true, close => {
				obj.SetActive(true);
				Timer(3f, () => {
					obj.SetActive(false);
					close();
				});
			});
		});
		NetworkManager.Instance.onTurnActionRespondedNotifier
		.Subscribe(msg => {
			if (msg.approved) {
				if (pendingCard != null)
				{
					(pendingCard.Card.IsNegative ? NegHands : PosHands).Remove(pendingCard);
					if (pendingCard.Card.Id / 100 == 2 || pendingCard.Card.Id / 100 == 4)
					{
						AddDirection(true, close => {
							pendingCard.transform.DOScale(Vector3.zero, 0.3f);
							Timer(0.3f, () => {
								close();
								GameObject.Destroy(pendingCard.gameObject);
							});						
						});
					}
				}
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
					Debug.Log($"{msg.playerId} {msg.hp} {msg.cost}");
					if (msg.playerId == NetworkManager.Instance.clientId) {
						var dmg = MyHp - msg.hp;
						MyHp = msg.hp;
						MyHpText.text = $"<b>{msg.hp}</b>";
						MyCostText.text = $"<b>{msg.cost}</b>";
						if (dmg > 0)
						{
							AddDirection(true, close => {
								DmgEffect.transform.position = MyDmgPos.position;
								DmgText.text = $"{dmg}";
								DmgEffect.SetActive(true);
								DmgEffect.transform.DOShakePosition(0.6f);
								GameUI.transform.DOShakePosition(0.4f, 30, 30);
								Timer(0.8f, () => {
									DmgEffect.SetActive(false);
									close();
								});
							});
						}
					}
					else {
						var dmg = OpHp - msg.hp;
						OpHp = msg.hp;
						OpHpText.text = $"<b>{msg.hp}</b>";
						OpCostText.text = $"<b>{msg.cost}</b>";
						if (dmg > 0)
						{
							AddDirection(true, close => {
								DmgEffect.transform.position = OpDmgPos.position;
								DmgText.text = $"{dmg}";
								DmgEffect.SetActive(true);
								DmgEffect.transform.DOShakePosition(0.6f);
								GameUI.transform.DOShakePosition(0.4f, 30, 30);
								Timer(0.8f, () => {
									DmgEffect.SetActive(false);
									close();
								});
							});
						}
					}
				}
				break;
				case TurnActionEvent.CharacterStateChanged:
				{
					var card = OpCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"CharacterStateChanged op {card.Card.Name} {msg.hp} {msg.attack}");
						if (card._HP != msg.hp)
						{
							// 데미지
							AddDirection(true, close => {
								var dmg = card._HP - msg.hp;
								DmgEffect.transform.position = card.transform.position;
								DmgText.text = $"{dmg}";
								DmgEffect.SetActive(true);
								DmgEffect.transform.DOShakePosition(0.6f);
								GameUI.transform.DOShakePosition(0.4f, 30, 30);
								Timer(0.8f, () => {
									card.SetHP(msg.hp);
									card.SetAttack(msg.attack);
									DmgEffect.SetActive(false);
									close();
								});
							});
						}
						else
						{
							card.SetHP(msg.hp);
							card.SetAttack(msg.attack);
						}

						
						return;
					}
					card = MyCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"CharacterStateChanged me {card.Card.Name} {msg.hp} {msg.attack}");
						if (card._HP != msg.hp)
						{
							// 데미지
							AddDirection(true, close => {
								var dmg = card._HP - msg.hp;
								DmgEffect.transform.position = card.transform.position;
								DmgText.text = $"{dmg}";
								DmgEffect.SetActive(true);
								DmgEffect.transform.DOShakePosition(0.6f);
								GameUI.transform.DOShakePosition(0.4f, 30, 30);
								Timer(0.8f, () => {
									card.SetHP(msg.hp);
									card.SetAttack(msg.attack);
									DmgEffect.SetActive(false);
									close();
								});
							});
						}
						else
						{
							card.SetHP(msg.hp);
							card.SetAttack(msg.attack);
						}
					}
				}
				break;
				case TurnActionEvent.Destroyed:
				{
					var card = OpCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"Destroyed op {card.Card.Name}");
						AddDirection(true, close => {
							card.transform.DOScale(Vector3.one * 0.1f, 0.5f);
							Timer(0.6f, () => {
								OpCharacters.Remove(card);
								GameObject.Destroy(card.gameObject);
								close();
							});
						});
						AddDirection(true, close => {
							AlignCards();
							Timer(0.6f, close);
						});
						return;
					}
					card = MyCharacters.FirstOrDefault(c => c.InstanceId == msg.instanceId);
					if (card != null)
					{
						Debug.Log($"Destroyed me {card.Card.Name}");
						AddDirection(true, close => {
							card.transform.DOScale(Vector3.one * 0.1f, 0.5f);
							Timer(0.6f, () => {
								MyCharacters.Remove(card);
								GameObject.Destroy(card.gameObject);
								close();
							});
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
					Debug.Log($"add buff {msg.playerId} {msg.buffName}");
					var buffs = msg.playerId == NetworkManager.Instance.clientId ? MeBuffs : OpBuffs;
					var emptyBuff = buffs.FirstOrDefault(b => !b.active);
					if (emptyBuff != null)
					{
						emptyBuff.SetActive(true);
						emptyBuff.GetComponentInChildren<Text>().text = $"<b>{msg.buffName}</b>";
						emptyBuff.transform.localScale = Vector3.zero;
						AddDirection(true, close => {
							emptyBuff.transform.DOScale(Vector3.one, 0.5f);
							emptyBuff.transform.DOShakeRotation(0.5f, 90, 20);
							Timer(0.5f, close);
						});
					}
				}
				break;
				case TurnActionEvent.BuffRemoved:
				{
					Debug.Log($"remove buff {msg.playerId} {msg.buffName}");
					var buffs = msg.playerId == NetworkManager.Instance.clientId ? MeBuffs : OpBuffs;
					var emptyBuff = buffs.FirstOrDefault(b => b.active && b.GetComponentInChildren<Text>().text == msg.buffName);
					if (emptyBuff != null)
					{
						AddDirection(true, close => {
							emptyBuff.transform.DOScale(Vector3.zero, 0.5f);
							emptyBuff.transform.DOShakeRotation(0.5f, 90, 20);
							Timer(0.5f, () => {
								emptyBuff.SetActive(false);
								close();
							});
						});
					}
				}
				break;
			}
		});
		NetworkManager.Instance.onGameEndedNotifier
		.Subscribe(msg => {
			if (msg.winner == NetworkManager.Instance.clientId)
			{
				WinUI.SetActive(true);
				WinUI.transform.DORotate(new Vector3(0, 0, 720), 5, RotateMode.LocalAxisAdd);
				WinUI.transform.localScale = Vector3.zero;
				WinUI.transform.DOScale(Vector3.one, 5);
			}
			else
			{
				LoseUI.SetActive(true);
				LoseUI.transform.DORotate(new Vector3(0, 0, 720), 5, RotateMode.LocalAxisAdd);
				LoseUI.transform.localScale = Vector3.zero;
				LoseUI.transform.DOScale(Vector3.one, 5);
			}
		});
	}

	public void TestGame() {
		var samples = new int[]{203, 102, 103, 301, 302, 303};
		samples.ToObservable()
			.Subscribe(id => {
				var card = SpawnCard(id);
				card.IsCharacterCard = false;
				card.InstanceId = id;
				card._Attack = 1;
				(card.Card.IsNegative ? NegHands : PosHands).Add(card);
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
		playCard.SetAttack(0);
		playCard.SetPower(card.Power);
		playCard.SetHP(card.HP);
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
		Debug.Log($"{attacker.Card.Name} -> {attackee.Card.Name}");
		waitActionResponse = true;
		NetworkManager.Instance.sendAttackCharacter(attacker.InstanceId, attackee.InstanceId);
	}
	public void AttackPlayer(PlayCard attacker) {
		if (waitActionResponse) return;
		Debug.Log($"{attacker.Card.Name} -> player!");
		waitActionResponse = true;
		NetworkManager.Instance.sendAttackPlayer(attacker.InstanceId);
	}
	public void EndTurn() {
		if (waitActionResponse) return;
		waitActionResponse = true;
		pendingCard = null;
		NetworkManager.Instance.sendEndTurn();
	}
	
}
}
