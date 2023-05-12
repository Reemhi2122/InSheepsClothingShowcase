using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance = null;

	[SerializeField]
	private bool dontDestroyOnLoad = false;

	protected void Awake()
	{
		if (Instance == null)
			Instance = gameObject.GetComponent<T>();
		else if (Instance.GetInstanceID() != GetInstanceID())
			Destroy(gameObject);

		if (dontDestroyOnLoad)
			DontDestroyOnLoad(gameObject);
	}
}