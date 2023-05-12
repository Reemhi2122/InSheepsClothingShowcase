using UnityEditor;
using UnityEngine;
using VRTools.Interaction.Editor;

namespace InSheepsClothing.Interaction
{
	[CustomEditor(typeof(Camera), true)]
	public class CameraEditor : GrabbableObjectEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			Camera camera = (Camera)target;

			if (GUILayout.Button("Take Picture"))
				camera.TakePicture(null);

			_ = serializedObject.ApplyModifiedProperties();
		}
	}
}