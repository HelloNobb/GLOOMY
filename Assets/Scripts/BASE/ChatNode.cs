[System.Serializable]
public class ChatNodeWrapper 
{
	public ChatNode[] Chats;
}

[System.Serializable]
public class ChatNode
{
	public string[] text;
	public string face;
}