using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Card : MonoBehaviour
{
	public Action OnDelete;
	
	[SerializeField]
	private CardMark attachedMark;
	[SerializeField]
	private RectTransform gfx;
	[SerializeField]
	private float animSpeed = 5f;

	private BoxCollider2D col;
	private CardState state;
	private CardHand container;
	private Vector3 origPos;
	
	public RectTransform Gfx => gfx;
	public CardState State => state;
	public CardMark AttachedMark => attachedMark;

	public void AttachToMark(CardMark newMark) => attachedMark = newMark;

	private void Awake()
	{
		col = GetComponent<BoxCollider2D>();
	}

	private void OnDestroy()
	{
		OnDelete?.Invoke();
		
		if (attachedMark != null) 
			Destroy(attachedMark);
	}

	public void Init()
	{
		
	}

	public IEnumerator MoveTo(Vector3 targetPos, Vector3 targetRot)
	{
		while (Vector3.Distance(transform.position, targetPos) > 0.01f)
		{
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * animSpeed);
			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRot, Time.deltaTime * animSpeed);
			yield return null;
		}
	
		transform.position = targetPos;
		transform.eulerAngles = targetRot;
	}
	
	public int GetIndex()
	{
		return container.Cards.IndexOf(this);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
	}
}
