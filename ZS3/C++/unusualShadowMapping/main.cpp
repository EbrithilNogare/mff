#include <cstdio>
#include <vector>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/transform.hpp>

#include "Camera.h"
#include "Scene.h"
#include "Math.h"


// ----------------------------------------------------------------------------
// Instanced geometry vertex shader source
// ----------------------------------------------------------------------------
static const char* vsSource[] = { R"(
#version 460 core

// Uniform blocks, i.e., constants
layout (location = 0) uniform mat4 worldToView;
layout (location = 1) uniform mat4 projection;

// Vertex attribute block, i.e., input
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec3 tangent;
layout (location = 3) in vec2 texCoord;

// Structure holding per instance data
struct InstanceData
{
  // Model to world transformation
  mat4 model;
  // Texture repeat factor
  vec4 texRepeat;
};

// Storage buffer used for instances using interface block syntax
layout (binding = 0) buffer InstanceBuffer
{
  // only one variable length array allowed
  InstanceData instanceData[];
};

// Vertex output
out vec2 vTexCoord;
out vec3 vTangent;
out vec3 vBitangent;
out vec3 vNormal;

void main()
{
  // Construct the normal transformation matrix
  mat3 normalTransform = mat3(transpose(inverse(instanceData[gl_InstanceID].model)));

  // Create the tangent space matrix and pass it to the fragment shader
  vNormal = normalize(normalTransform * normal);
  vTangent = normalize(normalTransform * tangent);
  vBitangent = normalize(cross(vTangent, vNormal));

  // Transform vertex position
  gl_Position = projection * worldToView * instanceData[gl_InstanceID].model * vec4(position.xyz, 1.0f);

  // Pass texture coordinates to the fragment shader
  vTexCoord = texCoord.st * instanceData[gl_InstanceID].texRepeat.xx;
}
)",
// ----------------------------------------------------------------------------
// Basic vertex shader
// ----------------------------------------------------------------------------
R"(
#version 460 core

// Uniform blocks, i.e., constants
layout (location = 0) uniform mat4 worldToView;
layout (location = 1) uniform mat4 projection;
layout (location = 2) uniform vec3 position;


void main()
{
  gl_Position = projection * worldToView * vec4(position.xyz, 1.0f);
}
)",
"" };

// ----------------------------------------------------------------------------
// Blinn-Phong shading fragment shader source
// ----------------------------------------------------------------------------
static const char* fsSource[] = { R"(
#version 460 core

// Texture samplers
layout (binding = 0) uniform sampler2D Diffuse;
layout (binding = 1) uniform sampler2D Normal;
layout (binding = 2) uniform sampler2D Specular;
layout (binding = 3) uniform sampler2D Occlusion;

// Light position/direction
uniform vec3 light;

in vec2 vTexCoord;
in vec3 vTangent;
in vec3 vBitangent;
in vec3 vNormal;
out vec4 color;

void main()
{
  vec3 diffuse = texture(Diffuse, vTexCoord.st).rgb;
  vec3 noSample = texture(Normal, vTexCoord.st).rgb;
  float specular = texture(Specular, vTexCoord.st).r;
  float occlusion = texture(Occlusion, vTexCoord.st).r;

  color = vec4(diffuse, 1.0f);
}
)",
// ----------------------------------------------------------------------------
// Basic fragment shader source
// ----------------------------------------------------------------------------
R"(
#version 460 core

out vec4 color;

void main()
{
  color = vec4(1.0f, 1.0f, 1.0f, 1.0f);
}
)",
"" };

// ----------------------------------------------------------------------------

// Enum class for storing default window parameters
enum class WindowParams : int
{
	Width = 800,
	Height = 600
};

// ----------------------------------------------------------------------------

// Near clip plane settings
float nearClipPlane = 0.01f;
// Far clip plane settings
float farClipPlane = 100.0f;

// Mouse movement
struct MouseStatus
{
	// Current position
	double x, y;
	// Previsous position
	double prevX, prevY;
} mouseStatus = { 0.0 };

// ----------------------------------------------------------------------------

namespace ShaderProgram
{
	enum
	{
		Geometry, LightPosition, NumShaderPrograms
	};
}

