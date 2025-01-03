SOURCE: https://forum.matfyzak.cz/viewtopic.php?f=422&t=11703

## Give an overview of the relative advantages and disadvantages of procedural and image-based textures.

**Procedural textures**:
unlimited resolution, both 2D and 3D possible, very customizable pattern wise, cheap to copy and alter details - not huge in memory after instancing too many times
**Textures**:
Anti-aliased, editable for artists, no need for mathematical representation of patterns for artists, 2D only, instancing takes a lot of memory, hard to edit as a whole

## What basic building block functions for shaders do you know?

step, smoothstep, pulse ( = step(further) - step(closer))
noises - 2D noise, 3D noise, different parameters and variants (parametric, cellnoise, …)
fBm - fractal brownian motion = addition of different noises (different amplitude and frequency)
math functions (such as mod for periodicity)

## How would one write a plain (non-antialiased) RM shader that yields a grid pattern?

Ci = input color
horizontal*line_indicator = pulse(0.45, 0.55, mod(s * freq, 1))
vertical*line_indicator = pulse(0.45, 0.55, mod(t * freq, 1))
Ci = Ci _ (1 - h_l_i) _ (1 - v_l_i)

## Describe noise shaders what types are there, what are their properties, how can one implement them, how does one control them?

Types: 2D/3D, cell/continuous, periodic/non-periodic, returning color/returning a float, parametrized by a number/by a coordinate pair (u,v), snoise that is remapped to [-1,1]
Control: Mainly through amplitude ( = amplitude _ snoise() ) and frequency ( = snoise(freq _ x))
Implementation: noise is standard, snoise = 2 * noise(x) - 1, fbm = sum over i snoises(x) where the amplitude gets /2 and frequency *2 in every iteration, turbulence is implemented as fBm except for abs(snoises(x))

## Describe Voronoi cell patterns: how can they be implemented, what sort of structures can they yield? What issues does one have to be aware of when implementing them?

Voronoi cell pattern divides the plane space (like (u,v)) into cells. Each cell contains all pixels that are closest to the cell’s corresponding point.
Implementation: We use cellnoise. For each pixel, we check the distance to its 9 neighbouring cellnoise cells in 2D (or 27 cells in 3D) (precisely, to a random point within that cell), compute the closest one and choose it. For some patterns, we use both f1 and f2 (1st closest and 2nd closest point), e.g., to create cracks (cracks = where points lie in the middle between f1 and f2).
Structures: Mainly natural phenomena like cells in biology, rock formations, drying dirt or stones in pavements.
Issues: Thickness of the border (compensation factor exists, normalizing the distance between f2 and f1), 2D vs 3D - a correct voronoi on a 2D slice of the object is not the same as a slice from a correct 3D voronoi of the whole object

## 6. What are Wang tiles, and what are they used for? What sort of patterns can one generate with them? How does one generate Wang tiles from arbitrary input images? What issues does this technique have?

Unit square tiles with colored edges and fixed orientation, the complete wang tile set with C colours contains C^4 tiles.
They are used to generate large textures in which we cannot see a regular pattern and which would be expensive to create by hand. They are also used for generating samples in a Poisson distribution.
Patterns are generated by a scanline algorithm going West to East, North to South, choosing the next tile at random (from the set of matching tiles). We can also use hash functions, so we do not need to generate the whole large texture, but just a part of it.

Issues: There is a corner problem, because Wang tiles only constrain the 4 neighbors. When the tiles have noticeable features in their corners, we need to extend the tileset with marked corners or just use corner tiles?

## 7. What are corner tiles, and in what regard are they more powerful than Wang tiles?

They have colored also corners in addition to edges.

## 8. What are Poisson disk patterns? How can one generate them, what are application areas, and what is their relationship to Wang tiles? How does one generate them for 3D patterns?

Poisson disk pattern is a distribution of random points in a hyperplane, parametrized by d, where each point satisfies the condition that there are no other points in a d diameter around the point.
Application areas mainly include “random” sampling in rendering or texture filtering. Where we want a random, but “even” distribution (pseudorandom). This also corresponds to placing natural objects (e.g., sunflowers on a field) in a random, but not distracting way using Wang tiles / Poisson distribution.

