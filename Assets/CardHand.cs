using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
	[SerializeField]
	protected CardHandConfig config;
	
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
	public CardHandConfig Config => config;
	
	private void Start()
	{
		SpawnCards();
	}
	
	private void SpawnCards()
	{
		ClearAll();
		
		for (int i = 0; i < 10; i++)
		{
			Card card = CreateNewCard();
			card.gameObject.name = $"Card [{i}]";
			card.AttachedSlot.name = $"CardSlot [{i}]";
		}
		UpdateVisuals();
	}
	
	private Card CreateNewCard()
	{
		Card newCard = Instantiate(cardSample, cardContainer).GetComponent<Card>();
		newCard.gameObject.SetActive(true);
		newCard.AttachToMark(layout.CreateCardSlot());
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
			
			if (Application.isPlaying) 
				Destroy(card.gameObject);
			else
				DestroyImmediate(card.gameObject);
		}
		cards.Clear();
		
		foreach (CardSlot cardSlot in layout.CardSlots)
		{
			if (cardSlot == null) continue;
			
			if (Application.isPlaying) 
				Destroy(cardSlot.gameObject);
			else
				DestroyImmediate(cardSlot.gameObject);
		}
		layout.CardSlots.Clear();
	}
	
	private void UpdateVisuals()
	{
		layout.GetComponent<HorizontalLayoutGroup>().spacing = config.CardSpacing;
		layout.RebuildLayout();

		if (cards.Count != layout.CardSlots.Count)
		{
			Debug.LogError($"[{nameof(CardHand)}]: Layout place count and cards count are different!!");
			return;
		}
		
		foreach (var card in cards)
		{
			if (card.State == CardState.Hovered)
				continue;
			
			if (Application.isPlaying)
				StartCoroutine(card.MoveTo(GetPosition(card), GetRotation(card)));
			else
			{
				card.transform.position = GetPosition(card);
				card.transform.eulerAngles = GetRotation(card);
			}
		}
	}
	
	private Vector3 GetPosition(Card card)
	{
		if (card == null || card.AttachedSlot == null)
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
		
		Vector3 newPos = card.AttachedSlot.transform.position + transform.up * curveResult;

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
		
	#region Gizmos Drawing
	private void OnDrawGizmos()
	{
		DrawCurveGizmos();
	}

	private void DrawCurveGizmos()
	{
		if (config == null || config.CurvePositioning == null || layout == null)
			return;
		
		layout.RebuildLayout();

		if (layout.CardSlots.Count < 2)
			return;
		
		Vector3 firstSlotPos = layout.CardSlots[0].transform.position;
		Vector3 lastSlotPos = layout.CardSlots[^1].transform.position;
		
		// CardSlots position
		Gizmos.color = Color.gray;
		Gizmos.DrawLine(firstSlotPos, lastSlotPos);
		foreach (var slot in layout.CardSlots)
		{
			Gizmos.DrawWireSphere(slot.transform.position, 10f);
		}

		// Hand curve
		Gizmos.color = Color.cyan;
		Vector3 prevPoint = Vector3.zero;
		bool firstPoint = true;

		int curveSteps = 50;

		for (int i = 0; i <= curveSteps; i++)
		{
			float t = (float)i / curveSteps;
			
			Vector3 basePosition = Vector3.Lerp(firstSlotPos, lastSlotPos, t);
			
			float curveValue;
			if (cards.Count <= 1)
				curveValue = config.CurvePositioning.Evaluate(0.5f) * config.VerticalOffsetCoef;
			else
				curveValue = config.CurvePositioning.Evaluate(t) * config.VerticalOffsetCoef * (cards.Count - 1);
			
			Vector3 curvePoint = basePosition + transform.up * curveValue;
			
			if (!firstPoint)
			{
				Gizmos.DrawLine(prevPoint, curvePoint);
			}

			prevPoint = curvePoint;
			firstPoint = false;
		}
		
		if (cards.Count > 0)
		{
			Gizmos.color = Color.green;
			for (int i = 0; i < cards.Count; i++)
			{
				if (cards[i] != null && cards[i].AttachedSlot != null)
				{
					Vector3 cardPos = GetPosition(cards[i]);
					Vector3 cardRot = GetRotation(cards[i]);
					
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(cards[i].AttachedSlot.transform.position, cardPos);
					DrawCardRotationGizmo(cardPos, cardRot);
				}
			}
		}
	}
	
	private void DrawCardRotationGizmo(Vector3 cardPosition, Vector3 cardRotation)
	{
		Quaternion rotation = Quaternion.Euler(cardRotation);
		Vector3 cardUp = rotation * Vector3.up;
		Vector3 cardRight = rotation * Vector3.right;
		Vector3 cardLeft = rotation * Vector3.left;
		
		// Card direction
		Gizmos.color = Color.red;
		Vector3 arrowEnd = cardPosition + cardUp * 30f;
		Gizmos.DrawLine(cardPosition, arrowEnd);
    
		// Card bottom line
		Gizmos.DrawLine(cardPosition + cardLeft * 10f, cardPosition + cardRight * 10f);
	}

	#endregion
}
