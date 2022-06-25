using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public class ButtonSelectionController : MonoBehaviour
{
	public void Start()
	{
		this.m_scrollRect = base.GetComponent<ScrollRect>();
		this.m_buttons = base.GetComponentsInChildren<Button>();
		this.m_buttons[this.m_index].Select();
		this.m_HorizontalPosition = 1f - (float)this.m_index / (float)(this.m_buttons.Length - 1);
	}

	public void Update()
	{
		this.m_Left = Input.GetKeyDown(KeyCode.LeftArrow);
		this.m_Right = Input.GetKeyDown(KeyCode.RightArrow);
		if (this.m_Left ^ this.m_Right)
		{
			if (this.m_Left)
			{
				this.m_index = Mathf.Clamp(this.m_index - 1, 0, this.m_buttons.Length - 1);
			}
			else
			{
				this.m_index = Mathf.Clamp(this.m_index + 1, 0, this.m_buttons.Length - 1);
			}
			this.m_buttons[this.m_index].Select();
			this.m_HorizontalPosition = 1f - (float)this.m_index / (float)(this.m_buttons.Length - 1);
		}
		this.m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(this.m_scrollRect.verticalNormalizedPosition, this.m_HorizontalPosition, Time.deltaTime / this.m_lerpTime);
	}

	[SerializeField]
	private float m_lerpTime;
	private ScrollRect m_scrollRect;
	private Button[] m_buttons;
	private int m_index;
	private float m_HorizontalPosition;
	private bool m_Left;
	private bool m_Right;
}