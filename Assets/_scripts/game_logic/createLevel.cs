using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().material = buildingMaterialImporter.getBuildingMaterial(ref GameObject.FindGameObjectWithTag ("GameController").GetComponent<levelController> ().wallTextureCache);
		Vector2 size = new Vector2 (transform.localScale.x, transform.localScale.z);
		//GameObject building = Resources.Load ("buildingCube") as GameObject;

		float gridX = size.x / 40;
		float gridY = size.y / 40;
		float buildingChance = 0.2f;
		for(int x = 2; x< size.x/gridX -2; x++){
			for(int y = 2; y< size.y/gridY -2; y++){
				if (Random.value < buildingChance && x % 6 != 0) {
					GameObject bldg = Instantiate (GameObject.FindGameObjectWithTag ("GameController").GetComponent<levelController> ().buildings.getBuilding (), transform);
					bldg.transform.localScale = bldg.transform.localScale / 100;
					bldg.transform.position = new Vector3 (-transform.localScale.x / 2 + x * gridX, bldg.transform.lossyScale.z, -gridY*2+ y * gridY);
					bldg.transform.Rotate (0, 0, 0 + Random.Range (0, 4) * 90);
				}
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
