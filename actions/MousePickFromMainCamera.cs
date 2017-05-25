// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
// Changes by Dumb Game Dev
// www.dumbgamedev.com

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Custom")]
	[Tooltip("Perform an advanced mouse pick action which also calculates the direction and heading to the clicked gameobject and the main camera")]
	public class MousePickFromMainCamera : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Set the length of the ray to cast from the Main Camera.")]
		public FsmFloat rayDistance = 100f;

		[UIHint(UIHint.Variable)]
		[Tooltip("Set Bool variable true if an object was picked, false if not.")]
		public FsmBool storeDidPickObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the picked GameObject.")]
		public FsmGameObject storeGameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the point of contact.")]
		public FsmVector3 storePoint;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the normal at the point of contact.")]
		public FsmVector3 storeNormal;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the distance to the point of contact.")]
		public FsmFloat storeDistance;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the position to the point of contact.")]
		public FsmVector3 storePosition;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the direction between the mouse and clicked object and the main camera")]
		public FsmVector3 storeDirection;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the heading between the mouse and clicked object and the main camera.")]
		public FsmVector3 storeHeading;

		[UIHint(UIHint.Layer)]	
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			rayDistance = 100f;
			storeDidPickObject = null;
			storeDistance = null;
			storeDirection = null;
			storeGameObject = null;
			storeHeading = null;
			storePoint = null;
			storeNormal = null;
			storeDistance = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoMousePick();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMousePick();
		}

		private void DoMousePick()
		{
			var hitInfo = ActionHelpers.MousePick(rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));

			var didPick = hitInfo.collider != null;
			storeDidPickObject.Value = didPick;

			if (didPick)
			{
				storeGameObject.Value = hitInfo.collider.gameObject;
				storeDistance.Value = hitInfo.distance;
				storePoint.Value = hitInfo.point;
				storeNormal.Value = hitInfo.normal;

				// calculate direction and heading (normalized by distance)

				storePosition.Value = Camera.main.transform.position;
				var _gameObject = storeGameObject.Value;


				storeDirection.Value = _gameObject.transform.position - storePosition.Value;
				storeHeading.Value = storeDirection.Value / storeDistance.Value;

			}
			else
			{
				Finish();

			}

		}
	}
}