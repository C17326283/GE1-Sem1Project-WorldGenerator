
# Low Poly planet Generator

Ryan Byrne

C17326283 

Dt211c/4

# Description of the project
This project procedurally generated teh mesh of a planet and turns it into a sphere then adds noise to it to create terrain.
It uses shaders for all the terrain colouring
It allows for user customisation

# Instructions for use
Load the .exe and press generate.
You can then input parameters as you like, clicking generate to see the results
When you are happy you can hit explore
Then you can move around the planet to see the details

# How it works
There is a Planet generator script that acts as a game manager with all the parameters that the planet needs such as lists of objetc groups to spawn and shaders etc
It then makes a planet object
This planet object makes all of the verticies and triangles for each side of a cube. The cube face then has all of its points normalized and deformed to create the terrain.
A second cube is also spawned with this but doesnt have noise added so this can act as water.
The deformation is based on the parameters in a scripatble object. It also holds noise layers which has the noise parameters such as strength and roughness.
This uses layers so variations of terrains can be made such as large contenents with spiky mountains.
The planet spawner script is also changed by the gui which allows the users to set custom parameters.
Once the terrain is ready the planet has objects generated onto it.
This just has a which moves around a radius outside the atmosphere and sends a raycast to the center of the planet.
If it hits land then it spawns an object from the object pool. There are multiple spawners and pools with different rules such as clouds spawn in the sky where as regular objs spawn at raycast hit point.
The terrain is shaded with a custom shader.
This shader uses the height level from the center to colour different levels.
Biome objects are also spawned around the planet and the shader uses the distance from these to determine which colour pallete it uses and blends that colours together on the material.
It also flattens the normals of the material to create the low poly effect.

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
There were a lot of things in this project that were unexpectedly difficult but i am most proud of the mesh generation
This was a very difficult concept for me at first and i eventually got it the way i wanted it
I am also quite proud of the shader work in this. I spent a long time figuring out shader graph to colour based on height relative to a sphere and incorporate biomes.
I also like the aesthetic of the sun rising over the low poly world



# Demonstration
[![YouTube](http://img.youtube.com/vi/J2kHSSFA4NU/0.jpg)](https://www.youtube.com/watch?v=XY-E6R6fm3Q&feature=youtu.be)

