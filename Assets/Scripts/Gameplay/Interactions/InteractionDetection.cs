using System.Collections.Generic;
using Spyro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace lota.gameplay.interactions
{


	public abstract class DetectionBase
	{
		public abstract bool TestDetection(IInteractable interactable, InteractionController controller);
		public abstract void DrawGizmo(IInteractable interactable, int result);


		protected bool IsInteractableInViewOfCamera(Camera camera, IInteractable interactable)
		{
			// Convert the object's world position to viewport coordinates
			Vector3 viewportPos = camera.WorldToViewportPoint(interactable.GetOwner().transform.position);
			// Check if the object is within the viewport boundaries
			return viewportPos.x > 0
				 && viewportPos.x < 1
				 && viewportPos.y > 0
				 && viewportPos.y < 1
				 && viewportPos.z > 0; // Ensure the object is in front of the camera

		}

	}
	public class SphereDetector : DetectionBase
	{
		private float radius;
		public SphereDetector(float detectionRadius)
		{
			this.radius = detectionRadius;
		}
		public override void DrawGizmo(IInteractable interactable, int result)
		{
			Gizmos.color = result == IInteractable.HOVERSTATE_ENTERED ? Color.green : Color.yellow;
			Gizmos.DrawWireSphere(interactable.GetOwner().transform.position, radius);
		}
		public override bool TestDetection(IInteractable interactable, InteractionController controller)
		{

			var dist = Vector3.Distance(controller.transform.position, interactable.GetOwner().transform.position);
			return dist <= radius && IsInteractableInViewOfCamera(controller.MainCamera, interactable);
		}
	}

	public class BoundsDetector : DetectionBase
	{
		private Vector3 center, size;
		public BoundsDetector(Vector3 boundsCenter, Vector3 boundsSize)
		{
			center = boundsCenter;
			size = boundsSize;
		}
		public override void DrawGizmo(IInteractable interactable, int result)
		{
			Gizmos.color = result == IInteractable.HOVERSTATE_ENTERED ? Color.green : Color.yellow;
			Gizmos.DrawWireCube(interactable.GetOwner().transform.position + center, size);
		}

		public override bool TestDetection(IInteractable interactable, InteractionController controller)
		{
			var detectionBounds = new Bounds(interactable.GetOwner().transform.position + center, size);
			return detectionBounds.Intersects(controller.CollisionBounds);
		}
	}


}






