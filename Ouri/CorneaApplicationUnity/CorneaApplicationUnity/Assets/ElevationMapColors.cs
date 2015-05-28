using UnityEngine;
using System.Collections;

public class ElevationMapColors {

	public Color32[] calculateColor(float[] elevations) {
		Color32[] verticesColors = new Color32[elevations.Length];
		for(int i = 0; i < verticesColors.Length; i++)
		{
			if(elevations[i] != -1000)
				verticesColors[i] = convertDistanceToColor(elevations[i]);
			else
				verticesColors[i] = new Color32(169, 234, 254, 255);
		}
		return verticesColors;
	}
	
	public Color32[] calculateColorSmooth(float[] elevations) {
		Color32[] verticesColors = new Color32[elevations.Length];
		for(int i = 0; i < verticesColors.Length; i++)
		{
			if(elevations[i] != -1000)
				verticesColors[i] = convertDistanceToSmoothColor(elevations[i]);
			else
				verticesColors[i] = new Color32(169, 234, 254, 255);
		}
		return verticesColors;
	}
	
	private static Color32 convertDistanceToColor(float distance) {
		float echelle = distance * 1000; // convertir des millimètres en micromètres
		
		if (echelle <= 610f && echelle >= 590f)
			return new Color32 (19, 209, 87, 255);
		else if (echelle < 590f && echelle >= 570f)
			return new Color32 (86, 231, 4, 255);
		else if (echelle < 570f && echelle >= 550f)
			return new Color32 (136, 248, 2, 255);
		else if (echelle < 550f && echelle >= 530f)
			return new Color32 (237, 253, 4, 255);
		else if (echelle < 530f && echelle >= 510f)
			return new Color32 (253, 230, 2, 255);
		else if (echelle < 510f && echelle >= 490f)
			return new Color32 (223, 196, 3, 255);
		else if (echelle < 490f && echelle >= 470f)
			return new Color32 (218, 172, 1, 255);
		else if (echelle < 470f && echelle >= 450f)
			return new Color32 (221, 139, 1, 255);
		else if (echelle < 450f && echelle >= 430f)
			return new Color32 (218, 112, 0, 255);
		else if (echelle < 430f && echelle >= 410f)
			return new Color32 (222, 81, 2, 255);
		else if (echelle < 410f && echelle >= 390f)
			return new Color32 (198, 47, 4, 255);
		else if (echelle < 390f && echelle >= 370f)
			return new Color32 (184, 0, 0, 255);
		else if (echelle < 370f && echelle >= 350f)
			return new Color32 (161, 3, 2, 255);
		else if (echelle < 350f && echelle >= 330f)
			return new Color32 (141, 61, 64, 255);
		else if (echelle < 330f && echelle >= 310f)
			return new Color32 (170, 80, 80, 255);
		else if (echelle < 310f && echelle >= 290f)
			return new Color32 (201, 101, 101, 255);
		else if (echelle < 290f && echelle >= 270f)
			return new Color32 (221, 118, 121, 255);
		else if (echelle < 270f && echelle >= 250f)
			return new Color32 (228, 140, 139, 255);
		else if (echelle > 610f && echelle <= 630f)
			return new Color32 (0, 239, 242, 255);
		else if (echelle > 630f && echelle <= 650f)
			return new Color32 (1, 210, 240, 255);
		else if (echelle > 650f && echelle <= 670f)
			return new Color32 (2, 188, 237, 255);
		else if (echelle > 670f && echelle <= 690f)
			return new Color32 (15, 158, 236, 255);
		else if (echelle > 690f && echelle <= 710f)
			return new Color32 (27, 122, 240, 255);
		else if (echelle > 710f && echelle <= 730f)
			return new Color32 (40, 101, 218, 255);
		else if (echelle > 730f && echelle <= 750f)
			return new Color32 (39, 59, 218, 255);
		else if (echelle > 750f && echelle <= 770f)
			return new Color32 (42, 1, 193, 255);
		else if (echelle > 770f && echelle <= 790f)
			return new Color32 (40, 0, 149, 255);
		else if (echelle > 790f && echelle <= 810f)
			return new Color32 (78, 40, 139, 255);
		else if (echelle > 810f && echelle <= 830f)
			return new Color32 (100, 38, 161, 255);
		else if (echelle > 830f && echelle <= 850f)
			return new Color32 (131, 40, 161, 255);
		else if (echelle > 850f && echelle <= 870f)
			return new Color32 (150, 39, 178, 255);
		else if (echelle > 870f && echelle <= 890f)
			return new Color32 (170, 40, 200, 255);
		else if (echelle > 890f && echelle <= 910f)
			return new Color32 (179, 59, 219, 255);
		else if (echelle > 910f && echelle <= 930f)
			return new Color32 (190, 78, 238, 255);
		else if (echelle > 930f && echelle <= 950f)
			return new Color32 (195, 101, 249, 255);
		else 
			return new Color32 (255, 255, 255, 255);
	}
	
	
	private static Color32 convertDistanceToSmoothColor(float distance) {
		float echelle = distance * 1000; // convertir des millimètres en micromètres
		if (echelle <= 600f && echelle > 580f)
			return Color32.Lerp (new Color32 (19, 209, 87, 255), new Color32 (86, 231, 4, 255), (echelle - 600f) / -20f);
		else if(echelle <= 580f && echelle > 560f)
			return Color32.Lerp (new Color32 (86, 231, 4, 255), new Color32 (136, 248, 2, 255), (echelle - 580f) / -20f);
		else if(echelle <= 560f && echelle > 540f)
			return Color32.Lerp (new Color32 (136, 248, 2, 255), new Color32 (237, 253, 4, 255), (echelle - 560f) / -20f);
		else if(echelle <= 540f && echelle > 520f)
			return Color32.Lerp (new Color32 (237, 253, 4, 255), new Color32 (253, 230, 2, 255), (echelle - 540f) / -20f);
		else if(echelle <= 520f && echelle > 500f)
			return Color32.Lerp (new Color32 (253, 230, 2, 255), new Color32 (223, 196, 3, 255), (echelle - 520f) / -20f);
		else if(echelle <= 500f && echelle > 480f)
			return Color32.Lerp (new Color32 (223, 196, 3, 255), new Color32 (218, 172, 1, 255), (echelle - 500f) / -20f);
		else if(echelle <= 480f && echelle > 460f)
			return Color32.Lerp (new Color32 (218, 172, 1, 255), new Color32 (221, 139, 1, 255), (echelle - 480f) / -20f);
		else if(echelle <= 460f && echelle > 440f)
			return Color32.Lerp (new Color32 (221, 139, 1, 255), new Color32 (218, 112, 0, 255), (echelle - 460f) / -20f);
		else if(echelle <= 440f && echelle > 420f)
			return Color32.Lerp (new Color32 (218, 112, 0, 255), new Color32 (222, 81, 2, 255), (echelle - 440f) / -20f);
		else if(echelle <= 420f && echelle > 400f)
			return Color32.Lerp (new Color32 (222, 81, 2, 255), new Color32 (198, 47, 4, 255), (echelle - 420f) / -20f);
		else if(echelle <= 400f && echelle > 380f)
			return Color32.Lerp (new Color32 (198, 47, 4, 255), new Color32 (184, 0, 0, 255), (echelle - 400f) / -20f);
		else if(echelle <= 380f && echelle > 360f)
			return Color32.Lerp (new Color32 (184, 0, 0, 255), new Color32 (161, 3, 2, 255), (echelle - 380f) / -20f);
		else if(echelle <= 360f && echelle > 340f)
			return Color32.Lerp (new Color32 (161, 3, 2, 255), new Color32 (141, 61, 64, 255), (echelle - 360f) / -20f);
		else if(echelle <= 340f && echelle > 320f)
			return Color32.Lerp (new Color32 (141, 61, 64, 255), new Color32 (170, 80, 80, 255), (echelle - 340f) / -20f);
		else if(echelle <= 320f && echelle > 300f)
			return Color32.Lerp (new Color32 (170, 80, 80, 255), new Color32 (201, 101, 101, 255), (echelle - 320f) / -20f);
		else if(echelle <= 300f && echelle > 280f)
			return Color32.Lerp (new Color32 (201, 101, 101, 255), new Color32 (221, 118, 121, 255), (echelle - 300) / -20f);
		else if(echelle <= 280f && echelle > 260f)
			return Color32.Lerp (new Color32 (221, 118, 121, 255), new Color32 (228, 140, 139, 255), (echelle - 280) / -20f);
		else if(echelle > 600f && echelle <= 620f)
			return Color32.Lerp (new Color32 (19, 209, 87, 255), new Color32 (0, 239, 242, 255), (echelle - 600f) / 20f);
		else if(echelle > 620f && echelle <= 640f)
			return Color32.Lerp (new Color32 (0, 239, 242, 255), new Color32 (1, 210, 240, 255), (echelle - 620f) / 20f);
		else if(echelle > 640f && echelle <= 660f)
			return Color32.Lerp (new Color32 (1, 210, 240, 255), new Color32 (2, 188, 237, 255), (echelle - 640f) / 20f);
		else if(echelle > 660f && echelle <= 680f)
			return Color32.Lerp (new Color32 (2, 188, 237, 255), new Color32 (15, 158, 236, 255), (echelle - 660f) / 20f);
		else if(echelle > 680f && echelle <= 700f)
			return Color32.Lerp (new Color32 (15, 158, 236, 255), new Color32 (27, 122, 240, 255), (echelle - 680f) / 20f);
		else if(echelle > 700f && echelle <= 720f)
			return Color32.Lerp (new Color32 (27, 122, 240, 255), new Color32 (40, 101, 218, 255), (echelle - 700f) / 20f);
		else if(echelle > 720f && echelle <= 740f)
			return Color32.Lerp (new Color32 (40, 101, 218, 255), new Color32 (39, 59, 218, 255), (echelle - 720f) / 20f);
		else if(echelle > 740f && echelle <= 760f)
			return Color32.Lerp (new Color32 (39, 59, 218, 255), new Color32 (42, 1, 193, 255), (echelle - 740f) / 20f);
		else if(echelle > 760f && echelle <= 780f)
			return Color32.Lerp (new Color32 (42, 1, 193, 255), new Color32 (40, 0, 149, 255), (echelle - 760f) / 20f);
		else if(echelle > 780f && echelle <= 800f)
			return Color32.Lerp (new Color32 (40, 0, 149, 255), new Color32 (78, 40, 139, 255), (echelle - 780f) / 20f);
		else if(echelle > 800f && echelle <= 820f)
			return Color32.Lerp (new Color32 (78, 40, 139, 255), new Color32 (100, 38, 161, 255), (echelle - 800f) / 20f);
		else if(echelle > 820f && echelle <= 840f)
			return Color32.Lerp (new Color32 (100, 38, 161, 255), new Color32 (131, 40, 161, 255), (echelle - 820f) / 20f);
		else if(echelle > 840f && echelle <= 860f)
			return Color32.Lerp (new Color32 (131, 40, 161, 255), new Color32 (150, 39, 178, 255), (echelle - 840f) / 20f);
		else if(echelle > 860f && echelle <= 880f)
			return Color32.Lerp (new Color32 (150, 39, 178, 255), new Color32 (170, 40, 200, 255), (echelle - 860f) / 20f);
		else if(echelle > 880f && echelle <= 900f)
			return Color32.Lerp (new Color32 (170, 40, 200, 255), new Color32 (179, 59, 219, 255), (echelle - 880f) / 20f);
		else if(echelle > 900f && echelle <= 920f)
			return Color32.Lerp (new Color32 (179, 59, 219, 255), new Color32 (190, 78, 238, 255), (echelle - 900f) / 20f);
		else if(echelle > 920f && echelle <= 940f)
			return Color32.Lerp (new Color32 (190, 78, 238, 255), new Color32 (195, 101, 249, 255), (echelle - 920f) / 20f);
		else
			return new Color32 (255, 255, 255, 255);
	}
}

