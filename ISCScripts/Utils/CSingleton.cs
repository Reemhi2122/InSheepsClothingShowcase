using System;

public abstract class CSingleton<T> where T : CSingleton<T>
{
	private static readonly Lazy<T> Lazy =
		new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);

	public static T Instance => Lazy.Value;
}