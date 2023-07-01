using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")] public Vector2 myMove;
        public Vector2 myLook;
        public bool myJump;
        public bool mySprint;

        [Header("Movement Settings")] public bool myAnalogMovement;

        [Header("Mouse Cursor Settings")] public bool myCursorLocked = true;
        public bool myCursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
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
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


        public void moveInput(Vector2 theNewMoveDirection)
        {
            myMove = theNewMoveDirection;
        }

        public void lookInput(Vector2 theNewLookDirection)
        {
            myLook = theNewLookDirection;
        }

        public void jumpInput(bool theNewJumpState)
        {
            myJump = theNewJumpState;
        }

        public void sprintInput(bool theNewSprintState)
        {
            mySprint = theNewSprintState;
        }

        private void OnApplicationFocus(bool theHasFocus)
        {
            setCursorState(myCursorLocked);
        }

        private void setCursorState(bool theNewState)
        {
            Cursor.lockState = theNewState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}