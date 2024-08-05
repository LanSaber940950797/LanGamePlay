using GameLogic.Battle;
using TEngine;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameLogic
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public Vector3 targetDirection;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		private int test = 0;
		
		public const bool IsBattle = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (IsBattle)
			{
				//var dir = value.Get<Vector2>();
				//GetBattleInputControl().MoveDir = value.Get<Vector3>();
				return;
			}
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (IsBattle)
			{
				return;
			}
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (IsBattle)
			{
				return;
			}
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void TargetDirectionInput(Vector3 newTargetDirection)
		{
			targetDirection = newTargetDirection;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private InputControlComponent GetBattleInputControl()
		{
			var room = GameHeler.Room();
			return room.GetComponent<InputControlComponent>();
		}
	}
	
}