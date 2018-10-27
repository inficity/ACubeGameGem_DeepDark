using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepDark.Client
{
public class PlayCard : MonoBehaviour
{
	public Card Card;
	public Texture2D Image { get; private set; }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnUse() {
		GameManager.Instance.UseCard(this);
	}
}
}
