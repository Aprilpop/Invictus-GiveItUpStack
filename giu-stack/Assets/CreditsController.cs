using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
	public ScrollRect scroll;
	public float speed = 12.0f;

	float startTime;
	float endTime;
	bool onDrag = false;

	void OnEnable()
	{
		ScrollInitializer();
	}

	public void ScrollInitializer()
	{
		scroll.verticalNormalizedPosition = 1.0f;
		startTime = Time.unscaledTime;
		endTime = startTime + speed;
	}

	void Update()
	{ 
		if (!onDrag)
		{
			scroll.verticalNormalizedPosition = CarculateScrollValue();

			if (scroll.verticalNormalizedPosition <= 0.01f)
			{
				ScrollInitializer();
			}
		}
	}

	float CarculateScrollValue()
	{
		return 1.0f - Mathf.InverseLerp(startTime, endTime, Time.unscaledTime);
	}

	float CarculateScrollTime()
	{
		return speed - Mathf.Lerp(0.0f, speed, scroll.verticalNormalizedPosition);
	}

	public void OnDragBegin()
	{
		onDrag = true;
	}

	public void OnDragEnd()
	{
		float scrollTime = CarculateScrollTime();
		startTime = Time.unscaledTime - scrollTime;
		endTime = startTime + speed;
		onDrag = false;
	}
}