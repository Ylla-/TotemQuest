using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {
	public static int rabbitRunState, rabbitIdleState, rabbitJumpState, rabbitDashState, rabbitTransitionState;
	public static int SpeedFloat, DashBool, GroundedBool, GlideBool, TotemInt;

	void Awake () {
		rabbitRunState  = Animator.StringToHash("Base Layer.BunnyRun");
		rabbitIdleState = Animator.StringToHash("Base Layer.BunnyIdle");
		rabbitJumpState = Animator.StringToHash("Base Layer.BunnyJump");
		rabbitDashState = Animator.StringToHash("Base Layer.Dash");
		rabbitTransitionState = Animator.StringToHash("Base Layer.BunnyTransition");

		SpeedFloat = Animator.StringToHash("Speed");
		DashBool = Animator.StringToHash("Dash");
		GroundedBool = Animator.StringToHash("Grounded");
		GlideBool = Animator.StringToHash("Glide");
		TotemInt = Animator.StringToHash("Totem");



	}

}
