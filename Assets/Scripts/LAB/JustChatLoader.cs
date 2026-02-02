using UnityEngine;
public class JustChatLoader : MonoBehaviour{

	private ChatNode[] chatNodes;

	public void LoadChat(TextAsset jsonFile){
		ChatNodeWrapper chatWrapper = JsonUtility.FromJson<ChatNodeWrapper>(jsonFile.text);
		chatNodes = chatWrapper.Chats;
	}
	public ChatNode GetRandomChatNode(TextAsset file) {
		//load
		LoadChat(file);
		//return random 1 node 
		if (chatNodes == null || chatNodes.Length == 0){
			Debug.Log("해당 chatnode is empty..");
			return null;
		}
		int randomIndex = UnityEngine.Random.Range(0,chatNodes.Length);
		return chatNodes[randomIndex];
	}
}