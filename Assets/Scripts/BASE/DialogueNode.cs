[System.Serializable]
public class DialogueNodes{
	public DialogueNode[] Dialogues; //DialogueNode객체 배열 담길 예정
}

[System.Serializable]
public class DialogueNode
{
	public string id;//
	public string[] text;
	public string speaker;
	public string face;
	public string command;
	public DialogueOption[] options;
	public string next;//
}
[System.Serializable]
public class DialogueOption
{
	public string text;
	public string next;
}