// Max buffer length
static const unsigned int MAX_BUFFER_LENGTH = 512;
// Main window handle
GLFWwindow* mainWindow = nullptr;
// Shader program handles
GLuint shaderProgram[ShaderProgram::NumShaderPrograms];
// Camera instance
Camera camera;
// Scene instance
Scene scene;
// Vsync on?
bool vsync = true;
// Anisotropic filtering on?
bool anisotropic = true;

// ----------------------------------------------------------------------------

// Number of cubes in the scene
const int numCubes = 1;
// Instance data for single objects in the scene
InstanceData single;
// Instance data for cubes
std::vector<InstanceData> instanceData(numCubes);
// Cube position
std::vector<glm::vec3> cubePositions;

// ----------------------------------------------------------------------------

// Callback for handling GLFW errors
void errorCallback(int error, const char* description)
{
	printf("GLFW Error %i: %s\n", error, description);
}

// Callback for handling window resize events
void resizeCallback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
	camera.SetProjection(45.0f, (float)width / (float)height, nearClipPlane, farClipPlane);
}

// Callback for handling mouse movement over the window
void mouseMoveCallback(GLFWwindow* window, double x, double y)
{
	mouseStatus.prevX = mouseStatus.x;
	mouseStatus.prevY = mouseStatus.y;
	mouseStatus.x = x;
	mouseStatus.y = y;
}

// Keyboard callback for handling system switches
void keyCallback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
	// Notify the window that user wants to exit the application
	if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);

	// Enable/disable MSAA - note that it still uses the MSAA buffer
	if (key == GLFW_KEY_F1 && action == GLFW_PRESS)
	{
		if (glIsEnabled(GL_MULTISAMPLE))
			glDisable(GL_MULTISAMPLE);
		else
			glEnable(GL_MULTISAMPLE);
	}

	// Enable/disable wireframe rendering
	if (key == GLFW_KEY_F2 && action == GLFW_PRESS)
	{
		GLint polygonMode;
		glGetIntegerv(GL_POLYGON_MODE, &polygonMode);
		if (polygonMode == GL_FILL)
			glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
		else
			glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
	}

	// Enable/disable backface culling
	if (key == GLFW_KEY_F3 && action == GLFW_PRESS)
	{
		if (glIsEnabled(GL_CULL_FACE))
			glDisable(GL_CULL_FACE);
		else
			glEnable(GL_CULL_FACE);
	}

	// Enable/disable vsync
	if (key == GLFW_KEY_F4 && action == GLFW_PRESS)
	{
		vsync = !vsync;
		if (vsync)
			glfwSwapInterval(1);
		else
			glfwSwapInterval(0);
	}

	// Enable/disable vsync
	if (key == GLFW_KEY_F5 && action == GLFW_PRESS)
	{
		anisotropic = !anisotropic;
		if (anisotropic)
			glSamplerParameterf(scene.GetSampler(Scene::Anisotropic), GL_TEXTURE_MAX_ANISOTROPY, 16.0f);
		else
			glSamplerParameterf(scene.GetSampler(Scene::Anisotropic), GL_TEXTURE_MAX_ANISOTROPY, 1.0f);
	}
}

