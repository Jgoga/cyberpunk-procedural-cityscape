# cyberpunk-procedural-cityscape
Study to draw metallic and bump map textures from template and also creating random lit windows.

I did not get it working in browser as texture creation is too memory heavy for browsers, but it is downloadable at:

This repository contains the asset folder without imported assets to show the code and image assets used and the unitypackage file to import it for own use.

Game binaries:
https://aarreentertainment.itch.io/procedural-cityscape-study

Here's an overview what the code does
## /Assets/_scripts/game_logic/levelController.cs

This is basically the main game logic, which is searchable by the tag 'GameController', to access the globals such as wallTextureCache and the buildingPool object buildings.

## /Assets/_scripts/game_logic/createLevel.cs

This will draw textured buildings on a cube to make a floating cityscape. The main use would be for a 2.5d game with scrolling background.

It uses the static buildingMaterialImporter to texture itself with a generic bump texture which is normally used to render roof textures.

It creates a grid of its' own size and draws a building on each grid point with a 20% chance. The script which creates the buildings is buildingPool, which has a size of 35 unique, randomized buildings.

## /Assets/_scripts/Procedural_generation/buildingPool.cs

BuildingPool has two functions, getBuilding and AddBuilding. The former creates a building with the latter if the object's pool size is below 35 as it is inherited from the List<GameObject> class.

AddBuilding loads the cube mesh used for buildings, and uses buildingMaterialImporter to create materials for front, sides and top/bottom. It also scales the cube according to the aspect ratio of the original image.

It searches the levelController object to reference the wall texture cache so there is no need to render every single wall texture, because they produce similar results with the source, with the only exception of emission maps.

## /Assets/_scripts/Procedural_generation/buildingMaterialImporter.cs

This is the hardest worker of all scripts. The main function returns a material and runs the source texture through converter functions. If the source name is not defined, it will create a roof texture.

The first thing it does after creating an empty material is look if textures already exist in the texture pool (I just noticed that emission search is useless, but I coded this rather quickly) and if a unique one with the same name, which I am using as an ID, exists, it uses that instead of creating a new one, which is very memory heavy, as it seems.

sourceToBumpMap creates a grayscale bump map image, which reads from the source items' colors. If it is red, it draws noise (random.value), and if it is green, it draws a deeper window, and as blue a door in between.

sourceToEmission creates an emission map, which overrides other textures and is used in texture lights. It randomizes the window color and starts randomly coloring a window if it is in the green parts' corner pixel (it checks top and left neighbours). If the neighbor is colored in the window color, it continues filling the pixels if they match the window placeholder color of the source image.

sourceToMetallic creates a metallic map to make the window reflection effect and difference from the wall in general. It is straightforward and draws different color placeholders with a differing setup.

bumpMapToNormalMap is my so far only copy pasted bit. It creates a normal map texture from the bump map, and researching how normal map conversions work would have been too much of trouble in a project which isn't exclusively normal map conversion.

##Known issues

Windows seem similar. Something in the randomization is causing issues.

The buildings after the first row seem... samey. It is probably related to pool size and randomization, but I must research it further.

Texture creation is SLOW. I need to explore other methods in creating the images. One issue might be in setPixel, for which an alternative would be to create a one dimensional array and use setPixels for the texture.