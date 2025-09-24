using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Card : MonoBehaviour
{
	public Action OnDelete;
	
	[SerializeField]
	private RectTransform gfx;
	[SerializeField]
	private float animTime = 0.5f;
	
	private CardSlot attachedSlot;
	private CardState state;
	
	public RectTransform Gfx => gfx;
	public CardState State => state;
	public CardSlot AttachedSlot => attachedSlot;

	public void AttachToMark(CardSlot newSlot) => attachedSlot = newSlot;

	private void OnDestroy()
	{
		OnDelete?.Invoke();
		
		if (attachedSlot != null) 
			Destroy(attachedSlot);
	}
	
	public IEnumerator MoveTo(Vector3 targetPos, Vector3 targetRot)
	{
		Vector3 startPos = transform.position;
		Vector3 startRot = transform.eulerAngles;
		
		Vector3 normalizedTargetRot = NormalizeAngles(targetRot, startRot);
    
		float elapsed = 0f;
    
		while (elapsed < animTime)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / animTime;
        
			transform.position = Vector3.Lerp(startPos, targetPos, t);
			transform.eulerAngles = Vector3.Lerp(startRot, normalizedTargetRot, t);
        
			yield return null;
		}

		transform.position = targetPos;
		transform.eulerAngles = targetRot;
	}
	
	private Vector3 NormalizeAngles(Vector3 target, Vector3 current)
	{
		Vector3 normalized = target;
		
		for (int i = 0; i < 3; i++)
		{
			float diff = target[i] - current[i];
			
			while (diff > 180f) diff -= 360f;
			while (diff < -180f) diff += 360f;
        
			normalized[i] = current[i] + diff;
		}
    
		return normalized;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
	}
}
