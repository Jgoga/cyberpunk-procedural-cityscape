using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class buildingMaterialImporter {



	public static Material getBuildingMaterial(ref List<Texture2D> cache, string src = null, float bumpiness = 0.2f, float windowChance = 0.2f) {

		Texture2D source;

		if (src == null) {
			src = "top";
			source = new Texture2D (512, 512);
			for (int y = 0; y < source.height; y++) {
				for (int x = 0; x < source.width; x++) {
					source.SetPixel (x, y, new Color (1, 0, 0, 1.0f));
				}
			}
		} else {
			source = Resources.Load (src) as Texture2D;
		}



		Material ret = new Material( Shader.Find("Standard"));
		ret.CopyPropertiesFromMaterial (Resources.Load ("buildingMaterial") as Material);
		Texture2D bump = null;
		Texture2D emission = null;
		Texture2D metallic = null;
		foreach (Texture2D tex in cache) {
			if (tex.name == src.Replace ('/', '_') + "_BumpMap")
				bump = tex;
			if (tex.name == src.Replace ('/', '_') + "_EmissionMap")
				emission = tex;
			if (tex.name == src.Replace ('/', '_') + "_MetallicGlossMap")
				metallic = tex;
		}
		if (bump == null) {
			bump = bumpMapToNormalMap (sourceToBumpMap (source, bumpiness));
			bump.name = src.Replace ('/', '_') + "_BumpMap";
			cache.Add (bump);
		}
		ret.SetTexture ("_BumpMap", bump);


		ret.SetTexture ("_EmissionMap", sourcetoEmission(source, windowChance));

		if (metallic == null) {
			metallic = sourcetoMetallic (source);
			metallic.name = src.Replace ('/', '_') + "_MetallicGlossMap";
			cache.Add (metallic);
		}
		ret.SetTexture ("_MetallicGlossMap", metallic);
		ret.EnableKeyword ("_NORMALMAP");
		ret.EnableKeyword ("_METALLICGLOSSMAP");
		return ret;
	}


	#region textureCreationScripts

	//create bump map from source texture. Red is wall with noise, green is window, blue is door
	static Texture2D sourceToBumpMap(Texture2D texSource, float bumpiness){
		Texture2D bumpMap = new Texture2D (texSource.width, texSource.height);
		for (int y = 0; y < texSource.height; y++) {
			for (int x = 0; x < texSource.width; x++) {
				float bval = 0;
				Color pixel = texSource.GetPixel (x, y);
				//check pixel color
				if (pixel.r > 0.75f)
					bval = Random.value * bumpiness;		
				if (pixel.g > 0.75f)
					bval = 0.01f;
				if (pixel.b > 0.75f)
					bval = 0.03f;

				bumpMap.SetPixel(x,y, new Color(bval, bval, bval ,1.0f));
			}
		}
		bumpMap.Apply ();
		return bumpMap;
	}

	//Create emission map, which means lit windows
	static Texture2D sourcetoEmission(Texture2D texSource, float windowChance){

		Color windowColor = new Color (1 - Random.value / 2, 1 - Random.value / 2, 1 - Random.value / 2);
		Texture2D emissMap = new Texture2D (texSource.width, texSource.height);
		Color[,] emissionMap = new Color[texSource.width, texSource.height];
		for (int y = 0; y < texSource.height; y++) {
			for (int x = 0; x < texSource.width; x++) {
				emissionMap [x, y] = new Color (0, 0, 0, 1.0f);
				if (x > 0 && y > 0) {
					//check if pixel is upper corner of window and randomize;
					if (texSource.GetPixel (x, y).g > 0.75f &&
					     texSource.GetPixel (x, y - 1).r > 0.25f &&
					     texSource.GetPixel (x - 1, y).r > 0.25f &&
					     Random.value < windowChance) {
						emissionMap [x, y] = windowColor;
					}
					//check if window is already lit. If it is, make window yellow, otherwise black.
					if (texSource.GetPixel (x, y).g > 0.75f && 
						(emissionMap [x - 1, y]==windowColor || emissionMap [x, y - 1]==windowColor)
					) {
						emissionMap [x, y] = windowColor;
					} 

				} 
				emissMap.SetPixel (x, y, emissionMap [x, y]);
			}
		}
		emissMap.Apply ();
		return emissMap;
	}

	static Texture2D sourcetoMetallic(Texture2D texSource){
		Texture2D metalMap = new Texture2D (texSource.width, texSource.height);
		for (int y = 0; y < texSource.height; y++) {
			for (int x = 0; x < texSource.width; x++) {
				metalMap.SetPixel (x, y, new Color(0.2f, 0.2f, 0.2f, 0.4f));
				if (x > 0 && y > 0) {
					if (texSource.GetPixel (x, y).g > 0.5f) {
						metalMap.SetPixel (x, y, new Color (0.3f, 0.3f, 0.3f, 1));
					}
					if (texSource.GetPixel (x, y).b > 0.5f) {
						metalMap.SetPixel (x, y, new Color (0.3f, 0.3f, 0.3f, 0.1f));
					}
				} 
			}
		}
		metalMap.Apply ();
		return metalMap;
	}

	//Creates normal map from black and white texture
	static Texture2D bumpMapToNormalMap(Texture2D bumpSource){
		Texture2D bumpTexture = new Texture2D (bumpSource.width, bumpSource.height, TextureFormat.ARGB32, false);
		for (int y = 0; y < bumpTexture.height; y++) {
			for (int x = 0; x < bumpTexture.width; x++) {
				float xLeft = bumpSource.GetPixel (x - 1, y).grayscale;
				float xRight = bumpSource.GetPixel (x + 1, y).grayscale;
				float yUp = bumpSource.GetPixel (x, y - 1).grayscale;
				float yDown = bumpSource.GetPixel (x, y + 1).grayscale;
				float xDelta = ((xLeft - xRight) + 1) * 0.5f;
				float yDelta = ((yUp - yDown) + 1) * 0.5f;
				bumpTexture.SetPixel(x,y,new Color(xDelta,yDelta,1.0f,1.0f));
			}
		}
		bumpTexture.Apply ();
		return bumpTexture;
	}
	#endregion

}
