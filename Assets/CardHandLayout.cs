using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class CardHandLayout : MonoBehaviour
{
	[SerializeField]
	public List<CardMark> cardMarks;
	[SerializeField] 
	private GameObject cardMarkSample;

	private void Awake()
	{
		cardMarks.Clear();
		cardMarkSample.SetActive(false);
	}

	public CardMark CreateCardMark(float width = 40f)
	{
		CardMark cardMark = Instantiate(cardMarkSample, transform).GetComponent<CardMark>();
		cardMark.gameObject.SetActive(true);
		cardMark.RectTransform.sizeDelta = new Vector2(width, cardMark.RectTransform.sizeDelta.y);
		cardMark.OnDelete += () => OnCardMarkDestroyed(cardMark);
		
		cardMarks.Add(cardMark);
		return cardMark;
	}

	private void OnCardMarkDestroyed(CardMark cardMark)
	{
		if (!cardMarks.Contains(cardMark))
		{
			Debug.Log($"[{nameof(CardHandLayout)}]: Can't delete place, it's not in the list", cardMark);
			return;
		}

		cardMark.OnDelete = null;
		cardMarks.Remove(cardMark);
	}

	public void SetPlaceWidth(CardMark cardMark, float width)
	{
		if (cardMark == null || !cardMarks.Contains(cardMark))
		{
			Debug.LogError($"[{nameof(CardHandLayout)}]: null or not found cardPlace");
			return;
		}
		
		cardMark.RectTransform.sizeDelta = new Vector2(width, cardMark.RectTransform.sizeDelta.y);
	}
	
	public void DeletePlace(int index)
	{
		Destroy(cardMarks[index].gameObject); 
		cardMarks.RemoveAt(index);
	}
	
	public void RebuildLayout()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
	}
}
