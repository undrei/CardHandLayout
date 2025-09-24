using System;
using UnityEngine;

public class CardMark : MonoBehaviour
{
	public Action OnDelete;
		
	private CardHandLayout layout;
	private RectTransform rectTransform;
	
	public RectTransform RectTransform 
	{
		get 
		{
			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();

			return rectTransform;
		}
	}
	
	private void OnDestroy()
	{
		OnDelete?.Invoke();
	}
}
