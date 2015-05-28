using UnityEngine;
using System.Collections;
using System;

public class CorneaColorMapping
{
	private float [] sphereDistances;
	private float prevDeformationFactor;

	public void calculateDistances(Vector3[] vertices, Sphere sphere)
	{
		this.prevDeformationFactor = 0f;
		this.sphereDistances = new float[vertices.Length];

		for (int i = 0; i < vertices.Length; i++) 
		{
			this.sphereDistances[i] = Convert.ToSingle(Math.Sqrt(Math.Pow(vertices[i].x - sphere.CX, 2) + 
			                                            Math.Pow(vertices[i].y - sphere.CY, 2) +
			                                            Math.Pow(vertices[i].z - sphere.CZ, 2))) - sphere.R;
		}
	}

	public Color32[] calculateColor() {
		Color32[] verticesColors = new Color32[this.sphereDistances.Length];
		for(int i = 0; i < verticesColors.Length; i++)
			verticesColors[i] = convertDistanceToColor(this.sphereDistances[i]);
		return verticesColors;
	}

	public Color32[] calculateColorSmooth() {
		Color32[] verticesColors = new Color32[this.sphereDistances.Length];
		for(int i = 0; i < verticesColors.Length; i++)
			verticesColors[i] = convertDistanceToSmoothColor(this.sphereDistances[i]);
		return verticesColors;
	}

	public Vector3[] zCorneaDeformate(float deformationFactor, Vector3[] vertices)
	{
		for(int i = 0; i < vertices.Length; i++) 
		{
			if(deformationFactor > this.prevDeformationFactor)
				vertices[i].z += (this.sphereDistances[i] * (deformationFactor - this.prevDeformationFactor + 1f)) - this.sphereDistances[i];
			else if(deformationFactor < this.prevDeformationFactor)
				vertices[i].z -= (this.sphereDistances[i] * (this.prevDeformationFactor - deformationFactor + 1f)) - this.sphereDistances[i];
		}
		this.prevDeformationFactor = deformationFactor;
		return vertices;
	}
	
	private static Color32 convertDistanceToColor(float distance) {
		float echelle = distance * 1000; // convertir des millimètres en micromètres
		
		if (echelle <= 2.5f && echelle >= - 2.5f)
			return new Color32 (74, 183, 71, 255);
		else if (echelle < - 2.5f && echelle >= - 7.5f)
			return new Color32 (102, 204, 226, 255);
		else if (echelle < - 7.5f && echelle >= - 12.5f)
			return new Color32 (85, 202, 233, 255);
		else if (echelle < - 12.5f && echelle >= - 17.5f)
			return new Color32 (61, 199, 243, 255);
		else if (echelle < - 17.5f && echelle >= - 22.5f)
			return new Color32 (57, 183, 234, 255);
		else if (echelle < - 22.5f && echelle >= - 27.5f)
			return new Color32 (68, 162, 218, 255);
		else if (echelle < - 27.5f && echelle >= - 32.5f)
			return new Color32 (67, 140, 203, 255);
		else if (echelle < - 32.5f && echelle >= - 37.5f)
			return new Color32 (66, 123, 190, 255);
		else if (echelle < - 37.5f && echelle >= - 42.5f)
			return new Color32 (64, 110, 181, 255);
		else if (echelle < - 42.5f && echelle >= - 47.5f)
			return new Color32 (60, 101, 175, 255);
		else if (echelle < - 47.5f && echelle >= - 52.5f)
			return new Color32 (61, 94, 171, 255);
		else if (echelle < - 52.5f && echelle >= - 57.5f)
			return new Color32 (64, 83, 164, 255);
		else if (echelle < - 57.5f && echelle >= - 62.5f)
			return new Color32 (72, 67, 155, 255);
		else if (echelle < - 62.5f && echelle >= - 67.5f)
			return new Color32 (99, 53, 147, 255);
		else if (echelle < - 67.5f && echelle >= - 72.5f)
			return new Color32 (114, 52, 148, 255);
		else if (echelle < - 72.5f && echelle >= - 77.5f)
			return new Color32 (129, 52, 148, 255);
		else if (echelle < - 77.5f && echelle >= - 82.5f)
			return new Color32 (145, 49, 147, 255);
		else if (echelle < - 82.5f && echelle >= - 87.5f)
			return new Color32 (163, 50, 148, 255);
		else if (echelle > 2.5f && echelle <= 7.5f)
			return new Color32 (246, 237, 22, 255);
		else if (echelle > 7.5f && echelle <= 12.5f)
			return new Color32 (249, 223, 18, 255);
		else if (echelle > 12.5f && echelle <= 17.5f)
			return new Color32 (254, 201, 16, 255);
		else if (echelle > 17.5f && echelle <= 22.5f)
			return new Color32 (253, 179, 23, 255);
		else if (echelle > 22.5f && echelle <= 27.5f)
			return new Color32 (249, 157, 29, 255);
		else if (echelle > 27.5f && echelle <= 32.5f)
			return new Color32 (246, 134, 32, 255);
		else if (echelle > 32.5f && echelle <= 37.5f)
			return new Color32 (243, 112, 35, 255);
		else if (echelle > 37.5f && echelle <= 42.5f)
			return new Color32 (241, 91, 35, 255);
		else if (echelle > 42.5f && echelle <= 47.5f)
			return new Color32 (240, 72, 34, 255);
		else if (echelle > 47.5f && echelle <= 52.5f)
			return new Color32 (239, 51, 35, 255);
		else if (echelle > 52.5f && echelle <= 57.5f)
			return new Color32 (220, 30, 38, 255);
		else if (echelle > 57.5f && echelle <= 62.5f)
			return new Color32 (201, 32, 44, 255);
		else if (echelle > 62.5f && echelle <= 67.5f)
			return new Color32 (179, 29, 60, 255);
		else if (echelle > 67.5f && echelle <= 72.5f)
			return new Color32 (160, 37, 79, 255);
		else if (echelle > 72.5f && echelle <= 77.5f)
			return new Color32 (139, 59, 100, 255);
		else if (echelle > 77.5f && echelle <= 82.5f)
			return new Color32 (120, 59, 100, 255);
		else if (echelle > 82.5f && echelle <= 87.5f)
			return new Color32 (100, 60, 100, 255);
		else 
			return new Color32 (255, 255, 255, 255);
	}


