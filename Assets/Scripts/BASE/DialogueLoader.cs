using System.Collections.Generic;
using UnityEngine;

public class DialogueLoader : MonoBehaviour {
	private DialogueNode[] nodes; // <- dataWrapper 속 dialogueNode[]들
	private Dictionary<string, DialogueNode> dictNodes = new(); // <- node들 id-node 쌍으로 넣기

	public void LoadDialogue(TextAsset jsonFile){ //필수 호출먼저
		//nodes <- wrapper통해 넣기
		DialogueNodes dataWrapper = JsonUtility.FromJson<DialogueNodes>(jsonFile.text);
		nodes = dataWrapper.Dialogues;
		//dictionary <- id-node 쌍 전체
		dictNodes.Clear();
		foreach(DialogueNode node in nodes) {
			if (string.IsNullOrEmpty(node.id)){
				Debug.LogWarning("[DLoader] id없는 노드 스킵");
				continue;
			}
			if (dictNodes.ContainsKey(node.id)){ //id중복시
				Debug.LogWarning($"[DLoader] 중복된 아이디의 노드 존재: {node.id}");
				continue;
			}
			if (node.text == null || node.text.Length == 0){
				node.text = new string[] {""};
			}
			if (node.options == null){
				node.options = System.Array.Empty<DialogueOption>();
			}

			dictNodes.Add(node.id, node);
		}
	}
	public DialogueNode GetNodeById(string id) {
		return dictNodes.TryGetValue(id, out DialogueNode node) ? node : null;
	}
	public Dictionary<string, DialogueNode> GetNodesDict(){
		return dictNodes;
	}
	public string GetRandomLine(DialogueNode node){
        if (node == null){
            Debug.Log("현재 노드가 null");
            return null;
        }
        int randomIndex = UnityEngine.Random.Range(0,node.text.Length);
		return node.text[randomIndex];
    }
	public DialogueNode GetRandomNode(){
		if (nodes == null || nodes.Length == 0){
			Debug.Log("해당 chatnode is empty..");
			return null;
		}
		int randomIndex = UnityEngine.Random.Range(0,nodes.Length);
		return nodes[randomIndex];
	}
}

// public static class DialogueParser {
// 	public static Dictionary<string, DialogueNode> LoadFromJson(TextAsset jsonFile){
// 		DialogueDataWrapper wrapper = JsonUtility.FromJson<DialogueDataWrapper>(jsonFile.text);
// 		Dictionary<string, DialogueNode> nodesDict = new();
// 		foreach(DialogueNode node in wrapper.Dialogues){
// 			nodesDict.Add(node.id, node);
// 		}
// 		return nodesDict;
// 	}
// }