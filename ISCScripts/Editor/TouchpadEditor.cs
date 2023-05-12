using UnityEditor;
using UnityEngine;

namespace InSheepsClothing.Interaction
{
	[CustomEditor(typeof(Touchpad), true)]
	public class TouchpadEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			Touchpad touchpad = (Touchpad)target;

			if (GUILayout.Button("Press"))
				touchpad.Press();
		}
	}
}