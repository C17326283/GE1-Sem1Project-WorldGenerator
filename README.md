
# Low Poly planet Generator

Ryan Byrne

C17326283 

Dt211c/4

# Description of the project
This project procedurally generated teh mesh of a planet and turns it into a sphere then adds noise to it to create terrain.
It uses shaders for all the terrain colouring
It allows for user customisation

# Instructions for use
You can download the project and compile it in unity or unzip the FinalBuild.zip file
Load the .exe
Press the start button
You can then input parameters as you like, clicking generate to see the results
When you are happy you can hit explore
Then you can move around the planet with mouse and W,A,S,D

# How it works
There is a Planet generator script that acts as a game manager with all the parameters that the planet needs such as lists of object groups to spawn and shaders etc
It then makes a planet object.

This planet object makes all of the verticies and triangles for each side of a cube. The cube face then has all of its points normalized and deformed to create a spherical terrain.
A second sphere is also spawned with this but doesnt have noise added so this can act as water.

The deformation is based on the parameters in a scriptable object. It also holds noise layers which has the noise parameters such as strength and roughness.
This uses layers so variations of terrains can be made such as large contenents with spiky mountains.

The planet spawner script is also changed by the gui which allows the users to set custom parameters.
Once the terrain is ready the planet has objects generated onto it.

This just has a which moves around a radius outside the atmosphere and sends a raycast to the center of the planet.
If it hits land then it spawns an object from the object pool. There are multiple spawners and pools with different rules such as clouds spawn in the sky where as regular objs spawn at raycast hit point.

The terrain is shaded with a custom shader.
This shader uses the height level from the center to colour different levels.
Biome objects are also spawned around the planet and the shader uses the distance from these to determine which colour pallete it uses and blends that colours together on the material.
It also flattens the normals of the material to create the low poly effect.

There are some other objects in the scene that help such as an atmosphere which is an inward facing sphere with a custom shader, Lighting objects which are just directional but they rotate for a day night cycle as well as having a child sun object far away matching the direction of light and the player camera which is locked at the beginning untill the player completes the generation.

# Proposal submitted earlier can go here:
This is a project to procedurally generate a planet.
It will edit the verticies to create different terrain levels.
It will have multiple different biomes and evironments.

# References
I followed this tutorial to get started but changed a lot of how it works: https://www.youtube.com/watch?v=QN39W020LqU&list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8
I used some scripts from old projects as well to get started such as the tree spawners but needed to heavily modify it.
I used low poly assets from the asset store for the objects that get spawned on the terrain and the clouds
i used a skybox from the asset store


# What am i most proud of
There were a lot of things in this project that were unexpectedly difficult and i am proud of a lot but i am most proud of the mesh generation as
this was a very difficult concept for me at first and i eventually got it the way i wanted it
I am also quite proud of the shader work in this. I spent a long time figuring out shader graph to colour based on height relative to a sphere and incorporate biomes.
I also like the aesthetic of the sun rising over the low poly world

# Issues, Difficulties and ideas
There were a couple issues that i faced that i want to highlight and they may be added in future. 
I tried to split the mesh down further for setting the biomes by face but this proved difficult to place smaller meshes that weres full faces of a cube or splitting the full faces caused missign verticies.
After over a day trying to fix this i decided to try some other methods and i added objects that i could center biomes around. This took a lot of work in shader graph to colour one mesh based on distances between multiple other objects and blend all the textures onto one but in the end i think it came out better than my original plan. This solution allows my future animals to get the distance of the closest biome using those objects and have different behaviours as well as i can spawn biome specific objects based on the closest biome point.

The atmosphere was also a tough thing to implement procedurally as it requires spawning a primitive sphere based on the height point on the terrain and then flipping the normals to point inward so an appropriate effect can be achieved but im happy with it in the end.

I had ai driven animals that i was hoping to add to this but i didnt have enough time but they will be added in the future.

I tried to implement a gravity based player movement but this also took far too long and was causing a lot of issues so i left it out for now.

A huge issue i had was with the collision meshes not generating correctly and took some workaround to fix, apparently it is a possible bug with the latest version but i found setting the mesh to convex and back along with some other mesh handling fixed it which allowed the terrain to spawn the objects correctly. 

I could also add a way for the terrain to pick from a list of possible gradient textures for the biome colouring. This wouldnt be too hard it just didnt fit within the scope of this.

I could also have added more of the settings that are used for the planet generation such as radius or water height but i decided to keep the ui and customisation simple.




# Demonstration
[![YouTube](http://img.youtube.com/vi/J2kHSSFA4NU/0.jpg)](https://www.youtube.com/watch?v=XY-E6R6fm3Q&feature=youtu.be)

I cleaned up the project after this video and noticed i forgot to configure the post processing which was added after.