// Helper function for creating and compiling the shaders
bool compileShaders()
{
	GLuint vertexShader, fragmentShader;
	GLint result;
	GLchar log[MAX_BUFFER_LENGTH];

	auto compileAndLinkShader = [&](int index)
	{
		// Create and compile the vertex shader
		vertexShader = glCreateShader(GL_VERTEX_SHADER);
		glShaderSource(vertexShader, 1, vsSource + index, nullptr);
		glCompileShader(vertexShader);

		// Check that compilation was a success
		glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &result);
		if (result == GL_FALSE)
		{
			glGetShaderInfoLog(vertexShader, MAX_BUFFER_LENGTH, nullptr, log);
			printf("Vertex shader compilation failed: %s\n", log);
			return false;
		}

		// Create and compile the fragment shader
		fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
		glShaderSource(fragmentShader, 1, fsSource + index, nullptr);
		glCompileShader(fragmentShader);

		// Check that compilation was a success
		glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &result);
		if (result == GL_FALSE)
		{
			glGetShaderInfoLog(fragmentShader, MAX_BUFFER_LENGTH, nullptr, log);
			printf("Fragment shader compilation failed: %s\n", log);
			glDeleteShader(vertexShader);
			return false;
		}

		// Create the shader program, attach shaders, link
		shaderProgram[index] = glCreateProgram();
		glAttachShader(shaderProgram[index], vertexShader);
		glAttachShader(shaderProgram[index], fragmentShader);
		glLinkProgram(shaderProgram[index]);

		// Check that linkage was a success
		glGetProgramiv(shaderProgram[index], GL_LINK_STATUS, &result);
		if (result == GL_FALSE)
		{
			glGetProgramInfoLog(shaderProgram[index], MAX_BUFFER_LENGTH, nullptr, log);
			printf("Shader program linking failed: %s\n", log);
			glDeleteShader(vertexShader);
			glDeleteShader(fragmentShader);
			return false;
		}

		// Clean up resources we don't need anymore at this point
		glDeleteShader(vertexShader);
		glDeleteShader(fragmentShader);
	};

	if (!compileAndLinkShader(ShaderProgram::Geometry))
		return false;

	if (!compileAndLinkShader(ShaderProgram::LightPosition))
		return false;

	return true;
}

// Helper method for OpenGL initialization
bool initOpenGL()
{
	// Set the GLFW error callback
	glfwSetErrorCallback(errorCallback);

	// Initialize the GLFW library
	if (!glfwInit()) return false;

	// Request OpenGL 4.6 core profile upon window creation
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 6);
	glfwWindowHint(GLFW_SAMPLES, 4);
	glfwWindowHint(GLFW_DEPTH_BITS, 24);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	// Create the window
	mainWindow = glfwCreateWindow((int)WindowParams::Width, (int)WindowParams::Height, "", nullptr, nullptr);
	if (mainWindow == nullptr)
	{
		printf("Failed to create the GLFW window!");
		return false;
	}

	// Make the created window with OpenGL context current for this thread
	glfwMakeContextCurrent(mainWindow);

	// Check that GLAD .dll loader and symbol imported is ready
	if (!gladLoadGL()) {
		printf("GLAD failed!\n");
		return false;
	}

	// Enable vsync
	if (vsync)
		glfwSwapInterval(1);
	else
		glfwSwapInterval(0);

	// Enable backface culling
	glEnable(GL_CULL_FACE);
	glCullFace(GL_BACK);

	// Enable depth test
	glEnable(GL_DEPTH_TEST);
	glDepthFunc(GL_LEQUAL);

	// Register a window resize callback
	glfwSetFramebufferSizeCallback(mainWindow, resizeCallback);

	// Register keyboard callback
	glfwSetKeyCallback(mainWindow, keyCallback);

	// Register mouse movement callback
	glfwSetCursorPosCallback(mainWindow, mouseMoveCallback);

	// Set the OpenGL viewport and camera projection
	resizeCallback(mainWindow, (int)WindowParams::Width, (int)WindowParams::Height);

	// Set the initial camera position and orientation
	camera.SetTransformation(glm::vec3(0.0f, 1.0f, -4.0f), glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f));

	return true;
}

// Helper method for graceful shutdown
void shutDown()
{
	scene.ReleaseResources();

	// Release the shader programs
	glDeleteProgram(shaderProgram[ShaderProgram::Geometry]);
	glDeleteProgram(shaderProgram[ShaderProgram::LightPosition]);

	// Relase the window
	glfwDestroyWindow(mainWindow);

	// Close the GLFW library
	glfwTerminate();
}

void createGeometry()
{
	scene.CreateGeometry();
	scene.LoadTextures();

	// Position the first cube half a meter above origin
	cubePositions.reserve(numCubes);
	cubePositions.push_back(glm::vec3(0.0f, 0.5f, 0.0f));

	// Generate random positions for the rest of the cubes
	for (int i = 1; i < numCubes; ++i)
	{
		float x = getRandom(-5.0f, 5.0f);
		float y = getRandom(1.0f, 5.0f);
		float z = getRandom(-5.0f, 5.0f);

		cubePositions.push_back(glm::vec3(x, y, z));
	}
}

