using UnityEngine;
using System.Collections;
using iGUI;

public class iGUICode_MenuScene : MonoBehaviour{
	[HideInInspector]
	public iGUILabel label1;
	[HideInInspector]
	public iGUIListBox listBox1;
	[HideInInspector]
	public iGUIRoot root1;

	static iGUICode_MenuScene instance;
	void Awake(){
		instance=this;
	}
	public static iGUICode_MenuScene getInstance(){
		return instance;
	}
}