Wang tiles can be used to generate Poisson patterns, using square tiles for 2D or cubes for 3D. Even though corner tiles are more suitable. This is done in multiple steps, all of which consist of dart throwing, relaxation (using Voronoi diagram centroids of the generated points, then substituting the points for the centers of the voronoi cells, and repeating), and clipping. In 2D, it is: corners, then horizontal and vertical edges, and finally the interior. In 3D, we additionally need to constraint the faces of the cubes.

## 9. Describe techniques for creating realistic-looking ocean surfaces.

Using spectral analysis of ocean movements. The spectra are seamlessly tileable.

## 10. Discuss the modeling potential of layered surfaces. What sort of surfaces can be described by this sort of approach?

Light-shader from RENDERMAN can add rust, freeze, graffiti, hole, and wet at the same time.

## 11. Describe statistical glitter modeling, as well as the earlier work on static glitter models using Voronoi textures, which were shown in the lecture. Discuss the differences in capability and appearance between these models.

Voronoi, Microfacets, high-resolution normal map

## 12. What coordinate systems are there in classic REYES RM shading language?

By default, shaders use a “current” space, which is implementation-defined.
There are 3 major coordinate spaces: world space, object space, and shader space.
When shading objects (e.g., procedural 3D marble texture), always prefer shader space, because in object space, all objects would look identical, and in world space, the texture would change on moving objects.
Generally, there are many more coordinate systems, though: (but are they available in the shaders?)
Pixar RenderMan recognizes these coordinate systems (you can use them in RIB)
object - the system of the current object
world - the global reference system
camera - at origin, view towards positive Z
screen - 2D system in the image plane
raster - the 2D system at pixel level
NDC - normalised device coordinates: like 'raster' space, with extent of [0..1],[0..1]

## 13. Give an overview of the classic REYES RenderMan rendering pipeline.

RIB parsing generates geometrical primitives from a .rib file.
Bound. Calculate the bounding volume of each geometric primitive. In case the primitive is behind eye space, or completely outside screen space, cull the primitive.
Split. Split large primitives into smaller, diceable primitives.
Dice. Convert the primitive into a grid of micropolygons, each approximately the size of a pixel. (Usually we try to have 4 micropolygons in a pixel according to the sampling theorem, i.e., 2 per width, 2 per height)
Shade. Calculate lighting and shading at each vertex of the micropolygon grid.
Bust the grid into individual micropolygons, each of which is bounded and checked for visibility.
Hide. Sample the micropolygons from all pixels that the micropolygon might be in, if we do not hit the micropolygon from a pixel, we do not add its contribution, otherwise we check the z of the micropolygon and compare with the pixel in the buffer. If the new z is closer to the camera, we replace the pixel with the new color, producing the final 2D image.

Note that dicing levels can be different for each part of the split object, there is also extensive culling going on - if the micropolygon does not intersect the visible region, it gets discarded. Primitives get split as a whole, no clipping.

## 14. What types of shaders are there in classic REYES RM?

    Light shaders, surface shaders, displacement shaders, imager shaders, atmosphere shaders, volume shaders (inside and outside the volume, 2 shaders), transformation shaders

## 15. What is the structure of a RIB file? What is the difference between options and attributes? How can one include additional complex geometry? What is the support for levels of detail?

```
<Options />
<World declaration>
	<Attributes />
</World declaration>
```

**Options** are global and set for the whole duration of rendering. Image size, shading rate, extension of the result (.tiff), etc. They are frozen once the World declaration begins and can’t change.
**Attributes** are local to their scope = “AttributeBegin” creates a snapshot of current settings, and after “AttributeEnd” restores that snapshot. The change do not leak outside of this context. Color, opacity, surface shader used, translations and rotations of the scene.
**Including additional geometry**:

