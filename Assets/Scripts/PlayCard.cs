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
	
	bool Dragging;
	public void OnBeginDrag(PointerEventData eventData) {
		if (!GameManager.Instance.CanAction) return;
		Dragging = true;
	}
	public void OnDrag(PointerEventData eventData) {
		if (Dragging)
		{
			this.transform.position = eventData.position;
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
	}
}
}
