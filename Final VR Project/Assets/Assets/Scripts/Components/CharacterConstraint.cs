using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterConstraint : MonoBehaviour {

	PlayerController bngController;
	CharacterController character;

	void Awake() {
		character = GetComponent<CharacterController>();
		bngController = transform.GetComponentInParent<PlayerController>();
	}

	private void Update() {
		CheckCharacterCollisionMove();
	}

	public virtual void CheckCharacterCollisionMove() {

		var initialCameraRigPosition = bngController.CameraRig.transform.position;
		var cameraPosition = bngController.CenterEyeAnchor.position;
		var delta = cameraPosition - transform.position;

		// Ignore Y position
		delta.y = 0;

		// Move Character Controller and Camera Rig to Camera's delta
		if (delta.magnitude > 0) {
			character.Move(delta);

			// Move Camera Rig back into position
			bngController.CameraRig.transform.position = initialCameraRigPosition;
		}
	}
}