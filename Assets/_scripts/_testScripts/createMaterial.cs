using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMaterial : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D texf = Resources.Load ("Buildings/front/1") as Texture2D;
		Texture2D texs = Resources.Load ("Buildings/side/1") as Texture2D;
		Material[] all = new Material[3];
		levelController levcon = GameObject.FindGameObjectWithTag ("GameController").GetComponent<levelController> ();

		transform.localScale = new Vector3 (transform.lossyScale.x, 1,transform.lossyScale.y * (texf.height / texf.width));
		all [0] = buildingMaterialImporter.getBuildingMaterial(ref levcon.wallTextureCache);
		all[1] = buildingMaterialImporter.getBuildingMaterial(ref levcon.wallTextureCache, "Buildings/front/1", 0.3f, 0.33f);
		all[2] = buildingMaterialImporter.getBuildingMaterial(ref levcon.wallTextureCache, "Buildings/side/1", 0.3f, 0.33f);
		transform.GetComponent<MeshRenderer> ().materials = all;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
