using UnityEngine;
using System.Collections.Generic;

public class CorneaManager
{
	private CorneaMeshCreate corneaMesh;
	private Pachymetry pachymetryMesh;

	public CorneaManager(string type) {
		if(type.CompareTo("ant") == 0)
		{
			this.corneaMesh = new CorneaMeshCreate ();
			this.corneaMesh.dataToMesh (ImportData.anteriorDatas, "ant");
		}
		else if(type.CompareTo("post") == 0)
		{
			this.corneaMesh = new CorneaMeshCreate ();
			this.corneaMesh.dataToMesh (ImportData.posteriorDatas, "pos");
		}
		else if(type.CompareTo("pach") == 0)
		{
			this.pachymetryMesh = new Pachymetry();
			this.pachymetryMesh.dataToMesh(ImportData.pachymetryDatas);
		}

	}
	
	public List<Vector3> getVerticesAnt() {
		return this.corneaMesh.getVerticesAnt ();
	}
	
	public List<int> getTrianglesAnt() {
		return this.corneaMesh.getTrianglesAnt ();
	}

	public List<Vector3> getVerticesPost() {
		return this.corneaMesh.getVerticesPost ();
	}
	
	public List<int> getTrianglesPost() {
		return this.corneaMesh.getTrianglesPost ();
	}

	public List<Vector3> getVerticesPach() {
		return this.pachymetryMesh.getVerticesPach ();
	}

	public List<int> getTrianglesPach() {
		return this.pachymetryMesh.getTrianglesPach ();
	}

	public List<float> getElevationPach() {
		return this.pachymetryMesh.getElevationPach ();
	}
}