- ReadArchive “...rib” (works like #include in C),
- ObjectBegin, ObjectEnd + ObjectInstance,
- Procedural, which accepts an AABB as an argument,
  - Procedural “DelayedReadArchive” filename + AABB,
  - Procedural “RunProgram” generator AABB, which runs an external program generating the geometry if AABB is on screen,
  - Procedural “DynamicLoad” generator AABB, which loads on object file generating the geometry if AABB is on screen

**Level of detail**:
Supports seamless blending between detail levels. Detail xmin, ymin, zmin, xmax, ymax, zmax specifies AABB size + DetailRange S L H E specifies blending thresholds. Basically, we use a combination of DetailRange + Procedural.
**AttributeBegin**
**Detail** [ -1 -1 -1 1 1 1 ] ← LOD AABB for the object
**DetailRange** [0 0 1 4]
Primitives for low-res model, < 4 pixels
E.g., Procedural “DelayedReadArchive” [“small.rib”] [-1 -1 -1 1 1 1 ]
**DetailRange**[1 4 256 400]
Primitives for medium-res model
**DetailRange**[256 400 1e38 1e38]
Primitives for high-res model
**AttributeEnd**

## 16. How can one render shadows in classic REYES RM? Which shaders have to be modified for this to work?

    You have to generate a shadow map (converted from a depth image) from the viewpoint of the light. Then you have to use this shadow map in a light shader (like a predefined shadowspot, shadowpoint). Lastly, you have to make sure that the surface shader uses light information. The function shadow(map, pointToTest) can be used in the light shader and returns 1.0 in a shadow and 0.0 if not in a shadow, according to the shadow map.

## 17. Bucket rendering in classic REYES RM - what is it, and what are its implications for using shaders? Which types of shaders are affected?

    A common memory optimization, it is a step called bucketing prior to the dicing step. Which means before shading.
    The output image is divided into a coarse grid of "buckets," each typically 16 by 16 pixels in size. The objects are then split roughly along the bucket boundaries and placed into buckets based on their location. Each bucket is diced and drawn individually, and the data from the previous bucket is discarded before the next bucket is processed.
    This leads to less memory consumption because only the 16x16 frame buffer is stored.
    Displacement shaders are affected - geometry is first diced, only after that it’s shaded (and displaced). This means that some micropolygons can get very deformed.

## 18. Describe the interaction of surface and light shaders in classic REYES RenderMan.

    Light shaders may use Illuminate (from direction) or Solar (from point) statements, or neither in case of ambient light, for example. These functions restrict the light to emitting photons only in the direction (either assuming point emission or not).
    !! In REYES, light shaders are only executed when a surface shader actually calls them, a.k.a. light shaders are basically “sub-programs” of surface shaders.
    Basic one way communication for a surface shader is the statement Illuminance which iterates all light sources and provides information about them in the loop (direction to the light L, Cl as incident light from that source). This call can be augmented by specifying the category of the lights that are to be iterated through (“uvlight” as per 12/18 in 03_SL_and_Lightning).
    More complicated: function lightsource(string paramname, type outputParam) that retrieves the value of paramname parameter from the light shader. This allows creating “magic lights” such as decals and bullet holes in existing surfaces, because displacements can also be done in surface shaders (!!!).

## 19. What are magic lights? How do they work, and what are they being used for?

    See last paragraph of question 15.
    A way to utilize the fact that many light shaders can be active at the same time. They can serve as a modular sub-programs to surface shaders (basically like function calls) that get evaluated and return their values when the surface shader needs them. Influence of the magic lights can be summed using the Illuminate() construct.
    Another use is a hacky way to add some features to a frozen asset that should not be changed - add a magic light and create bullet holes in the already frozen rusted metal shader.

## 20. What is Ambient Occlusion: how does one compute it, and what is it good for?

    AO computes the ratio of occluded and unoccluded space for each point of an object in the hemisphere above the point. The more occluded it is (inside a pipe, in a corner, ...), the darker it should be. We can render the computed AO in a grayscale image or use the AO information in the surface shaders.
    AO is a global method = the illumination at each point is a function of other geometry in the scene. It is usually implemented as a bunch of rays cast in a few sampled directions (e.g., manually using mi_trace_probe in a Mental Ray surface shader). We usually limit the maximum distance of the rays, because we do not care about objects that are too far.

## 21. Anti-aliasing of shaders: why is this such an issue with original RenderMan, and not so much with other shader language systems?

    Reason: Procedural textures are point-sampled for each micro-polygon and whilst 2D textures can be pre-filtered, procedurals can’t.
    Basic solution is to shoot more samples per micropolygon or make a denser micropolygon grid. Both of these solutions are expensive and do NOT solve the problem, only make it smaller, potentially undetectable for human eyes.
    Not an issue with other shader language systems because those don’t use the micropolygon algorithm. Antialiasing is basically built-in for ray tracers, because we sample multiple times for a single pixel, from multiple slightly different positions / sub-pixels.

## 22. How can one anti-alias a classic REYES RenderMan shader (three basic options)?

- Denser sampling (see no. 19)
- Convert into 2D texture, and pre-filter that. You are getting rid of the scalability advantage and need to know beforehand what resolution the texture should be.
- Analytically pre-filter the procedural shader function itself. Which can be mathematically challenging or impossible.

The analytical variant is done by averaging the texture function around the sample point using a convolution with a kernel filter, usually a box filter (gaussian filter is another option). We can use functions such as Du(x), Dv(x), area(p) to determine the sample size / kernel size and #define additional helper functions such as filterwidth(x) or filterwidth_point(p). Where Du(x) is the derivative of x w.r.t u, sometimes called ray differential (in ray tracing renderers).

## 23. Mental Ray: describe the overall architecture of the system. What main types of

    shaders are there in this system? What are phenomena?

Shaders: camera, light, material (displace, shadow, volume, material), … + all green below

Shader types: Anonymous shaders (called directly), named shaders (defined for later reference), shader lists (accumulation of shader results, i.e., when we write multiple shaders below each other, they are implicitly chained together and executed in the order, chaining the outputs to the inputs of the next ones), shader graphs and phenomena.

A named shader graph is a phenomenon - see slide, front_depth is a combination of front_bright and depth_fade. The root shader is the last one that returns the value.
Phenomena always define an interface (i.e., arguments from the scene file) and a root shader (i.e., the main shader that gets executed and returns the result). The root shader / other shaders may use outputs of other shaders as their inputs.

## 24. What is the basic structure of a Mental Ray scene file? What do the shaders look like, how are they referenced from within an .mi file, and which language are they written in?

Scene represented by a database of entities
Objects, lights, cameras are elements
Elements to be rendered are instanced
Rendering procedures are shaders
Algorithmic parameters are specified as options
instance + shaders + options = render
the .mi file is a text based file based on a grammar
specification = options
shading definitions = shaders, textures, materials, data
rendering entities = camera, objects, lights
instances = instanced entities, groups of instances

Structure example: 1) set options, 2) define camera, 3) instance camera, 4) define material + object, 5) instance object, 6) define root group, 7) render the root group by the camera instance.

