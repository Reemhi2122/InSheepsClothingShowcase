using System.Collections.Generic;
using UnityEngine;

namespace InSheepsClothing.Extra
{
	public enum RandomizeSetting
	{
		RandomizeSetting_Ranged,
		RandomizeSetting_Presets,
		RandomizeSetting_Either
	}

	public enum SpawnSetting
	{
		SpawnSetting_Random,
		SpawnSetting_None
	}

	public class RandomizeLocation : MonoBehaviour
	{
		[SerializeField]
		private bool self = false;
		public bool Self => self;

		[SerializeField]
		private List<GameObject> objects = new List<GameObject>();

		[Range(0f, 1f)]
		[SerializeField]
		private float spawnChance = 0.0f;

		[SerializeField]
		private SpawnSetting scaleSetting = SpawnSetting.SpawnSetting_Random;
		public SpawnSetting ScaleSetting => scaleSetting;

		[SerializeField]
		private RandomizeSetting scaleMode = RandomizeSetting.RandomizeSetting_Ranged;
		public RandomizeSetting ScaleMode => scaleMode;

		[SerializeField]
		private Vector3 minScale = Vector3.one;

		[SerializeField]
		private Vector3 maxScale = Vector3.one;

		[SerializeField]
		private List<Vector3> scalePresets = new List<Vector3>();

		[SerializeField]
		private SpawnSetting rotationSetting = SpawnSetting.SpawnSetting_Random;
		public SpawnSetting RotationSetting => rotationSetting;

		[SerializeField]
		private RandomizeSetting rotationMode = RandomizeSetting.RandomizeSetting_Ranged;
		public RandomizeSetting RotationMode => rotationMode;

		[SerializeField]
		private Vector3 minRotation = Vector3.zero;

		[SerializeField]
		private Vector3 maxRotation = Vector3.zero;

		[SerializeField]
		private List<Quaternion> rotationPresets = new List<Quaternion>();

		[SerializeField]
		private bool parentObject = false;

		private Vector3 ChooseScaleRanged()
		{
			return new Vector3(
				Random.Range(minScale.x, maxScale.x),
				Random.Range(minScale.y, maxScale.y),
				Random.Range(minScale.z, maxScale.z)
			);
		}

		private Vector3 ChooseScalePresets() => scalePresets.Count > 0 ? scalePresets[Random.Range(0, scalePresets.Count)] : ChooseScaleRanged();

		private Vector3 GetScale()
		{
			if (scaleSetting == SpawnSetting.SpawnSetting_None)
				return transform.localScale;

			switch (scaleMode)
			{
				case RandomizeSetting.RandomizeSetting_Ranged:
					return ChooseScaleRanged();
				case RandomizeSetting.RandomizeSetting_Presets:
					return ChooseScalePresets();
				case RandomizeSetting.RandomizeSetting_Either:
					return Random.value > 0.5f ? ChooseScaleRanged() : ChooseScalePresets();
			}

			return Vector3.one;
		}

		private Quaternion ChooseRotationRanged()
		{
			return Quaternion.Euler(
				Random.Range(minRotation.x, maxRotation.x),
				Random.Range(minRotation.y, maxRotation.y),
				Random.Range(minRotation.z, maxRotation.z)
			);
		}

		private Quaternion ChooseRotationPresets() => rotationPresets.Count > 0 ? rotationPresets[Random.Range(0, rotationPresets.Count)] : ChooseRotationRanged();

		private Quaternion GetRotation()
		{
			if (rotationSetting == SpawnSetting.SpawnSetting_None)
				return transform.rotation;

			switch (scaleMode)
			{
				case RandomizeSetting.RandomizeSetting_Ranged:
					return ChooseRotationRanged();
				case RandomizeSetting.RandomizeSetting_Presets:
					return ChooseRotationPresets();
				case RandomizeSetting.RandomizeSetting_Either:
					return Random.value > 0.5f ? ChooseRotationRanged() : ChooseRotationPresets();
			}

			return Quaternion.Euler(0, 0, 0);
		}

		private bool CanSpawn() => Random.value < spawnChance;

		private void Start()
		{
			if (!CanSpawn())
				return;

			if (!self)
			{
				if (objects.Count == 0)
					return;

				GameObject obj = Instantiate(objects[Random.Range(0, objects.Count)], parentObject ? this.transform : null);
				obj.transform.position = transform.position;
				obj.transform.localScale = GetScale();
				obj.transform.rotation = GetRotation();
			}
			else
			{
				transform.localScale = GetScale();
				transform.rotation = GetRotation();
			}
		}
	}
}