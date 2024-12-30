using System;
using Animancer;
using Spyro;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public abstract class Interactable : MonoBehaviour
	{
		public bool triggerOnce;
		private DetectionBase detection;
		private bool activeStatus = true;
		public DetectionBase Detector => detection ??= Detection;
		protected abstract DetectionBase Detection { get; }
		public abstract string UIDisplayMessage { get; }
		public abstract void OnInteractCommit();
		public virtual void OnInteractHoverEnter() { }
		public virtual void OnInteractHoverExit() { }

		public void Enable() => activeStatus = true;
		public void Disable() => activeStatus = false;
		public bool IsActive => activeStatus;
		public bool IsDetected { get; set; }


		void OnDrawGizmos()
		{
			Detection.DrawGizmo(this);
		}
	}



}
