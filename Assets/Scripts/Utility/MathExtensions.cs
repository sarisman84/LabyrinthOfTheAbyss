using UnityEngine;

namespace lota.utility
{
	public static class MathExtensions
	{
		public static Vector3 ToVector3XZ(this Vector2 original)
		{
			return new Vector3(original.x, 0, original.y);
		}
	}
}