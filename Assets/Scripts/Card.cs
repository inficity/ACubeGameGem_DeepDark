
using UnityEngine;

public class Card
{
	public int Id { get; private set; }
	public string Name { get; private set; }
	public int Cost { get; private set; }
	public Texture2D Image { get; private set; }

	public Card(int id, string name, int cost)
	{
		this.Id = id;
		this.Name = name;
		this.Cost = cost;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnUse() {
		GameManager.Instance.UseCard(this);
	}
}
