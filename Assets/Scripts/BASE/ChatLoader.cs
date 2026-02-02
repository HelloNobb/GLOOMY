using UnityEngine;

public class ChatLoader : MonoBehaviour{
	// 일단 원하는 chat의 jsonfile보내면 랜덤 노드 하나 보냄
	private ChatNode[] chatNodes;
	public ChatNode GetRandomChatNode(TextAsset file) {
		//load
		ChatNodeWrapper chatWrapper = JsonUtility.FromJson<ChatNodeWrapper>(file.text);
		chatNodes = chatWrapper.Chats;
		//return random 1 node 
		if (chatNodes == null || chatNodes.Length == 0){
			Debug.Log("해당 chatnode is empty..");
			return null;
		}
		int randomIndex = UnityEngine.Random.Range(0,chatNodes.Length);
		return chatNodes[randomIndex];
	}
}