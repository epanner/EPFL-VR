using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Teleport : MonoBehaviour
{
	private InputDevice leftController;
	private InputDevice rightController;

	public GameObject player;
	public Lanes lanes;

	private List<GameObject> teleportPads;
	private int currentLane = 1;

	public bool teleportEnabled = false;

	void Start()
	{
		// Get controllers by handedness
		var leftHandDevices = new List<InputDevice>();
		var rightHandDevices = new List<InputDevice>();

		InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
		InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

		if (leftHandDevices.Count > 0)
		{
			leftController = leftHandDevices[0];
			Debug.Log("Left controller found: " + leftController.name);
		}


		if (rightHandDevices.Count > 0)
		{
			rightController = rightHandDevices[0];
			Debug.Log("Right controller found: " + rightController.name);
		}

		if (!leftController.isValid)
			Debug.LogWarning("Left controller not found!");
		if (!rightController.isValid)
			Debug.LogWarning("Right controller not found!");

		teleportPads = lanes.GetTeleportPads();
	}

	void Update()
	{
		if (!teleportEnabled)
			return;

		// Check primary button on left controller
		if (leftController.isValid &&
			leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPressed) &&
			leftPressed)
		{
			Debug.Log("Left primary button (X) pressed");
			TeleportToLane(Mathf.Max(0, currentLane - 1));
		}

		// Check primary button on right controller
		if (rightController.isValid &&
			rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPressed) &&
			rightPressed)
		{
			Debug.Log("Right primary button (A) pressed");
			int nextLane = (currentLane + 1) % teleportPads.Count;
			TeleportToLane(nextLane);
		}
	}

	private void TeleportToLane(int laneIndex)
	{
		Vector3 targetPosition = teleportPads[laneIndex].transform.position;
		player.transform.position = targetPosition;

		currentLane = laneIndex;
	}
}
