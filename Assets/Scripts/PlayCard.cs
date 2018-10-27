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
	// public Texture2D Image { get; private set; }

	public Image Image;
	public Text Title;
	public Text Description;
	public Text Power;
	public Text HP;
	public Text Cost;
	
	// Update is called once per frame

	bool Dragging;
	public void OnBeginDrag(PointerEventData eventData) {
		Dragging = true;
	}
	public void OnDrag(PointerEventData data) {
		this.transform.position = data.position;
	}
	public void OnEndDrag(PointerEventData eventData) {
		Dragging = false;
		GameManager.Instance.AlignCards();
	}
	public void OnUse() {
		GameManager.Instance.UseCard(this);
	}
}
}
