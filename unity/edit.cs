/**
 *
 *在Unity3D中，我们经常会需要在本地或者服务器上读取游戏数据，这几篇blog记录下目前知道的两种读取数据表的方式。
 一种是在Resource目录下通过TextAsset读取，一种是通过文件流的方式读取。Resource 下的数据是不能修改的，比如
 单机游戏的装备，NPC数据等；而文件流的方式是存和取数据，比如保存游戏进度等。
 
  写这个code snippet时，我一开始用resources文件夹下面的内容更新文本，结果发现永远不会成功，这是因为unity
  打包时，将这个文件一起打包成一个总的文件，所以不能更新它，只能读取它，要样实现更新，只能将该文件放在resources
  目录外的位置，如streamingAssets文件夹。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
using System.Text;



public class edit : MonoBehaviour {

	private string[][] _array; //这个二维数组里面保存了csv文件的内容。
	private GameObject root;
	private GameObject textArea;  //

	private int ii; //点击一个按钮，找到该按钮文字时，把行数记录下来，编辑时要把该行数更改。这时一个中间变量。
	private GameObject currentButton;

	void Start () {
		root = GameObject.Find ("Canvas");
		textArea = GameObject.Find ("Canvas/Comptext");
		ReadTheText ();
	}

	public void ClickButton(){//当用户点击某个按钮，将这个按钮对应的介绍文字读出来，显示到相应的区域中。
		currentButton=EventSystem.current.currentSelectedGameObject;
		string childName = currentButton.name;
		string parentName = currentButton.transform.parent.gameObject.name;
		string displayText = "";
		for (int i = 0; i < _array.Length - 1; i++) {
			if (_array [i] [3] == childName && _array [i] [4] == parentName) {  //如果这个按钮对应的父按钮到该按钮都匹配，可以唯一确定该按钮。
				displayText = _array [i] [2];
				ii = i;
				break;
			}
		}
	}

	private void ReadTheText(){
		StreamReader sr = File.OpenText (Application.dataPath + "/StreamingAssets/database/Theory.txt");
		//读取每一行。
		string[] lineArray=sr.ReadToEnd().Split("\r[0]");
		_array = new string[lineArray.Length][];

		for (int i = 0; i < lineArray.Length; i++) {
			_array [i] = lineArray [i].Split ("," [0]);
		}
	}

	public void PostEdit(){  //当用户点击了编辑按钮后，更新csv中的内容，并重新读取一遍，把内容更新。
		GameObject inputfield = root.transform.Find ("inputfield").gameObject;
		string text_content = inputfield.GetComponent<inputfield> ().text;
		_array [ii] [2] = text_content;  //第二个字段里面是文本的内容，要更新第二个字段
		updateFile(Application.dataPath+"/StreamingAssets/database/Theory.txt");
		Debug.Log (text_content);
		root.transform.Find ("inputfield").gameObject.SetActive (false);
		ReadTheText ();   //文件已经改了，但内存里的内容还没有变，要把内存里面的内容更新。
		textArea.GetComponent<Text>().text=text_content; //把显示区域内容更改变用户输入的内容。
	}

	public void updateFile(string filePath){
		string[] str = new string[_array.Length];
		for (int i = 0; i < _array.Length; i++) {
			str [i] = string.Join (",", _array [i]);
		}
		File.WriteAllLines (filePath, str);
	}


}
