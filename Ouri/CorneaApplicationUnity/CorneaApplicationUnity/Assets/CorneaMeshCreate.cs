
using System.Collections;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class CorneaMeshCreate
{
	private const int NBSommets = 101;
	
	private List<Vector3> verticesAnt;
	private List<int> trianglesAnt;
	private List<Vector3> verticesPost;
	private List<int> trianglesPost;
	public static List<Vector3> borderVerticesAnt;
	public static List<Vector3> borderVerticesPost;
	public static List<Vector3> borderVerticesAntIntersect;

	//indices sommets sauvegardés
	private int[,] indexes;

	
	private void initSavedIndexes(int[,] indexes) {
		for (int i = 0; i < indexes.GetLength(0); i++)
			for (int j = 0; j < indexes.GetLength(1); j++)
				this.indexes [i, j] = -1;
	}
	
	public void dataToMesh(float[] sommets, string type) {
		//Read the file as one string considering every 101 numbers as a sequence. Create the final array.
		
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();

		List<Vector3> borderVertices = new List<Vector3> ();

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
				vertices.Add(new Vector3(x, y, sommetCentre));
				this.indexes[i, j] = vertices.Count - 1;

				if((sommets[compteur - (NBSommets + 1)] != -1000 && 
				   sommets[compteur - 1] != -1000 &&
				   sommets[compteur - NBSommets] != -1000) ||
				   (sommets[compteur - NBSommets] != -1000 && 
				 sommets[compteur + 1] != -1000 &&
				 sommets[compteur + (1- NBSommets)] != -1000) ||
				   (sommets[compteur - 1] != -1000 && 
				 sommets[compteur + (NBSommets - 1)] != -1000 &&
				 sommets[compteur + NBSommets] != -1000) ||
				   (sommets[compteur + NBSommets] != -1000 && 
				 sommets[compteur + (1 + NBSommets)] != -1000 &&
				 sommets[compteur + 1] != -1000))
				{
					if(sommets[compteur - (NBSommets + 1)] == -1000 || i - 1 == 0 ||
					   sommets[compteur + (1- NBSommets)] == -1000 || j - 1 == 0 ||
					   sommets[compteur + (NBSommets - 1)] == -1000 || i + 1 == NBSommets - 1 ||
					   sommets[compteur + (1 + NBSommets)] == -1000 || j + 1 == NBSommets - 1 ||
					   sommets[compteur - NBSommets] == -1000 || sommets[compteur - 1] == -1000 ||
					   sommets[compteur + NBSommets] == -1000 || sommets[compteur + 1] == -1000)
						borderVertices.Add(new Vector3(x, y, sommetCentre));
				}

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
					
					vertices.Add(new Vector3(x - 0.05f, y - 0.05f, center));
					centerIndex = vertices.Count - 1;
					
					if(this.indexes[i - 1, j] == -1)
					{
						vertices.Add(new Vector3(x - 0.1f, y, sg2));
						this.indexes[i - 1, j] = vertices.Count - 1;

						// Detect the border of the mesh: if i - 1 == 0 || sg2 left == -1000: This is a border vertex
						if(i - 1 == 0 || j - 1 == 0 || sommets[compteur - 2 + NBSommets] == -1000 || sommets[compteur - 2] == -1000 || sommets[compteur - 2 - NBSommets] == -1000 || sommets[compteur - 1 + NBSommets] == -1000 || sommets[compteur + NBSommets] == -1000)
							borderVertices.Add(new Vector3(x - 0.1f, y, sg2));
					}
					if(this.indexes[i, j - 1] == -1)
					{
						vertices.Add(new Vector3(x, y - 0.1f, sb2));
						this.indexes[i, j - 1] = vertices.Count - 1;

						if(j - 1 == 0 || i - 1 == 0 || sommets[compteur - 1 - (2 * NBSommets)] == -1000 || sommets[compteur - (2 * NBSommets)] == -1000 || sommets[compteur + 1 - (2 * NBSommets)] == -1000 || sommets[compteur + 1] == -1000 || sommets[compteur + 1 - NBSommets] == -1000)
							borderVertices.Add(new Vector3(x, y - 0.1f, sb2));
					}
					if(this.indexes[i - 1, j - 1] == -1)
					{
						vertices.Add (new Vector3 (x - 0.1f, y - 0.1f, sbg3));
						this.indexes[i - 1, j - 1] = vertices.Count - 1;

						if(i - 1 == 0 || j - 1 == 0 || sommets[compteur - 2] == -1000 || sommets[compteur - 2 - (2 * NBSommets)] == -1000 || sommets[compteur - 2 - NBSommets] == -1000 || sommets[compteur - (2 * NBSommets)] == -1000 || sommets[compteur - 1 - (2 * NBSommets)] == -1000)
							borderVertices.Add(new Vector3(x - 0.1f, y - 0.1f, sbg3));
					}
					
					tmpTriangles = new int[]{this.indexes[i, j - 1], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i, j - 1],
						this.indexes[i, j - 1], this.indexes[i - 1, j - 1], centerIndex, centerIndex, this.indexes[i - 1, j - 1], this.indexes[i, j - 1],
						this.indexes[i - 1, j - 1], this.indexes[i - 1, j], centerIndex, centerIndex, this.indexes[i - 1, j], this.indexes[i - 1, j - 1],
						this.indexes[i - 1, j], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i - 1, j]};
					triangles.AddRange(tmpTriangles);
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
					
					vertices.Add(new Vector3(x + 0.05f, y - 0.05f, center));
					centerIndex = vertices.Count - 1;
					vertices.Add(new Vector3(x + 0.1f, y, sd2));
					this.indexes[i + 1, j] = vertices.Count - 1;

					if(i + 1 == NBSommets - 1 || j - 1 == 0 || sommets[compteur + 2 + NBSommets] == -1000 || sommets[compteur + 2] == -1000 || sommets[compteur + 2 - NBSommets] == -1000 || sommets[compteur + NBSommets] == -1000 || sommets[compteur + NBSommets + 1] == -1000)
						borderVertices.Add(new Vector3(x + 0.1f, y, sd2));
					
					if(this.indexes[i + 1, j - 1] == -1)
					{
						vertices.Add (new Vector3 (x + 0.1f, y - 0.1f, sbd3));
						this.indexes[i + 1, j - 1] = vertices.Count - 1;

						if(i + 1 == NBSommets - 1 || j - 1 == 0 || sommets[compteur + 2] == -1000 || sommets[compteur + 2 - (2 * NBSommets)] == -1000 || sommets[compteur + 2 - NBSommets] == -1000 || sommets[compteur - (2 * NBSommets)] == -1000 || sommets[compteur + 1 - (2 * NBSommets)] == -1000)
							borderVertices.Add(new Vector3(x + 0.1f, y - 0.1f, sbd3));
					}
					if(this.indexes[i, j - 1] == -1)
					{
						vertices.Add(new Vector3(x, y - 0.1f, sb2));
						this.indexes[i, j - 1] = vertices.Count - 1;

						if(j - 1 == 0 || i + 1 == NBSommets - 1 || sommets[compteur - 1 - (2 * NBSommets)] == -1000 || sommets[compteur - (2 * NBSommets)] == -1000 || sommets[compteur + 1 - (2 * NBSommets)] == -1000 || sommets[compteur - 1] == -1000 || sommets[compteur - 1 - NBSommets] == -1000)
							borderVertices.Add(new Vector3(x, y - 0.1f, sb2));
					}
					
					tmpTriangles = new int[]{this.indexes[i, j], this.indexes[i + 1, j], centerIndex, centerIndex, this.indexes[i + 1, j], this.indexes[i, j],
						this.indexes[i + 1, j] , this.indexes[i + 1, j - 1], centerIndex, centerIndex, this.indexes[i + 1, j - 1], this.indexes[i + 1, j],
						this.indexes[i + 1, j - 1], this.indexes[i, j - 1], centerIndex, centerIndex, this.indexes[i, j - 1], this.indexes[i + 1, j - 1],
						this.indexes[i, j - 1], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i, j - 1]};
					triangles.AddRange(tmpTriangles);
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
					
					vertices.Add(new Vector3(x - 0.05f, y + 0.05f, center));
					centerIndex = vertices.Count - 1;
					vertices.Add(new Vector3(x, y + 0.1f, sh2));
					indexes[i, j + 1] = vertices.Count - 1;

					if(j + 1 == NBSommets - 1 || i - 1 == 0 || sommets[compteur - 1 + (2 * NBSommets)] == -1000 || sommets[compteur + (2 * NBSommets)] == -1000 || sommets[compteur + 1 + (2 * NBSommets)] == -1000 || sommets[compteur + 1] == -1000 || sommets[compteur + 1 + NBSommets] == -1000)
						borderVertices.Add(new Vector3(x, y + 0.1f, sh2));
					
					if(this.indexes[i - 1, j] == -1)
					{
						vertices.Add(new Vector3(x - 0.1f, y, sg2));
						this.indexes[i - 1, j] = vertices.Count - 1;

						if(i - 1 == 0 || j + 1 == NBSommets - 1 || sommets[compteur - 2 + NBSommets] == -1000 || sommets[compteur - 2] == -1000 || sommets[compteur - 2 - NBSommets] == -1000 || sommets[compteur - NBSommets] == -1000 || sommets[compteur - 1 - NBSommets] == -1000)
							borderVertices.Add(new Vector3(x - 0.1f, y, sg2));
					}
					if(indexes[i - 1, j + 1] == -1)
					{
						vertices.Add(new Vector3(x - 0.1f, y + 0.1f, shg1));
						indexes[i - 1, j + 1] = vertices.Count - 1;

						if(i - 1 == 0 || j + 1 == NBSommets - 1 || sommets[compteur + (2 * NBSommets)] == -1000 || sommets[compteur - 2 + (2 * NBSommets)] == -1000 || sommets[compteur - 1 + (2 * NBSommets)] == -1000 || sommets[compteur - 2] == -1000 || sommets[compteur - 2 + NBSommets] == -1000)
							borderVertices.Add(new Vector3(x - 0.1f, y + 0.1f, shg1));
					}
					
					tmpTriangles = new int[]{this.indexes[i, j], this.indexes[i - 1, j], centerIndex, centerIndex, this.indexes[i - 1, j], this.indexes[i, j],
						this.indexes[i - 1, j], this.indexes[i - 1, j + 1], centerIndex, centerIndex, this.indexes[i - 1, j + 1], this.indexes[i - 1, j],
						this.indexes[i - 1, j + 1], this.indexes[i, j + 1], centerIndex, centerIndex, this.indexes[i, j + 1], this.indexes[i - 1, j + 1],
						this.indexes[i, j + 1], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i, j + 1]};
					triangles.AddRange(tmpTriangles);
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
					
					vertices.Add(new Vector3(x + 0.05f, y + 0.05f, center));
					centerIndex = vertices.Count - 1;
					vertices.Add (new Vector3 (x + 0.1f, y + 0.1f, shd1));
					indexes[i + 1, j+ 1] = vertices.Count - 1;

					if(i + 1 == NBSommets - 1 || j + 1 == NBSommets - 1 || sommets[compteur + (2 * NBSommets)] == -1000 || sommets[compteur + 2 + (2 * NBSommets)] == -1000 || sommets[compteur + 1 + (2 * NBSommets)] == -1000 || sommets[compteur + 2] == -1000 || sommets[compteur + 2 + NBSommets] == -1000)
						borderVertices.Add(new Vector3(x + 0.1f, y + 0.1f, shd1));
					
					if(indexes[i, j + 1] == -1)
					{
						vertices.Add(new Vector3(x, y + 0.1f, sh2));
						indexes[i, j + 1] = vertices.Count - 1;

						if(i + 1 == NBSommets - 1 || j + 1 == NBSommets - 1 || sommets[compteur - 1 + (2 * NBSommets)] == -1000 || sommets[compteur + (2 * NBSommets)] == -1000 || sommets[compteur +1 + (2 * NBSommets)] == -1000 || sommets[compteur - 1] == -1000 || sommets[compteur - 1 + NBSommets] == -1000)
							borderVertices.Add(new Vector3(x, y + 0.1f, sh2));
					}
					if(this.indexes[i + 1, j] == -1)
					{
						vertices.Add(new Vector3(x + 0.1f, y, sd2));
						this.indexes[i + 1, j] = vertices.Count - 1;

						if(i + 1 == NBSommets - 1 || j + 1 == NBSommets - 1 || sommets[compteur + 2 + NBSommets] == -1000 || sommets[compteur + 2] == -1000 || sommets[compteur + 2 - NBSommets] == -1000 || sommets[compteur - NBSommets] == -1000 || sommets[compteur - NBSommets + 1] == -1000)
							borderVertices.Add(new Vector3(x + 0.1f, y, sd2));
					}
					
					tmpTriangles = new int[]{this.indexes[i, j], this.indexes[i, j + 1], centerIndex, centerIndex, this.indexes[i, j + 1], this.indexes[i, j],
						this.indexes[i, j + 1], this.indexes[i + 1, j + 1], centerIndex, centerIndex, this.indexes[i + 1, j + 1], this.indexes[i, j + 1],
						this.indexes[i + 1, j + 1], this.indexes[i + 1, j], centerIndex, centerIndex, this.indexes[i + 1, j], this.indexes[i + 1, j + 1],
						this.indexes[i + 1, j], this.indexes[i, j], centerIndex, centerIndex, this.indexes[i, j], this.indexes[i + 1, j]};
					triangles.AddRange(tmpTriangles);
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
		if(type.CompareTo("ant") == 0)
		{
			this.verticesAnt = vertices;
			this.trianglesAnt = triangles;
			borderVerticesAnt = borderVertices;
		}
		else if(type.CompareTo("pos") == 0)
		{
			this.verticesPost = vertices;
			this.trianglesPost = triangles;
			borderVerticesPost = borderVertices;
		}
	}

	public List<Vector3> getVerticesAnt() {
		return this.verticesAnt;
	}
	
	public List<int> getTrianglesAnt() {
		return this.trianglesAnt;
	}

	public List<Vector3> getVerticesPost() {
		return this.verticesPost;
	}
	
	public List<int> getTrianglesPost() {
		return this.trianglesPost;
	}
}

