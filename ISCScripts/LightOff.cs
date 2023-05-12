using InSheepsClothing.Events;
using UnityEngine;

namespace InSheepsClothing.Extra
{
	public class LightOff : MonoBehaviour
	{
		private Light light = null;
		private float lightValue = 0.0f;

		private void Awake()
		{
			light = GetComponent<Light>();
			lightValue = light.intensity;

			light.intensity = 0;
		}

		private void OnEnable()
		{
			EventManager.Instance.RoundStart += RoundStart;
		}

		private void OnDisable()
		{
			EventManager.Instance.RoundStart += RoundStart;
		}

		private void RoundStart(object sender)
		{
			light.intensity = lightValue;
		}
	}
}