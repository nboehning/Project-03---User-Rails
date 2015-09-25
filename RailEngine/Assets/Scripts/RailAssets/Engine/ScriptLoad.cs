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
    TextReader reader;

    DirectoryInfo info;

    //for testing to see if levels are there
    List<string> levelNames = new List<string>();

    //read in the levels into this
    List<Item> levels = new List<Item>();

    void Start()
    {
        //read in files in a directory
        info = new DirectoryInfo(Application.dataPath + "/");
        FileInfo[] levelInfo = info.GetFiles();

        foreach(FileInfo file in levelInfo)
        {
            if (file.Name.EndsWith(".dan"))
            {
                levelNames.Add(file.Name.ToString());
                textConsole.text += "\n" + file;

                reader = file.OpenText();


                string lineOfText;
                int lineNumber = 0;
                Item tempItem = new Item();
                while ((lineOfText = reader.ReadLine()) != null)
                {

                    
                    switch (lineNumber)
                    {
                        case 0:
                            tempItem.author = lineOfText;
                            break;
                        case 1:
                            tempItem.name = lineOfText;
                            break;
                            //ADD tempItem To a list so that the scroll list will read it.
                    }

                    lineNumber++;
                }

            }
        }

        if(levelNames.Count <= 0)
        {
            textConsole.text += "\nNo Levels Found in <" + Application.dataPath + "/>";
        }



    }


    
	
}
