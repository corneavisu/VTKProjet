using System.Collections;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class Pachymetry {

	private const int NBSommets = 101;

	private List<Vector3> verticesPach;
	private List<int> trianglesPach;
	private List<float> elevation;
	
	//indices sommets sauvegardés
	private int[,] indexes;
	
	
	private void initSavedIndexes(int[,] indexes) {
		for (int i = 0; i < indexes.GetLength(0); i++)
			for (int j = 0; j < indexes.GetLength(1); j++)
				this.indexes [i, j] = -1;
	}
	
	public void dataToMesh(float[] sommets) {
		//Read the file as one string considering every 101 numbers as a sequence. Create the final array.

		this.verticesPach = new List<Vector3> ();
		this.trianglesPach = new List<int> ();
		this.elevation = new List<float> ();

		this.indexes = new int[NBSommets, NBSommets];
		this.initSavedIndexes (this.indexes);
		
		int compteur = NBSommets + 1;
		
		// Vertices Coordinates
		float x = 0.1f; 
		float y = 0.1f;
		
		int i = 1;
		int j = 1;
		
		while (compteur <= (NBSommets - 2) * (NBSommets - 2))
		{
			if(i == NBSommets) // 
			{// Fin d'une séquence, retour à la ligne
				x = 0.1f;
				i = 1;
				compteur += NBSommets + 1; /* Ici le compteur doit être incrémenté de 102 
				pour ne pas lier le maillage gauche de la nouvelle ligne à celui de droite de la ligne précédente*/
			}
			if (sommets [compteur] != -1000) 
			{
				// sommet central de la zone étudiée
				float sommetCentre = sommets[compteur];
				this.verticesPach.Add(new Vector3(x, y));
				this.elevation.Add(sommetCentre);
				this.indexes[i, j] = this.verticesPach.Count - 1;
				int centerIndex; // centre à calculer
				int[] tmpTriangles;
				
				if(sommets[compteur - (NBSommets + 1)] != -1000 && 
				   sommets[compteur - 1] != -1000 &&
				   sommets[compteur - NBSommets] != -1000)
				{
					// Carré bas-gauche connu: calcul du centre et création des triangles
					float sg2, sbg3, sb2;
					
					sg2 = sommets[compteur - 1];
					sbg3 = sommets[compteur - (NBSommets + 1)];
					sb2 = sommets[compteur - NBSommets];
					float center = (sbg3 + sg2 + sommetCentre + sb2)/4;
					
					this.verticesPach.Add(new Vector3(x - 0.05f, y - 0.05f));
					this.elevation.Add(center);
					centerIndex = this.verticesPach.Count - 1;
					
					if(this.indexes[i - 1, j] == -1)
					{
						this.verticesPach.Add(new Vector3(x - 0.1f, y));
						this.elevation.Add(sg2);
						this.indexes[i - 1, j] = this.verticesPach.Count - 1;
					}
					if(this.indexes[i, j - 1] == -1)
					{
						this.verticesPach.Add(new Vector3(x, y - 0.1f));
						this.elevation.Add(sb2);
						this.indexes[i, j - 1] = this.verticesPach.Count - 1;
					}
					if(this.indexes[i - 1, j - 1] == -1)
					{
						this.verticesPach.Add (new Vector3 (x - 0.1f, y - 0.1f));
						this.elevation.Add(sbg3);
						this.indexes[i - 1, j - 1] = this.verticesPach.Count - 1;
					}
					
					tmpTriangles = new int[]{this.indexes[i, j - 1], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i, j - 1],
						this.indexes[i, j - 1], this.indexes[i - 1, j - 1], centerIndex, centerIndex, this.indexes[i - 1, j - 1], this.indexes[i, j - 1],
						this.indexes[i - 1, j - 1], this.indexes[i - 1, j], centerIndex, centerIndex, this.indexes[i - 1, j], this.indexes[i - 1, j - 1],
						this.indexes[i - 1, j], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i - 1, j]};
					this.trianglesPach.AddRange(tmpTriangles);
				}
				
				if(sommets[compteur - NBSommets] != -1000 && 
				   sommets[compteur + 1] != -1000 &&
				   sommets[compteur + (1- NBSommets)] != -1000)
				{
					// Carré bas-droit connu: calcul du centre et création des triangles
					float sbd3, sd2, sb2;
					
					sbd3 = sommets[compteur + (1 - NBSommets)];
					sd2 = sommets[compteur + 1];
					sb2 = sommets[compteur - NBSommets];
					float center = (sb2 + sommetCentre + sd2 + sbd3)/4;
					
					this.verticesPach.Add(new Vector3(x + 0.05f, y - 0.05f));
					this.elevation.Add(center);
					centerIndex = this.verticesPach.Count - 1;
					this.verticesPach.Add(new Vector3(x + 0.1f, y));
					this.elevation.Add(sd2);
					this.indexes[i + 1, j] = this.verticesPach.Count - 1;
					
					if(this.indexes[i + 1, j - 1] == -1)
					{
						this.verticesPach.Add (new Vector3 (x + 0.1f, y - 0.1f));
						this.elevation.Add(sbd3);
						this.indexes[i + 1, j - 1] = this.verticesPach.Count - 1;
					}
					if(this.indexes[i, j - 1] == -1)
					{
						this.verticesPach.Add(new Vector3(x, y - 0.1f));
						this.elevation.Add(sb2);
						this.indexes[i, j - 1] = this.verticesPach.Count - 1;
					}
					
					tmpTriangles = new int[]{this.indexes[i, j], this.indexes[i + 1, j], centerIndex, centerIndex, this.indexes[i + 1, j], this.indexes[i, j],
						this.indexes[i + 1, j] , this.indexes[i + 1, j - 1], centerIndex, centerIndex, this.indexes[i + 1, j - 1], this.indexes[i + 1, j],
						this.indexes[i + 1, j - 1], this.indexes[i, j - 1], centerIndex, centerIndex, this.indexes[i, j - 1], this.indexes[i + 1, j - 1],
						this.indexes[i, j - 1], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i, j - 1]};
					this.trianglesPach.AddRange(tmpTriangles);
				}
				
				if(sommets[compteur - 1] != -1000 && 
				   sommets[compteur + (NBSommets - 1)] != -1000 &&
				   sommets[compteur + NBSommets] != -1000)
				{
					// Carré haut-gauche connu: calcul du centre et création des triangles
					float shg1, sh2, sg2;
					shg1 = sommets[compteur + (NBSommets - 1)];
					sh2 = sommets[compteur + NBSommets];
					sg2 = sommets[compteur - 1]; 
					float center = (sommetCentre + sg2 + shg1 + sh2)/4;
					
					this.verticesPach.Add(new Vector3(x - 0.05f, y + 0.05f));
					this.elevation.Add(center);
					centerIndex = this.verticesPach.Count - 1;
					this.verticesPach.Add(new Vector3(x, y + 0.1f));
					this.elevation.Add(sh2);
					indexes[i, j + 1] = this.verticesPach.Count - 1;
					
					if(this.indexes[i - 1, j] == -1)
					{
						this.verticesPach.Add(new Vector3(x - 0.1f, y));
						this.elevation.Add(sg2);
						this.indexes[i - 1, j] = this.verticesPach.Count - 1;
					}
					if(indexes[i - 1, j + 1] == -1)
					{
						this.verticesPach.Add(new Vector3(x - 0.1f, y + 0.1f));
						this.elevation.Add(shg1);
						indexes[i - 1, j + 1] = this.verticesPach.Count - 1;
					}
					
					tmpTriangles = new int[]{this.indexes[i, j], this.indexes[i - 1, j], centerIndex, centerIndex, this.indexes[i - 1, j], this.indexes[i, j],
						this.indexes[i - 1, j], this.indexes[i - 1, j + 1], centerIndex, centerIndex, this.indexes[i - 1, j + 1], this.indexes[i - 1, j],
						this.indexes[i - 1, j + 1], this.indexes[i, j + 1], centerIndex, centerIndex, this.indexes[i, j + 1], this.indexes[i - 1, j + 1],
						this.indexes[i, j + 1], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i, j + 1]};
					this.trianglesPach.AddRange(tmpTriangles);
				}
				if(sommets[compteur + NBSommets] != -1000 && 
				   sommets[compteur + (1 + NBSommets)] != -1000 &&
				   sommets[compteur + 1] != -1000)
				{
					// Carré haut-droit connu: calcul du centre et création des triangles
					float shd1, sh2, sd2;
					shd1 = sommets[compteur + (1 + NBSommets)];
					sh2 = sommets[compteur + NBSommets];
					sd2 = sommets[compteur + 1];
					float center = (sommetCentre + sh2 + shd1 + sd2)/4;
					
					this.verticesPach.Add(new Vector3(x + 0.05f, y + 0.05f));
					this.elevation.Add(center);
					centerIndex = this.verticesPach.Count - 1;
					this.verticesPach.Add (new Vector3 (x + 0.1f, y + 0.1f));
					this.elevation.Add(shd1);
					indexes[i + 1, j+ 1] = this.verticesPach.Count - 1;
					
					if(indexes[i, j + 1] == -1)
					{
						this.verticesPach.Add(new Vector3(x, y + 0.1f));
						this.elevation.Add(sh2);
						indexes[i, j + 1] = this.verticesPach.Count - 1;
					}
					if(this.indexes[i + 1, j] == -1)
					{
						this.verticesPach.Add(new Vector3(x + 0.1f, y));
						this.elevation.Add(sd2);
						this.indexes[i + 1, j] = this.verticesPach.Count - 1;
					}
					
					tmpTriangles = new int[]{this.indexes[i, j], this.indexes[i, j + 1], centerIndex, centerIndex, this.indexes[i, j + 1], this.indexes[i, j],
						this.indexes[i, j + 1], this.indexes[i + 1, j + 1], centerIndex, centerIndex, this.indexes[i + 1, j + 1], this.indexes[i, j + 1],
						this.indexes[i + 1, j + 1], this.indexes[i + 1, j], centerIndex, centerIndex, this.indexes[i + 1, j], this.indexes[i + 1, j + 1],
						this.indexes[i + 1, j], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i + 1, j]};
					this.trianglesPach.AddRange(tmpTriangles);
				}
			}
			
			x += 0.2f;
			i += 2;
			if(i == NBSommets)
			{
				y += 0.2f;
				j += 2;
			}
			
			compteur += 2; 
		}
	}
	
	public List<Vector3> getVerticesPach() {
		return this.verticesPach;
	}
	
	public List<int> getTrianglesPach() {
		return this.trianglesPach;
	}
	
	public List<float> getElevationPach() {
		return this.elevation;
	}
}
