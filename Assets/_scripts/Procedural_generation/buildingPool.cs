using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingPool : List<GameObject> {

	public void AddBuilding(string sideTexture, string frontTexture){
		GameObject mesh = Resources.Load ("buildingCube") as GameObject;

		Material[] all = new Material[3];
		levelController levcon = GameObject.FindGameObjectWithTag ("GameController").GetComponent<levelController> ();

		all [0] = buildingMaterialImporter.getBuildingMaterial(ref levcon.wallTextureCache);
		all[1] = buildingMaterialImporter.getBuildingMaterial(ref levcon.wallTextureCache, "Buildings/front/"+frontTexture, 0.3f, 0.33f);
		all[2] = buildingMaterialImporter.getBuildingMaterial(ref levcon.wallTextureCache, "Buildings/side/"+sideTexture, 0.3f, 0.33f);
		mesh.transform.localScale = new Vector3 (mesh.transform.lossyScale.x, 1,mesh.transform.lossyScale.y * (all[1].GetTexture("_BumpMap").height / all[1].GetTexture("_BumpMap").width));
		mesh.transform.GetComponent<MeshRenderer> ().materials = all;
		this.Add (mesh);
	}

	public GameObject getBuilding(){
		if(this.Count<35){

			Object[] frontTextures = Resources.LoadAll ("Buildings/front", typeof(Texture2D));
			Object[] sideTextures = Resources.LoadAll ("Buildings/side", typeof(Texture2D));
			AddBuilding (frontTextures [Random.Range (0, frontTextures.Length)].name, sideTextures [Random.Range (0, sideTextures.Length)].name);
		}
		return this [Random.Range (0, this.Count)];
	}
}
