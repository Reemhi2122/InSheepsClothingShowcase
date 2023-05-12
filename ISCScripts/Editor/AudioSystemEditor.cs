using System.Collections.Generic;
using System.IO;
using InSheepsClothing.Events;
using UnityEditor;
using UnityEngine;
using USubtitles;

namespace InSheepsClothing.Audio.Editor
{
	[CustomEditor(typeof(AudioSystem))]
	public class AudioSystemEditor : UnityEditor.Editor
	{
		private static int selected = 0;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			AudioSystem audioSystem = (AudioSystem)target;

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

			EditorGUILayout.Separator();

			List<string> options = new List<string>();
			foreach (UAudioClip clip in audioSystem.Clips)
				options.Add(clip.name);

			GUILayout.BeginHorizontal();

			selected = EditorGUILayout.Popup(string.Empty, selected, options.ToArray());

			if (GUILayout.Button("Play AudioClip", myFoldoutStyle))
			{
				if (!Application.isPlaying)
					return;

				PlayAudioClipEventArgs e = new PlayAudioClipEventArgs
				{
					audioClip = audioSystem.Clips[selected],
					immediate = true
				};
				EventManager.Instance.OnPlayAudioClip(this, e);
			}

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			GUILayout.EndHorizontal();

			Rect dropZoneRect = EditorGUILayout.GetControlRect(true, 100);

			Event evt = Event.current;
			GUIStyle dropAreaStyle = new GUIStyle(GUI.skin.button);
			GUI.Box(dropZoneRect, "Drop folder to get AudioClips", dropAreaStyle);

			switch (evt.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!dropZoneRect.Contains(evt.mousePosition))
						break;

					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

					if (evt.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag();

						foreach (Object dragged_object in DragAndDrop.objectReferences)
						{
							string folderPath = AssetDatabase.GetAssetPath(dragged_object);
							if (folderPath != "")
							{
								try
								{
									IEnumerable<string> assetFiles;
									assetFiles = Directory.GetFiles(folderPath);

									foreach (string path in assetFiles)
									{
										if (!path.Contains(".meta"))
										{
											Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
											for (int i = 0; i < objs.Length; i++)
											{
												Object obj = objs[i];
												if (obj != null && obj.GetType() == typeof(InSheepsClothingAudioClip))
												{
													InSheepsClothingAudioClip audioClip = obj as InSheepsClothingAudioClip;
													audioSystem.Clips.Add(audioClip);
												}
											}
										}
									}
									EditorUtility.SetDirty(target);
								}
								catch
								{
									Debug.LogError("Please make sure you drag a full folder to auto-load the switcher.");
								}
							}
						}
					}
					break;
			}

			_ = serializedObject.ApplyModifiedProperties();
		}

		public override bool RequiresConstantRepaint() => true;
	}
}