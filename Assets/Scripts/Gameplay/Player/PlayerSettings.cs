using System.Collections.Specialized;
using UnityEngine;

namespace lota.gameplay.player
{
	[CreateAssetMenu(menuName = "LabyrinthOfTheAbyss/PlayerSettings")]
	public class PlayerSettings : ScriptableObject
	{
		[Header("Horizontal Movement")]
		[Range(0.0f, 500.0f)] public float movementSpeed;
		[Range(1.0f, 10.0f)] public float sprintModifier;
		[Range(0.1f, 2.0f)] public float acceleration;
		[Range(0.1f, 2.0f)] public float decceleration;
		[Range(0.0f, 1000.0f)] public float rotationSpeed;

		[Header("Vertical Movement")]
		[Range(0.0f, 100.0f)] public float jumpHeight;
		public float gravity;
		[Range(1.0f, 5.0f)] public float fallMultiplier;
		[Range(1.0f, 5.0f)] public float lowFallMultiplier;
		public Vector3 gravityDirection = Vector3.down;


		public void RenderGizmos(Vector3 origin, Vector3 upDirection)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(origin, upDirection * jumpHeight);
			Gizmos.DrawSphere(origin + upDirection * jumpHeight, 0.25f);
			Gizmos.DrawSphere(origin, 0.25f);
		}
	}
}