The shaders are written in C and Mental Ray uses the compiled object files. Shaders are C functions that return a boolean and have a pointer to the result (I/O), to the state, and to the parameters from the scene file. They are referenced to from .mi using a declaration (contains shader name, result type, and parameters) and then we call them by their declared name.

## 25. How would one render a path traced image with Mental Ray?

Each shader in the scene has to be able to compute its direct illumination from iterating and sampling through all light sources, and its indirect lightning by tracing additional rays according to its BRDF. Trace depth parameter can be passed to all the shaders to cut off too deep recursions. Reflections, refraction shaders are required.

## 26. How does one obtain edge traced images in Mental Ray? What sort of shaders are

involved in this task?
Store shader – What data will be necessary when deciding if a contour should be drawn? (e.g., store a tag of the object and a normal vector)
Contrast shader – Has enough data been collected to draw a contour? (e.g., if different tags OR if the dot product of the normals is above a threshold, return true = there is a contour)
Contour shader – How should the contour be drawn (i.e. what colour and how thick should the line be)?
Output shader – How should the contours be used in creating the output image? (goes through the list of contours and creates a vector PostScript file)

## 27. What are shader graphs? How can one automatically obtain shader graphs from surface captures? (this is referring to the 2006 paper by Jason Lawrence et al.)

