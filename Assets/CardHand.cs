using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
	[SerializeField]
	protected CardContainerConfig config;
	
	[SerializeField] 
	protected CardHandLayout layout;
	[SerializeField]
	protected Transform cardContainer;
	[SerializeField] 
	protected GameObject cardSample;
	[SerializeField]
	protected List<Card> cards = new List<Card>();
	
	public List<Card> Cards => cards;
	public CardHandLayout Layout => layout;
	public CardContainerConfig Config => config;
	
	private void Start()
	{
		SpawnCards();
	}

	[ContextMenu("Spawn Cards")]
	private void SpawnCards()
	{
		ClearAll();
		
		for (int i = 0; i < 10; i++)
		{
			Card card = CreateNewCard();
			card.gameObject.name = $"Card [{i}]";
			card.AttachedMark.name = $"CardMark [{i}]";
		}
		UpdateVisuals();
	}
	
	private Card CreateNewCard()
	{
		Card newCard = Instantiate(cardSample, cardContainer).GetComponent<Card>();
		newCard.gameObject.SetActive(true);
		newCard.AttachToMark(layout.CreateCardMark(config.CardPlaceWidth));
		layout.RebuildLayout();
		cards.Add(newCard);

		newCard.OnDelete += () => RemoveCard(newCard);
		
		return newCard;
	}
	
	private void RemoveCard(Card card)
	{
		if (card != null && cards.Contains(card))
		{
			cards.Remove(card);
		}
	}
	
	private void ClearAll()
	{
		foreach (Card card in cards)
		{
			if (card == null) continue;
			Destroy(card.gameObject);
		}
		cards.Clear();
		
		foreach (CardMark cardMark in layout.cardMarks)
		{
			if (cardMark == null) continue;
			Destroy(cardMark.gameObject);
		}
		layout.cardMarks.Clear();
	}
	
#region Position updating
	[ContextMenu("Update Visuals")]
	private void UpdateVisuals()
	{
		layout.GetComponent<HorizontalLayoutGroup>().spacing = config.CardSpacing;
		layout.RebuildLayout();

		if (cards.Count != layout.cardMarks.Count)
		{
			Debug.LogError($"[{nameof(CardHand)}]: Layout place count and cards count are different!!");
			return;
		}
		
		foreach (var card in cards)
		{
			if (card.State == CardState.Hovered)
				continue;
			
			StartCoroutine(card.MoveTo(GetPosition(card), GetRotation(card)));
		}
	}
	
	private Vector3 GetPosition(Card card)
	{
		if (card == null || card.AttachedMark == null)
		{
			Debug.LogWarning($"[{nameof(CardHand)}]: Can't get position of the card");
			return Vector3.zero;
		}
		
		int index = cards.IndexOf(card);
		float curveResult;
		
		if (cards.Count <= 1)
			curveResult = config.CurvePositioning.Evaluate(0.5f) * config.VerticalOffsetCoef;
		else
			curveResult = config.CurvePositioning.Evaluate((float) index / (cards.Count - 1)) * config.VerticalOffsetCoef * (cards.Count - 1);
		
		Vector3 newPos = card.AttachedMark.transform.position + transform.up * curveResult;

		return newPos;
	}
	
	private Vector3 GetRotation(Card card)
	{
		int index = cards.IndexOf(card);
		float curveResult;
	
		if (cards.Count <= 1) 
			curveResult = config.CurveRotation.Evaluate(0.5f) * config.RotationCoef;
		else
			curveResult = config.CurveRotation.Evaluate((float)index / (cards.Count - 1)) * config.RotationCoef * (cards.Count - 1);
		
		Quaternion localRotation = Quaternion.Euler(0, 0, curveResult + config.RotationOffset);
		Quaternion finalRotation = transform.rotation * localRotation;
	
		return finalRotation.eulerAngles;
	}
	
	public void ReorderCards()
	{
		foreach (Card card in cards)
		{
			card.transform.SetSiblingIndex(cards.IndexOf(card));
		}
	}
#endregion
}
