using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatCatalog : ScriptableObject
{
	[Serializable]
	public class Entry{
		public string key;
		public TextAsset json;
	}
	public List<Entry> entries = new();
}