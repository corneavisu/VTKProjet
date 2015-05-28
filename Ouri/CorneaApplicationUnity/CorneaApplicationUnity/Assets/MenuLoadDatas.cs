using UnityEngine;
using System.Collections;
using System;
using System.IO;
using iGUI;
using System.Collections.Generic;

public class MenuLoadDatas : MonoBehaviour {

	private GUISkin guiSkin;
	private List<iGUIButton> buttons;
	private iGUIListBox listBox;
	public static String corneaId;
	private List<String> cornees;
	private String removedCorneaId;
	private GUISkin skin;
	//private iGUIProgressBar progressBar;
	//private float progress;

	// Use this for initialization
	void Start () 
	{
		String corneasPath = Application.persistentDataPath + "/Cornees";
		this.cornees = new List<string>();		
		this.buttons = new List<iGUIButton>();
		this.listBox = iGUICode_MenuScene.getInstance ().listBox1;

		#if UNITY_IPHONE
		{
			String[] tmpCornees = Directory.GetDirectories (corneasPath);
			for(int i = 0; i < tmpCornees.Length; i++)
			{
				if(tmpCornees[i].Contains("cornee"))
				{
					String number = tmpCornees[i].Substring(80, tmpCornees[i].IndexOf('_') - 80);
					this.cornees.Add(number);
					this.buttons.Add(this.listBox.addElement<iGUIButton>());
				}
			}
		}
		#elif UNITY_ANDROID
		{
			TextAsset[] tmpCornees = Resources.LoadAll<TextAsset>("Cornees");
			for(int i = 0; i < tmpCornees.Length; i++)
			{
				String filename = tmpCornees[i].name;
				filename = filename.Substring(0, filename.IndexOf('_'));
				if(!this.cornees.Contains(filename))
				{
					this.cornees.Add(filename);
					this.buttons.Add(this.listBox.addElement<iGUIButton>());
				}
			}
		}
		#endif

		this.cornees.Sort();

		GUIStyle styleButton = new GUIStyle();
		styleButton.fixedHeight = 128f;
		styleButton.fontSize = 40;


		for(int i = 0; i < buttons.Count; i++)
		{
			//this.buttons[i].style = styleButton;
			this.buttons[i].setWidth(1f);

			this.buttons[i].label.text = "Cornee id: " + this.cornees[i];
			this.buttons[i].setType(iGUIButtonType.ButtonBigBlack);
			this.buttons[i].setEnabled(true);
			this.buttons[i].longPressCallback = longPressCorneaItem;
			this.buttons[i].clickCallback = visualizeCorneaItem;
		}
	
		GUISkin skin = GetSkin();


		skin.button.fixedHeight = 128f;
		skin.button.fontSize = 40;
		skin.button.border = new RectOffset(15, 15, 15, 15);
		skin.button.alignment = TextAnchor.MiddleCenter;
		skin.button.richText = true;
		skin.button.stretchWidth = true;


		skin.label.fontSize = 40;
		skin.label.alignment = TextAnchor.MiddleCenter;
	
		//this.buttons[0].style 
		iGUICode_MenuScene.getInstance().root1.setSkin(skin);
	}

	void longPressCorneaItem(iGUIElement caller)
	{
		this.removedCorneaId = caller.label.text.Substring(11, caller.label.text.Length - 11);
		iGUIWindow alertWindow = iGUIRoot.alert("Attention", "Voulez-vous supprimer la cornée " + this.removedCorneaId + "?", "Oui", removeCorneaItem, "Annuler", null);
		alertWindow.setWidth(1f);
		alertWindow.setColor(Color.magenta);
		alertWindow.opacity = 255f;
	}

	void removeCorneaItem(iGUIElement caller)
	{
		int index = cornees.IndexOf(this.removedCorneaId);

		Directory.Delete(Application.persistentDataPath + "/Cornees/" + this.removedCorneaId + "_cornee", true);
		this.listBox.removeElement(buttons[index]);
	}

	void visualizeCorneaItem(iGUIElement caller)
	{
		corneaId = caller.label.text.Substring(11, caller.label.text.Length - 11);
		StartCoroutine(loadLevel());
	}

	public IEnumerator loadLevel()
	{
		#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.Gray);
		#elif UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		#endif
		Handheld.StartActivityIndicator();
		yield return new WaitForSeconds(0);
		Application.LoadLevel("CorneaScene");
	}

	private static GUISkin GetSkin()
	{
		return GUISkin.CreateInstance<GUISkin>();
	}
}