	private static Color32 convertDistanceToSmoothColor(float distance) {
		float echelle = distance * 1000; // convertir des millimètres en micromètres
		if (echelle <= 0f && echelle > -5f)
			return Color32.Lerp (new Color32 (74, 183, 71, 255), new Color32 (102, 204, 226, 255), echelle / -5f);
		else if(echelle <= -5f && echelle > -10f)
			return Color32.Lerp (new Color32 (102, 204, 226, 255), new Color32 (85, 202, 233, 255), (echelle + 5f) / -5f);
		else if(echelle <= -10f && echelle > -15f)
			return Color32.Lerp (new Color32 (85, 202, 233, 255), new Color32 (61, 199, 243, 255), (echelle + 10f) / -5f);
		else if(echelle <= -15f && echelle > -20f)
			return Color32.Lerp (new Color32 (61, 199, 243, 255), new Color32 (57, 183, 234, 255), (echelle + 15f) / -5f);
		else if(echelle <= -20f && echelle > -25f)
			return Color32.Lerp (new Color32 (57, 183, 234, 255), new Color32 (68, 162, 218, 255), (echelle + 20f) / -5f);
		else if(echelle <= -25f && echelle > -30f)
			return Color32.Lerp (new Color32 (68, 162, 218, 255), new Color32 (67, 140, 203, 255), (echelle + 25f) / -5f);
		else if(echelle <= -30f && echelle > -35f)
			return Color32.Lerp (new Color32 (67, 140, 203, 255), new Color32 (66, 123, 190, 255), (echelle + 30f) / -5f);
		else if(echelle <= -35f && echelle > -40f)
			return Color32.Lerp (new Color32 (66, 123, 190, 255), new Color32 (64, 110, 181, 255), (echelle + 35f) / -5f);
		else if(echelle <= -40f && echelle > -45f)
			return Color32.Lerp (new Color32 (64, 110, 181, 255), new Color32 (60, 101, 175, 255), (echelle + 40f) / -5f);
		else if(echelle <= -45f && echelle > -50f)
			return Color32.Lerp (new Color32 (60, 101, 175, 255), new Color32 (61, 94, 171, 255), (echelle + 45f) / -5f);
		else if(echelle <= -50f && echelle > -55f)
			return Color32.Lerp (new Color32 (61, 94, 171, 255), new Color32 (64, 83, 164, 255), (echelle + 50f) / -5f);
		else if(echelle <= -55f && echelle > -60f)
			return Color32.Lerp (new Color32 (64, 83, 164, 255), new Color32 (72, 67, 155, 255), (echelle + 55f) / -5f);
		else if(echelle <= -60f && echelle > -65f)
			return Color32.Lerp (new Color32 (72, 67, 155, 255), new Color32 (99, 53, 147, 255), (echelle + 60f) / -5f);
		else if(echelle <= -65f && echelle > -70f)
			return Color32.Lerp (new Color32 (99, 53, 147, 255), new Color32 (114, 52, 148, 255), (echelle + 65f) / -5f);
		else if(echelle <= -70f && echelle > -75f)
			return Color32.Lerp (new Color32 (114, 52, 148, 255), new Color32 (129, 52, 148, 255), (echelle + 70f) / -5f);
		else if(echelle <= -75f && echelle > -80f)
			return Color32.Lerp (new Color32 (129, 52, 148, 255), new Color32 (145, 49, 147, 255), (echelle + 75) / -5f);
		else if(echelle <= -80f && echelle > -85f)
			return Color32.Lerp (new Color32 (145, 49, 147, 255), new Color32 (163, 50, 148, 255), (echelle + 80) / -5f);
		else if(echelle > 0f && echelle <= 5f)
			return Color32.Lerp (new Color32 (74, 183, 71, 255), new Color32 (246, 237, 22, 255), echelle / 5f);
		else if(echelle > 5f && echelle <= 10f)
			return Color32.Lerp (new Color32 (246, 237, 22, 255), new Color32 (249, 223, 18, 255), (echelle - 5f) / 5f);
		else if(echelle > 10f && echelle <= 15f)
			return Color32.Lerp (new Color32 (249, 223, 18, 255), new Color32 (254, 201, 16, 255), (echelle - 10f) / 5f);
		else if(echelle > 15f && echelle <= 20f)
			return Color32.Lerp (new Color32 (254, 201, 16, 255), new Color32 (253, 179, 23, 255), (echelle - 15f) / 5f);
		else if(echelle > 20f && echelle <= 25f)
			return Color32.Lerp (new Color32 (253, 179, 23, 255), new Color32 (249, 157, 29, 255), (echelle - 20f) / 5f);
		else if(echelle > 25f && echelle <= 30f)
			return Color32.Lerp (new Color32 (249, 157, 29, 255), new Color32 (246, 134, 32, 255), (echelle - 25f) / 5f);
		else if(echelle > 30f && echelle <= 35f)
			return Color32.Lerp (new Color32 (246, 134, 32, 255), new Color32 (243, 112, 35, 255), (echelle - 30f) / 5f);
		else if(echelle > 35f && echelle <= 40f)
			return Color32.Lerp (new Color32 (243, 112, 35, 255), new Color32 (241, 91, 35, 255), (echelle - 35f) / 5f);
		else if(echelle > 40f && echelle <= 45f)
			return Color32.Lerp (new Color32 (241, 91, 35, 255), new Color32 (240, 72, 34, 255), (echelle - 40f) / 5f);
		else if(echelle > 45f && echelle <= 50f)
			return Color32.Lerp (new Color32 (240, 72, 34, 255), new Color32 (239, 51, 35, 255), (echelle - 45f) / 5f);
		else if(echelle > 50f && echelle <= 55f)
			return Color32.Lerp (new Color32 (239, 51, 35, 255), new Color32 (220, 30, 38, 255), (echelle - 50f) / 5f);
		else if(echelle > 55f && echelle <= 60f)
			return Color32.Lerp (new Color32 (220, 30, 38, 255), new Color32 (201, 32, 44, 255), (echelle - 55f) / 5f);
		else if(echelle > 60f && echelle <= 65f)
			return Color32.Lerp (new Color32 (201, 32, 44, 255), new Color32 (179, 29, 60, 255), (echelle - 60f) / 5f);
		else if(echelle > 65f && echelle <= 70f)
			return Color32.Lerp (new Color32 (179, 29, 60, 255), new Color32 (160, 37, 79, 255), (echelle - 65f) / 5f);
		else if(echelle > 70f && echelle <= 75f)
			return Color32.Lerp (new Color32 (160, 37, 79, 255), new Color32 (139, 59, 100, 255), (echelle - 70f) / 5f);
		else if(echelle > 75f && echelle <= 80f)
			return Color32.Lerp (new Color32 (139, 59, 100, 255), new Color32 (120, 59, 100, 255), (echelle - 75f) / 5f);
		else if(echelle > 80f && echelle <= 85f)
			return Color32.Lerp (new Color32 (120, 59, 100, 255), new Color32 (100, 60, 100, 255), (echelle - 80f) / 5f);
		else
			return new Color32 (255, 255, 255, 255);
	}
}

