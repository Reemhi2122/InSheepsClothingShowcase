using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InSheepsClothing
{
	[CreateAssetMenu()]
	public class Presentation : ScriptableObject
	{
		[SerializeField]
		private Sprite[] slides = null;
		public Sprite[] Slides => slides;

		[SerializeField]
		private Color color = Color.white;
		public Color Color => color;
	}
}