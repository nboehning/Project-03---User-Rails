using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Author: Andrew Seba
/// </summary>
public class ScriptLoad : MonoBehaviour {

	public Text textConsole;

	TextAsset textFile;
	StreamReader reader;

	DirectoryInfo info;

	//for testing to see if levels are there
	List<string> levelNames = new List<string>();

	//read in the levels into this
	public List<Item> levels = new List<Item>();

	void Start()
	{
		//read in files in a directory
		info = new DirectoryInfo(Application.dataPath + "/");
		FileInfo[] levelInfo = info.GetFiles();

		foreach(FileInfo file in levelInfo)
		{
			if (file.Name.EndsWith(".dan"))
			{
				levelNames.Add(file.Name);
				textConsole.text += "\n" + file;

				reader = file.OpenText();


				string lineOfText;
				int lineNumber = 0;
				Item tempItem = new Item();
				while ((lineOfText = reader.ReadLine()) != null)
				{

					if(lineNumber < 3)
					{
						switch (lineNumber)
						{
							case 0:
								tempItem.author = lineOfText;
								break;
							case 1:
								tempItem.name = lineOfText;
								break;
							
						}

					}



					lineNumber++;
				}
				tempItem.fileName = file.Name;

				levels.Add(tempItem);

			}
		}
        reader.Close();

		if(levelNames.Count <= 0)
		{
			textConsole.text += "\nNo Levels Found in <" + Application.dataPath + "/>"; 
		}

		//BroadcastMessage("LoadInLevelList");
		ScriptCreateScrollList scrollList = 
			gameObject.GetComponent<ScriptCreateScrollList>();
		scrollList.LoadInLevelList();

	}


	public void ImportLevel(string pLevelName)
	{

        FileInfo levelReadingData = new FileInfo(Application.dataPath + "/" + FindLevel(pLevelName));
        reader = levelReadingData.OpenText();

        string input = reader.ReadToEnd();
        reader.Close();

        File.WriteAllText(Application.dataPath + "/waypoints.txt", input);
        

    }

    string FindLevel(string pName)
	{
		foreach(Item levelItem in levels)
		{
            if (pName == levelItem.name)
			{

				return levelItem.fileName;
			}
				
		}
		return "/Resources/embedded.txt";
		
	}


}
