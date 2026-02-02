using UnityEngine;

[CreateAssetMenu(fileName ="AppConfig",menuName ="Gloomy/config")]
public class AppConfig : ScriptableObject
{
	[Header("Typing")]
	[Tooltip("chat typing speed(sec)")]
	[Range(0.001f, 0.1f)]
	public float TYPE_SPEED = 0.02f;
}