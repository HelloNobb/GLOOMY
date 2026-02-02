using UnityEngine;
using UnityEngine.UI;

public class LabSecretPanel : MonoBehaviour
{
	public Button cancelBttn;
	public Button submitBttn;

	void Start()
	{
		gameObject.SetActive(false);
	}
	void OnEnable()
	{
		LabManager.instance.blackPanel.SetActive(true);
	}
	void OnDisable()
	{
		LabManager.instance.blackPanel.SetActive(false);
	}
}