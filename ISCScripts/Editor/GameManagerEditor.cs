using UnityEditor;
using UnityEngine;

namespace InSheepsClothing.Interaction
{
	[CustomEditor(typeof(GameManager), true)]
	public class GameManagerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GameManager gameManager = (GameManager)target;
		}
	}
}