Inverse shading trees.pdf in SIS
From the appearance acquisition process we receive a 6D SVBRDF (2D u,v coordinates, 2D eye position, 2D light position; eye and light are 2D, because they are normalized 3D vectors). This is a very complex and data-driven data structure with not many intuitive and modifiable parameters - gigabytes of data.
The goal of shading trees is representing a SVBRDF in a tree, computed from down to top (1D curves =multiply=> lobes =sum=> 4D BRDF =multiply_by_2D_weights=> 6D SVBRDF), i.e., representing the high-dimensional function as a tree shaped collection of lower dimensional functions.
The goal of inverse shading trees is to convert the complex data structure from the appearance acquisition into the shading tree.. This is done by decomposing the huge matrix into smaller ones towards the bottom of the tree.
The decomposition proposed in the paper:
Tabulates the raw acquired data in a matrix (u,v)x(eyeX,eyeY,lightX,lightY).
Factorizes the matrix as a product of two matrices (2D blending weights × 4D BRDF). This is done by ACLS (alternating constrained least squares), which works the best w.r.t. the constraints (e.g., energy conservation, sparsity, …).

The BRDFs can also be decomposed further for example into diffuse and specular parts.

## 28. What are the capabilities of polarimetric appearance capture techniques? Which materials are these being used for, and what properties can they yield, in addition to simple texture information?

Light polarization, semi-transparent objects, solve anisotropy

## 29. Describe the Disney BRDF: what is the design philosophy behind it, and what sort of parameters does it offer?

The Disney BRDF (Bidirectional Reflectance Distribution Function) is a shading model developed by Disney Animation Studios for use in computer graphics and rendering. It is designed to provide a flexible and physically plausible approach to simulate the appearance of a wide range of materials under various lighting conditions.

The design philosophy behind the Disney BRDF is to create a shader that is artist-friendly, intuitive, and capable of accurately reproducing the complex behavior of real-world materials. The goal is to give artists control over the appearance of materials without requiring them to have an in-depth understanding of the underlying physics.

The Disney BRDF offers a set of parameters that allow artists to control the material's visual properties. Some of the key parameters include:

Base Color
Metallic
Roughness
Specular
Subsurface

## 30. What can you say about hair rendering? How is this modeled, and what sort of shaders are applied to it?

Hair is a line. Algebraically we can get the closest distance of two lines, if it is less than the diameter of a hair, we hit it.
Also, particle systems can be done with hair primitive.

## 31. What is the purpose of the AnySL approach by Karrenberg et al? What are its limitations?

Shader compatibility - using an already existing shader in a different rendering engine with different shader API without rewriting it to the new shader language.
Upon creating a new rendering engine, you only need to implement the AnySL API bindings to be able to use any existing shaders for other AnySL capable rendering engines.
Limitations and issues: There is no unified shading API for different renderers, so each rendering engine has to implement the AnySL API. It also supports only surface shaders. It is a bit slower than hand tuned SIMD shaders exactly for the HW used.

## 32. Give an overview of the main differences between shading on the GPU, in RenderMan, and in a ray-based rendering system: discuss performance, scalability, and anti-aliasing issues.

GPU shading uses vertex and fragment shaders, it first uses the vertex shader, then tessellates the geometry, rasterizes it and processes it with fragment shaders (exactly 1 sample per pixel, if not using full-screen anti aliasing). Fragment shaders are responsible for surfaces, light sources, post-processing effects, etc. Shadows and reflections have to be “hacked” using shadow maps, environment maps, etc.
RenderMan20 shading works by dicing the objects into micropolygons and shading the flat micropolygons separately. There are typically more micropolygons per pixel. Different shader types for surfaces, lights, … Shadows and reflections also have to be “hacked” as in GPUs.
Ray Tracing rendering systems cast rays in a physically plausible model and integrate the light on every single point in the scene by adding up all incoming light from all directions (direct and bounced). Shadows, reflections, refractions, etc. are solved by ray casting.

