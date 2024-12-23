using DG.Tweening;
using lota.gameplay.interactions;
using lota.generated.input;
using lota.systemic;
using UnityEngine;

namespace lota.gameplay.interactions
{
	public class MovingEffect : Interactable
	{
		public float detectionRadius;

		public override string UIDisplayMessage => $"Press <input:{InputService.IDToString(InputActionID.Player_Interact)}> to move {gameObject.name}!";

		protected override DetectionBase Detection => new SphereDetector(detectionRadius);


		private Vector3 spawnPos;

		void Awake()
		{
			spawnPos = transform.position;
		}
		public override void OnInteractCommit()
		{
			transform.DOMove(spawnPos + (Vector3.up * 10), 3.0f).OnComplete(() =>
			{
				transform.DOMove(spawnPos, 3.0f).SetEase(Ease.Linear);
			}).SetEase(Ease.Linear);
		}

	}
}
