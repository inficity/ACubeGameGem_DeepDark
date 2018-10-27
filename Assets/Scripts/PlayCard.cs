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
	public Texture2D Image { get; private set; }
	
	// Update is called once per frame

	void Start() {
	}

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