RT is very slow and doesn’t converge very fast when compared to the other two, however it is way more realistic.
RenderMan and GPU shading suffer from the same anti-aliasing issues with procedural textures - sampling a procedural texture is subject to Nyquist law. In ray tracing, this issue is implicitly solved by casting multiple rays for a single pixel.
Both GPU and old RenderMan scale very badly with an increasing amount of light sources / shadows, as we have to iterate through lights and sample and generate multiple shadow maps for each frame. In ray tracing, shadows are easily obtained by tracing the rays.
RTs are very scalable utilizing rendering farms where each node can shoot as many rays as it can completing passes that get put together at the end, as well as cut the image into pieces and compute separate chunks. To make use of SIMD, we can use ray packets.
Is GPU shading is scalable? I would say yes, as every pixel is computed in parallel.
RenderMan shading is scalable too because of bucketing method, that was firstly done for memory issues, but also allows some degree of parallelism.

## 33. What is the basic approach of Renderman RIS with regard to shading? What are the differences to classic REYES RM?

It is a ray tracer, hence the pipeline is completely different. RIS doesn’t allow shaders to cast rays, only to use predefined materials that the renderer provides (like PxrSurface). The part of the renderer that does all the raycasting is called an Integrator, several exist (RayTracer, VCM, ..) and can be exchanged without modifying any other part of the scene or the shaders.

https://renderman.pixar.com/resources/RenderMan_20/risOverview.html

## 34. What are the performance tweaks (as in: features one can enable and disable, to obtain render quality vs. speed trade-offs) one can use when working with the RenderMan RIS path tracer?

maxPathLength = absolute upper bound on the maximum ray depth
num<whatever>Samples = how much <whatever> is sampled (diffuse, specular, lights, bxdfs..)
RR-MC parameters = roulette start, throughput threshold
Unidirectional vs VCM (choice between PxrPathTracer and PxrVCM)

Also by controlling the Light part of the settings, you can decide if you want to render Thin Shadows or if you want to trace light paths. You can also choose if the renderer should generate photons which allows for rendering caustics.

## 35. What is Open Shading Language, and how does its functionality compare to classic REYES RM, Mental Ray, and Renderman RIS?

OSL is a shading language specification and its implementation. It is made for ray-tracing renderers and as such does not allow casting rays explicitly: “There are no ”light loops” or explicitly traced rays in OSL surface shaders.” It specifies a plethora of Bxdfs that the renderers should implement and then allows to call these internal functions - closures. It allows for runtime optimization by deflating the node graph and reducing redundant branches, and dead code during runtime.

Main difference compared to REYES and MentalRay is the absence of raycasting functions.
Main difference to RIS is the fact that OSL provides a lot of Bxdfs, whereas RIS reduced all surface materials into PxrSurface that is not based on a physical appearance of a material.
RIS also allows the use of OSL shaders.

From Blender: OSL is different from, for example, RSL or GLSL, in that it does not have a light loop. There is no access to lights in the scene, and the material must be built from closures that are implemented in the renderer itself. This is more limited, but also makes it possible for the renderer to do optimizations and ensure all shaders can be importance sampled.

Tl;dr:
OSL: No light loops and no explicit ray casting. Uses closures. Light sampling and ray casting are moved from shaders to the render implementation.
REYES: Explicit light loops in shaders, but ray casting not possible.
Mental Ray: Explicit light loops and ray casting / sampling directly from shaders.

## 36. What is nVidia MDL, and how does its functionality compare to classic REYES RM, Mental Ray, OSL, and Renderman RIS?

NVIDIA Material Definition Language (MDL) is a framework and language for describing and exchanging materials between different rendering systems. It allows for the creation of physically-based material definitions that can be used across various rendering engines and software. Standardized format.

## 37. What are Allegorithmic Substance Designer and Substance Painter? Which steps in a lookdev workflow do these applications cover? What are their respective strengths?

SD is a tool for creating procedural materials and textures that can then be exported to every mainstream 3D CG editor (3ds max, Unity, etc). It works by combining simple shaders in a Node graph in the GUI of the application, with live 3D preview, much like OSL.

SP on the other hand allows for application of the materials created prior onto a 3D geometry by painting the material. Literally painting with brushes on a 3D model, with live WYSIWYG preview.

They cover the appearance modeling (SD) and texturing models (SP).
Their main strength lies in the materials being exportable to all main stream 3D designer applications, renderers and engines (Arnold, Corona, Vray, 3ds Max, Maya, Unity, Unreal, ...)
