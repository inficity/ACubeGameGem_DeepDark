using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace DeepDark.Client
{
public class PlayCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Card Card;
	public Image Image;
	public Text Title;
	public Text Description;
	public Text Power;
	public Text HP;
	public Text Cost;
	public Image Glow;
	public bool IsCharacterCard;
	public int InstanceId;

	public int _HP;
	public int _Attack;
	public int _Power;

	public void SetHP(int hp)
	{
		_HP = hp;
		HP.text = $"<b>{hp}</b>";
	}
	public void SetAttack(int attack)
	{
		_Attack = attack;
	}
	public void SetPower(int power)
	{
		_Power = power;
		Power.text = $"<b>{power}</b>";
	}
	
	bool Dragging;
	bool DraggingPunch;
	public void OnBeginDrag(PointerEventData eventData) {
		if (!GameManager.Instance.CanAction)
		{
			Debug.Log($"Cant action {Card.Name}");
			return;
		}
		if (IsCharacterCard)
		{
			if (_Attack <= 0)
			{
				Debug.Log($"No attack {Card.Name}");
				return;
			}
			if (!GameManager.Instance.MyCharacters.Contains(this))
			{
				Debug.Log($"Attack only by my character {Card.Name}");
				return;
			}
			DraggingPunch = true;
			GameManager.Instance.PunchEnd.SetActive(true);
			GameManager.Instance.PunchEnd.transform.position = eventData.position;
		}
		else
		{
			Dragging = true;

		}
	}
	public void OnDrag(PointerEventData eventData) {
		if (Dragging)
		{
			this.transform.position = eventData.position;
		}
		if (DraggingPunch)
		{
			GameManager.Instance.PunchEnd.transform.position = eventData.position;
		}
	}
	public void OnEndDrag(PointerEventData eventData) {
		if (Dragging)
		{
			Dragging = false;
			var corners = new Vector3[4];
			GameManager.Instance.UseCardArea.GetWorldCorners(corners);
			var rect = new Rect();
			rect.xMin = corners[0].x;
			rect.xMax = corners[2].x;
			rect.yMin = corners[0].y;
			rect.yMax = corners[2].y;
			if (rect.Contains(eventData.position))
			{
				GameManager.Instance.UseCard(this);
			}
			else
			{
				GameManager.Instance.AlignCards();
			}
		}
		if (DraggingPunch)
		{
			var targetCard = eventData.pointerCurrentRaycast.gameObject?.GetComponentInParent<PlayCard>();
			GameManager.Instance.PunchEnd.SetActive(false);
			if (GameManager.Instance.OpCharacters.Contains(targetCard))
			{
				GameManager.Instance.AttackCard(this, targetCard);
			}
			if (eventData.pointerCurrentRaycast.gameObject?.GetComponentInParent<PlayerPortait>() != null)
			{
				GameManager.Instance.AttackPlayer(this);
			}
		}
	}
}
}
