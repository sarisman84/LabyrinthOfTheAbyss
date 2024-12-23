using System;
using System.Collections.Generic;
using Spyro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace lota.gameplay.interactions
{


	public abstract class DetectionBase
	{
		public abstract bool TestDetection(Interactable interactable, InteractionController controller);
		public abstract void DrawGizmo(Interactable interactable);

		protected bool IsInteractableInViewOfCamera(Camera camera, Interactable interactable)
		{
			// Convert the object's world position to viewport coordinates
			Vector3 viewportPos = camera.WorldToViewportPoint(interactable.transform.position);
			// Check if the object is within the viewport boundaries
			bool isInCameraView = viewportPos.x > 0
				 && viewportPos.x < 1
				 && viewportPos.y > 0
				 && viewportPos.y < 1
				 && viewportPos.z > 0; // Ensure the object is in front of the camera

			return isInCameraView;

		}


	}
	public class SphereDetector : DetectionBase
	{
		private float radius;
		public SphereDetector(float detectionRadius)
		{
			this.radius = detectionRadius;
		}

		public override void DrawGizmo(Interactable interactable)
		{
			Gizmos.color = interactable.IsDetected ? Color.green : Color.yellow;
			Gizmos.DrawWireSphere(interactable.transform.position, radius);
		}

		public override bool TestDetection(Interactable interactable, InteractionController controller)
		{
			var dist = Vector3.Distance(controller.transform.position, interactable.transform.position);
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

		public override void DrawGizmo(Interactable interactable)
		{
			Gizmos.color = interactable.IsDetected ? Color.green : Color.yellow;
			Gizmos.DrawWireCube(interactable.transform.position + center, size);
		}

		public override bool TestDetection(Interactable interactable, InteractionController controller)
		{
			var detectionBounds = new Bounds(interactable.transform.position + center, size);
			var targetBounds = new Bounds(controller.transform.position + controller.CollisionBounds.center, controller.CollisionBounds.size);
			return detectionBounds.Intersects(targetBounds) && IsInteractableInViewOfCamera(controller.MainCamera, interactable);
		}
	}


}