// Helper method for handling input events
void processInput(float dt)
{
	// Camera movement
	int direction = (int)MovementDirections::None;
	if (glfwGetKey(mainWindow, GLFW_KEY_W) == GLFW_PRESS)
		direction |= (int)MovementDirections::Forward;

	if (glfwGetKey(mainWindow, GLFW_KEY_S) == GLFW_PRESS)
		direction |= (int)MovementDirections::Backward;

	if (glfwGetKey(mainWindow, GLFW_KEY_A) == GLFW_PRESS)
		direction |= (int)MovementDirections::Left;

	if (glfwGetKey(mainWindow, GLFW_KEY_D) == GLFW_PRESS)
		direction |= (int)MovementDirections::Right;

	if (glfwGetKey(mainWindow, GLFW_KEY_R) == GLFW_PRESS)
		direction |= (int)MovementDirections::Up;

	if (glfwGetKey(mainWindow, GLFW_KEY_F) == GLFW_PRESS)
		direction |= (int)MovementDirections::Down;

	glm::vec2 mouseMove(0.0f, 0.0f);
	if (glfwGetMouseButton(mainWindow, GLFW_MOUSE_BUTTON_RIGHT) == GLFW_PRESS)
	{
		mouseMove.x = (float)(mouseStatus.x - mouseStatus.prevX);
		mouseMove.y = (float)(mouseStatus.y - mouseStatus.prevY);
	}

	camera.Move((MovementDirections)direction, mouseMove, dt);

	if (glfwGetKey(mainWindow, GLFW_KEY_SPACE) == GLFW_PRESS)
		camera.SetTransformation(glm::vec3(0.0f, 1.0f, -4.0f), glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f));
}

void bindTextures(const GLuint& diffuse, const GLuint& normal, const GLuint& specular, const GLuint& occlusion)
{
	// We want to bind textures and appropriate samplers
	glActiveTexture(GL_TEXTURE0 + 0);
	glBindTexture(GL_TEXTURE_2D, diffuse);
	glBindSampler(0, scene.GetSampler(Scene::Anisotropic));

	glActiveTexture(GL_TEXTURE0 + 1);
	glBindTexture(GL_TEXTURE_2D, normal);
	glBindSampler(1, scene.GetSampler(Scene::Anisotropic));

	glActiveTexture(GL_TEXTURE0 + 2);
	glBindTexture(GL_TEXTURE_2D, specular);
	glBindSampler(2, scene.GetSampler(Scene::Anisotropic));

	glActiveTexture(GL_TEXTURE0 + 3);
	glBindTexture(GL_TEXTURE_2D, occlusion);
	glBindSampler(3, scene.GetSampler(Scene::Anisotropic));
}

void updateInstanceData(void* data, GLsizei size)
{
	// Update the buffer data using map/unmap buffer
	void* ptr = glMapBuffer(GL_SHADER_STORAGE_BUFFER, GL_WRITE_ONLY);
	memcpy(ptr, data, size);
	glUnmapBuffer(GL_SHADER_STORAGE_BUFFER);
}

void renderLightObject(const glm::vec3& lightPosition)
{
	glUseProgram(shaderProgram[ShaderProgram::LightPosition]);

	// Update the transformation & projection matrices
	glUniformMatrix4fv(0, 1, GL_FALSE, glm::value_ptr(camera.GetTransformation()));
	glUniformMatrix4fv(1, 1, GL_FALSE, glm::value_ptr(camera.GetProjection()));

	// Update the light position
	glUniform3f(2, lightPosition.x, lightPosition.y, lightPosition.z);

	// Draw the light object
	glPointSize(10.0f);
	glDrawArrays(GL_POINTS, 0, 1);

	glUseProgram(0);
}

