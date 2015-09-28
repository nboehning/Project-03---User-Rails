using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item {
	public string name;
	public Sprite icon;
	public string author;
	public string dateCreated;
	public string fileName;
	public Button.ButtonClickedEvent thingToDo;
}

public class ScriptCreateScrollList : MonoBehaviour {

	public GameObject sampleButton;
	public List<Item> itemList;

	public Transform contentPanel;

	ScriptLoad scriptLoad;

	public void LoadInLevelList()
	{
		scriptLoad = gameObject.GetComponent<ScriptLoad>();

		itemList = scriptLoad.levels;
		PopulateList();
	}
	
	void PopulateList()
	{
		foreach (Item item in itemList)
		{
			GameObject newButton = (GameObject)Instantiate(sampleButton);
			SampleButton tempButton = newButton.GetComponent<SampleButton>();
			
			tempButton.levelName.text = item.name;
			tempButton.authorLabel.text = item.author;
			
			//tempButton.icon.sprite = item.icon;
			
			//tempButton.button.onClick = item.thingToDo;

			tempButton.button.onClick.AddListener(delegate { SomethingToDo(tempButton.levelName.text); });

			newButton.transform.SetParent(contentPanel);
			
		}
	}

	public void SomethingToDo(string name)
	{
		
		scriptLoad.ImportLevel(name);
		Application.LoadLevel("SceneBasic");
	}

}
