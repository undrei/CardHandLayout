using UnityEngine;

[CreateAssetMenu(fileName = "new_cc_config", menuName = "Config/CardContainerConfig")]
public class CardContainerConfig : ScriptableObject
{
	[SerializeField]
	private AnimationCurve curvePositioning;
	[SerializeField]
	private AnimationCurve curveRotation;
	
	[Header("Position setup")]
	[SerializeField] 
	private float cardSpacing = 0f;
	[SerializeField]
	private float cardPlaceWidth = 40f;
	[SerializeField] 
	private float verticalOffsetCoef = 5f;
	[SerializeField]
	private float cardHoverOffset = 5f;
	
	[Header("Rotation setup")]
	[SerializeField]
	private float rotationOffset = 0f;
	[SerializeField] 
	private float rotationCoef = 2f;

	public AnimationCurve CurvePositioning => curvePositioning;
	public AnimationCurve CurveRotation => curveRotation;
	public float CardSpacing => cardSpacing;
	public float CardPlaceWidth => cardPlaceWidth;
	public float VerticalOffsetCoef => verticalOffsetCoef;
	public float RotationOffset => rotationOffset;
	public float RotationCoef => rotationCoef;
	public float CardHoverOffset => cardHoverOffset;
}