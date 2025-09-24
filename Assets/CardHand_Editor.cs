using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardHand))]
public class CardHand_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Card Hand Controls", EditorStyles.boldLabel);

		CardHand cardHand = (CardHand)target;
		
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Spawn Cards"))
		{
			var method = typeof(CardHand).GetMethod("SpawnCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			method?.Invoke(cardHand, null);
		}

		if (GUILayout.Button("Update Visuals"))
		{
			var method = typeof(CardHand).GetMethod("UpdateVisuals", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			method?.Invoke(cardHand, null);
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Clear All"))
		{
			var method = typeof(CardHand).GetMethod("ClearAll", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			method?.Invoke(cardHand, null);
		}

		if (GUILayout.Button("Rebuild Layout"))
		{
			if (cardHand.Layout != null)
			{
				cardHand.Layout.RebuildLayout();
			}
		}

		EditorGUILayout.EndHorizontal();
	}
}
