using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InSheepsClothing.Extra
{
	public class RandomizeMaterial : MonoBehaviour
	{
		[SerializeField]
		private List<Material> materials = new List<Material>();

		[SerializeField]
		private List<Renderer> renderers = new List<Renderer>();

		[SerializeField]
		private RandomizeMaterial dependentOn = null;

		private Material chosenMaterial = null;
		public Material ChosenMaterial => chosenMaterial;

		private void Start()
		{
			if (materials.Count == 0)
				return;

			StartCoroutine(SetMaterial());
		}

		private IEnumerator SetMaterial()
		{

			if (dependentOn)
			{
				while (dependentOn.ChosenMaterial == null)
				{
					yield return null;
				}
				chosenMaterial = dependentOn.ChosenMaterial;
			}
			else
				chosenMaterial = materials[Random.Range(0, materials.Count)];

			foreach (Renderer renderer in renderers)
			{
				Material[] materials = renderer.sharedMaterials;
				for (int i = 0; i < materials.Length; i++)
					materials[i] = chosenMaterial;
				renderer.sharedMaterials = materials;
			}
		}
	}
}