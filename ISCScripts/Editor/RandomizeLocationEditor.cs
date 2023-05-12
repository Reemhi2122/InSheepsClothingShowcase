using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace InSheepsClothing.Extra
{
	[CustomEditor(typeof(RandomizeLocation)), CanEditMultipleObjects]
	public class RandomizeLocationEditor : Editor
	{
		private static bool showRotationSettings;
		private static bool showScaleSettings;

		SerializedProperty objects = null, spawnChance = null, scaleSetting = null, scaleMode = null, minScale = null, maxScale = null, scalePresets = null, rotationSetting = null, rotationMode = null, minRotation = null, maxRotation = null, parentObject = null, self = null;

		void OnEnable()
		{
			objects = serializedObject.FindProperty("objects");
			spawnChance = serializedObject.FindProperty("spawnChance");
			scaleSetting = serializedObject.FindProperty("scaleSetting");
			scaleMode = serializedObject.FindProperty("_scaleMode");
			minScale = serializedObject.FindProperty("minScale");
			maxScale = serializedObject.FindProperty("maxScale");
			scalePresets = serializedObject.FindProperty("scalePresets");
			rotationSetting = serializedObject.FindProperty("rotationSetting");
			rotationMode = serializedObject.FindProperty("rotationMode");
			minRotation = serializedObject.FindProperty("minRotation");
			maxRotation = serializedObject.FindProperty("maxRotation");
			parentObject = serializedObject.FindProperty("parentObject");
			self = serializedObject.FindProperty("self");
		}

		public override void OnInspectorGUI()
		{
			RandomizeLocation randomizeLocation = (RandomizeLocation)target;

			GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.miniButton);
			myFoldoutStyle.fontStyle = FontStyle.Bold;
			myFoldoutStyle.fontSize = 12;
			Color myStyleColor = Color.white;
			myFoldoutStyle.normal.textColor = myStyleColor;
			myFoldoutStyle.onNormal.textColor = myStyleColor;
			myFoldoutStyle.hover.textColor = myStyleColor;
			myFoldoutStyle.onHover.textColor = myStyleColor;
			myFoldoutStyle.focused.textColor = myStyleColor;
			myFoldoutStyle.onFocused.textColor = myStyleColor;
			myFoldoutStyle.active.textColor = myStyleColor;
			myFoldoutStyle.onActive.textColor = myStyleColor;

			_ = EditorGUILayout.PropertyField(self, new GUIContent("Self"));

			if (!randomizeLocation.Self)
				_ = EditorGUILayout.PropertyField(objects, new GUIContent("Objects"));
			_ = EditorGUILayout.PropertyField(spawnChance, new GUIContent("Spawn Chance"));

			EditorGUILayout.Separator();

			_ = EditorGUILayout.PropertyField(scaleSetting, new GUIContent("Scale Setting"));

			if (randomizeLocation.ScaleSetting == SpawnSetting.SpawnSetting_Random)
			{
				showScaleSettings = EditorGUILayout.Foldout(showScaleSettings, "Scale Settings");
				if (showScaleSettings)
				{
					EditorGUI.indentLevel++;
					_ = EditorGUILayout.PropertyField(scaleMode, new GUIContent("Scale Mode"));

					if (randomizeLocation.ScaleMode == RandomizeSetting.RandomizeSetting_Ranged || randomizeLocation.ScaleMode == RandomizeSetting.RandomizeSetting_Either)
					{
						EditorGUI.indentLevel++;
						_ = EditorGUILayout.PropertyField(minScale, new GUIContent("Min Scale"));
						_ = EditorGUILayout.PropertyField(maxScale, new GUIContent("Max Scale"));
						EditorGUI.indentLevel--;
					}
					if (randomizeLocation.ScaleMode == RandomizeSetting.RandomizeSetting_Presets || randomizeLocation.ScaleMode == RandomizeSetting.RandomizeSetting_Either)
					{
						EditorGUI.indentLevel++;
						_ = EditorGUILayout.PropertyField(scalePresets, new GUIContent("Presets"));
						EditorGUI.indentLevel--;
					}
					EditorGUI.indentLevel--;
				}
			}

			EditorGUILayout.Separator();

			_ = EditorGUILayout.PropertyField(rotationSetting, new GUIContent("Rotation Setting"));

			if (randomizeLocation.RotationSetting == SpawnSetting.SpawnSetting_Random)
			{
				showRotationSettings = EditorGUILayout.Foldout(showRotationSettings, "Rotation Settings");
				if (showRotationSettings)
				{
					EditorGUI.indentLevel++;
					_ = EditorGUILayout.PropertyField(rotationMode, new GUIContent("Rotation Mode"));

					if (randomizeLocation.RotationMode == RandomizeSetting.RandomizeSetting_Ranged || randomizeLocation.RotationMode == RandomizeSetting.RandomizeSetting_Either)
					{
						EditorGUI.indentLevel++;
						_ = EditorGUILayout.PropertyField(minRotation, new GUIContent("Min Rotation"));
						_ = EditorGUILayout.PropertyField(maxRotation, new GUIContent("Max Rotation"));
						EditorGUI.indentLevel--;
					}
					if (randomizeLocation.RotationMode == RandomizeSetting.RandomizeSetting_Presets || randomizeLocation.RotationMode == RandomizeSetting.RandomizeSetting_Either)
					{
						EditorGUI.indentLevel++;
						_ = EditorGUILayout.PropertyField(scalePresets, new GUIContent("Presets"));
						EditorGUI.indentLevel--;
					}
					EditorGUI.indentLevel--;
				}
			}

			EditorGUILayout.Separator();

			_ = EditorGUILayout.PropertyField(parentObject, new GUIContent("Parent Object"));

			EditorGUILayout.Separator();

			_ = serializedObject.ApplyModifiedProperties();
		}

		protected virtual void OnSceneGUI()
		{
		}

		public override bool RequiresConstantRepaint() => true;
	}
}