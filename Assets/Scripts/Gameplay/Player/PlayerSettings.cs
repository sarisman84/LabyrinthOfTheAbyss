using System.Collections.Specialized;
using UnityEngine;

namespace lota.gameplay.player
{
	[CreateAssetMenu(menuName = "LabyrinthOfTheAbyss/PlayerSettings")]
	public class PlayerSettings : ScriptableObject
	{
		[Range(0.0f, 500.0f)] public float movementSpeed;
		[Range(1.0f, 10.0f)] public float sprintModifier;
		[Range(0.1f, 2.0f)] public float acceleration;
		[Range(0.1f, 2.0f)] public float decceleration;
		public float jumpHeight;
		public float gravity;
		[Range(1.0f, 5.0f)] public float fallMultiplier;
		[Range(1.0f, 5.0f)] public float lowFallMultiplier;
		public Vector3 gravityDirection = Vector3.down;


		public void RenderGizmos(Vector3 origin)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(origin, Vector3.up * jumpHeight);
			Gizmos.DrawSphere(origin + Vector3.up * jumpHeight, 0.25f);
			Gizmos.DrawSphere(origin, 0.25f);
		}
	}
}