void renderScene()
{
	// Clear the color buffer
	glClearColor(0.1f, 0.2f, 0.4f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	// Tell OpenGL we'd like to use the previously compiled shader program
	glUseProgram(shaderProgram[ShaderProgram::Geometry]);

	// Update the transformation & projection matrices
	glUniformMatrix4fv(0, 1, GL_FALSE, glm::value_ptr(camera.GetTransformation()));
	glUniformMatrix4fv(1, 1, GL_FALSE, glm::value_ptr(camera.GetProjection()));

	// Update the light position
	static glm::vec3 light(-3.0f, 3.0f, 0.0f);
	GLint lightLoc = glGetUniformLocation(shaderProgram[ShaderProgram::Geometry], "light");
	glUniform3f(lightLoc, light.x, light.y, light.z);

	// -------------------------------------------------------------------------

	// Update instance data - in this example they are static but instances could move, rotate, etc.

	// Scene floor
	single.model = glm::scale(glm::vec3(30.0f, 0.0f, 30.0f));
	single.model *= glm::rotate(PI_HALF, glm::vec3(1.0f, 0.0f, 0.0f));
	single.texRepeat = glm::vec4(30.0f, 0.0f, 0.0f, 0.0f);

	float angle = 20.0f;
	for (int i = 0; i < numCubes; ++i)
	{
		// Create unit matrix
		glm::mat4x4 model(glm::vec4(1.0f, 0.0f, 0.0f, 0.0f), glm::vec4(0.0f, 1.0f, 0.0f, 0.0f), glm::vec4(0.0f, 0.0f, 1.0f, 0.0f), glm::vec4(0.0f, 0.0f, 0.0f, 1.0f));
		model *= glm::translate(cubePositions[i]);
		model *= glm::rotate(glm::radians(i * angle), glm::vec3(1.0f, 1.0f, 1.0f));
		instanceData[i].model = model;
		instanceData[i].texRepeat = glm::vec4(1.0f, 0.0f, 0.0f, 0.0f);
	}

	// -------------------------------------------------------------------------

	// Draw scene floor
	bindTextures(scene.GetTexture(Scene::CheckerBoard), scene.GetTexture(Scene::Blue), scene.GetTexture(Scene::Grey), scene.GetTexture(Scene::White));
	updateInstanceData(&single, sizeof(InstanceData));
	glBindVertexArray(scene.GetVAO(Scene::Quad));
	glDrawElementsInstanced(GL_TRIANGLES, scene.GetNumIndices(Scene::Quad), GL_UNSIGNED_INT, reinterpret_cast<void*>(0), 1);

	// Draw cubes
	bindTextures(scene.GetTexture(Scene::Diffuse), scene.GetTexture(Scene::Normal), scene.GetTexture(Scene::Specular), scene.GetTexture(Scene::Occlusion));
	updateInstanceData(&*instanceData.begin(), (GLsizei)instanceData.size() * sizeof(InstanceData));
	glBindVertexArray(scene.GetVAO(Scene::Cube));
	glDrawElementsInstanced(GL_TRIANGLES, scene.GetNumIndices(Scene::Cube), GL_UNSIGNED_INT, reinterpret_cast<void*>(0), numCubes);

	// Draw the light object (cheating a little bit by using previously used VAO)
	renderLightObject(light);

	// -------------------------------------------------------------------------

	// Unbind the shader program and other resources
	glBindVertexArray(0);
	glUseProgram(0);
}

// Helper method for implementing the application main loop
void mainLoop()
{
	static double prevTime = 0.0;
	while (!glfwWindowShouldClose(mainWindow))
	{
		// Calculate delta time
		double time = glfwGetTime();
		float dt = (float)(time - prevTime);
		prevTime = time;

		static char title[MAX_BUFFER_LENGTH];
		snprintf(title, MAX_BUFFER_LENGTH, "dt = %.2fms, FPS = %.1f", dt * 1000.0f, 1.0f / dt);
		glfwSetWindowTitle(mainWindow, title);

		// Poll the events like keyboard, mouse, etc.
		glfwPollEvents();

		// Process keyboard input
		processInput(dt);

		// Render the scene
		renderScene();

		// Swap actual buffers on the GPU
		glfwSwapBuffers(mainWindow);
	}
}

int main()
{
	// Initialize the OpenGL context and create a window
	if (!initOpenGL())
	{
		printf("Failed to initialize OpenGL!\n");
		shutDown();
		return -1;
	}

	// Compile shaders needed to run
	if (!compileShaders())
	{
		printf("Failed to compile shaders!\n");
		shutDown();
		return -1;
	}

	// Create the scene geometry and load textures
	createGeometry();

	// Enter the application main loop
	mainLoop();

	// Release used resources and exit
	shutDown();
	return 0;
}
