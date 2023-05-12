using System;
using UnityEngine;

namespace InSheepsClothing
{
	[System.Serializable]
    public class Timer
	{
		[SerializeField]
		private float time = 0.0f;
		[SerializeField]
		private float currentTime = 0.0f;
		public Action TickAction;

		public bool Enabled = false;

		public void Reset() => currentTime = 0.0f;

		public void SetTime(float minTime)
		{
			time = minTime;
		}

		public float GetTime() => time;
		public float GetElapsedTime() => currentTime;

		public void Update()
		{
			if (!Enabled)
				return;

			currentTime += UnityEngine.Time.deltaTime;
			if (currentTime >= time)
			{
				Reset();
				TickAction.Invoke();
			}
		}
    }
}