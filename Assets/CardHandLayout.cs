using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class CardHandLayout : MonoBehaviour
{
	[SerializeField] 
	private GameObject cardSlotSample;
	[SerializeField]
	private List<CardSlot> cardSlots;
	
	public List<CardSlot> CardSlots => cardSlots;

	private void Awake()
	{
		cardSlots.Clear();
		cardSlotSample.SetActive(false);
	}

	public CardSlot CreateCardSlot()
	{
		CardSlot cardSlot = Instantiate(cardSlotSample, transform).GetComponent<CardSlot>();
		cardSlot.gameObject.SetActive(true);
		cardSlot.OnDelete += () => OnCardMarkDestroyed(cardSlot);
		
		cardSlots.Add(cardSlot);
		return cardSlot;
	}

	private void OnCardMarkDestroyed(CardSlot cardSlot)
	{
		if (!cardSlots.Contains(cardSlot))
		{
			Debug.Log($"[{nameof(CardHandLayout)}]: Can't delete place, it's not in the list", cardSlot);
			return;
		}

		cardSlot.OnDelete = null;
		cardSlots.Remove(cardSlot);
	}
	
	public void RebuildLayout()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
	}
}
