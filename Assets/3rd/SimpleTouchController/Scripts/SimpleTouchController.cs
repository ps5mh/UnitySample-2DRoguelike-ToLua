using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


public class SimpleTouchController : MonoBehaviour {

	// PUBLIC
	public delegate void TouchDelegate(Vector2 value);
	public event TouchDelegate TouchEvent;

	public delegate void TouchStateDelegate(bool touchPresent);
	public event TouchStateDelegate TouchStateEvent;
	private Vector2 __AdditionMovement;

	private int INVALID_TOUCH_ID = -1024;

	// PRIVATE
	[SerializeField]
	private RectTransform joystickArea;
	public int PointerID = 0;
	private Vector2 movementVector;

	public Vector2 GetTouchPosition
	{
		get { return movementVector;}
	}

	public Vector2 AdditionMovement
	{
		get { return __AdditionMovement;}
	}


	public void BeginDrag(BaseEventData b)
	{
		var touch = b as PointerEventData;
		PointerID = touch.pointerId;
		if(TouchStateEvent != null)
			TouchStateEvent(PointerID != INVALID_TOUCH_ID);
	}

	public void EndDrag()
	{
		PointerID = INVALID_TOUCH_ID;
		movementVector = joystickArea.anchoredPosition = Vector2.zero;

		if(TouchStateEvent != null)
			TouchStateEvent(PointerID != INVALID_TOUCH_ID);

	}

	public void Drag(BaseEventData b)
	{
		var touch = b as PointerEventData;
		__AdditionMovement.Set(touch.position.x, touch.position.y);
	}

	public void OnValueChanged(Vector2 value)
	{
		if(PointerID != INVALID_TOUCH_ID)
		{
			// convert the value between 1 0 to -1 +1
			movementVector.x = ((1 - value.x) - 0.5f) * 2f;
			movementVector.y = ((1 - value.y) - 0.5f) * 2f;

			if(TouchEvent != null)
			{
				TouchEvent(movementVector);
			}
		}

	}

}
