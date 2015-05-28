using UnityEngine;
using System.Collections;
using iGUI;

public class GraphicsUserInterface {

	public void setTactileZoneOrientation() 
	{
		iGUICode_CorneaScene.getInstance ().tabPanel2.setPositionAndSize(new Rect(0f, 1f, 0.25f, 1f));

		iGUICode_CorneaScene.getInstance ().image1.setPositionAndSize(new Rect(0f, 1f, 0.25f, 1f));
	}
}