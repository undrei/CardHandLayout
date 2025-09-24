using UnityEngine;

[CreateAssetMenu(fileName = "NewCardHandConfig", menuName = "Config/CardHandConfig")]
public class CardHandConfig : ScriptableObject
{
	[SerializeField]
	private AnimationCurve curvePositioning;
	[SerializeField]
	private AnimationCurve curveRotation;
	
	[Header("Position setup")]
	[SerializeField] 
	private float cardSpacing = 0f;
	[SerializeField] 
	private float verticalOffsetCoef = 5f;
	
	[Header("Rotation setup")]
	[SerializeField]
	private float rotationOffset = 0f;
	[SerializeField] 
	private float rotationCoef = 2f;

	public AnimationCurve CurvePositioning => curvePositioning;
	public AnimationCurve CurveRotation => curveRotation;
	public float CardSpacing => cardSpacing;
	public float VerticalOffsetCoef => verticalOffsetCoef;
	public float RotationOffset => rotationOffset;
	public float RotationCoef => rotationCoef;
}