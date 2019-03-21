#include <cstdio>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/gtc/type_ptr.hpp>

#include "Camera.h"
#include "Scene.h"


// ----------------------------------------------------------------------------

// Vertex shader source
static const char* vsSource[] = { R"(
#version 440 core

// Uniform blocks, i.e., constants
layout (location = 0) uniform mat4 worldToView;
layout (location = 1) uniform mat4 projection;

// Vertex attribute block, i.e., input
layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoord;

// Vertex output
out vec2 vTexCoord;

void main()
{
  vTexCoord = texCoord.st;
  gl_Position = projection * worldToView * vec4(position.xyz, 1.0f);
}
)", "" };

// Fragment shader source
static const char* fsSource[] = { R"(
#version 440 core

in vec2 vTexCoord;
out vec4 color;

void main()
{
  color = vec4(vTexCoord.st, 0.0f, 1.0f);
}
)", "" };

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
} mouseStatus = {0.0};

// ----------------------------------------------------------------------------

// Max buffer length
static const unsigned int MAX_BUFFER_LENGTH = 256;
// Main window handle
GLFWwindow* mainWindow = nullptr;
// Shader program handle
GLuint shaderProgram = 0;
// Vertex Array Object handle - it can contain multiple vertex buffers
GLuint vao = 0;
// Vertex buffer object
GLuint vbo = 0;
// Element array buffer, i.e., index buffer
GLuint ibo = 0;
// Camera instance
Camera camera;
// Scene instance
Scene scene;
// Vsync on?
bool vsync = true;

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
}

// Helper function for creating and compiling the shaders
bool compileShaders()
{
  GLuint vertexShader, fragmentShader;
  GLint result;
  GLchar log[MAX_BUFFER_LENGTH];

  // Create and compile the vertex shader
  vertexShader = glCreateShader(GL_VERTEX_SHADER);
  glShaderSource(vertexShader, 1, vsSource, nullptr);
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
  glShaderSource(fragmentShader, 1, fsSource, nullptr);
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
  shaderProgram = glCreateProgram();
  glAttachShader(shaderProgram, vertexShader);
  glAttachShader(shaderProgram, fragmentShader);
  glLinkProgram(shaderProgram);

  // Check that linkage was a success
  glGetProgramiv(shaderProgram, GL_LINK_STATUS, &result);
  if (result == GL_FALSE)
  {
    glGetProgramInfoLog(shaderProgram, MAX_BUFFER_LENGTH, nullptr, log);
    printf("Shader program linking failed: %s\n", log);
    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);
    return false;
  }

  // Clean up resources we don't need anymore at this point
  glDeleteShader(vertexShader);
  glDeleteShader(fragmentShader);

  return true;
}

// Helper method for creating scene geometry
void createGeometry()
{
  // Create and bind the VAO
  glGenVertexArrays(1, &vao);
  glBindVertexArray(vao);

  std::vector<Vertex> vb;
  std::vector<GLuint> ib;
  scene.GetCube(vb, ib);

  // Generate the VBO for vertex storage and fill it with data
  glGenBuffers(1, &vbo);
  glBindBuffer(GL_ARRAY_BUFFER, vbo);
  glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vb.size(), static_cast<void*>(&*vb.begin()), GL_STATIC_DRAW);
  glBindBuffer(GL_ARRAY_BUFFER, 0);

  // Generate the index buffer and fill it with data
  glGenBuffers(1, &ibo);
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ibo);
  glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLuint) * ib.size(), static_cast<void*>(&*ib.begin()), GL_STATIC_DRAW);
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

  // Unbind the VAO, we'll bind it prior rendering
  glBindVertexArray(0);
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
  glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 4);
  glfwWindowHint(GLFW_SAMPLES, 4);
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
  camera.SetTransformation(glm::vec3(0.0f, 0.0f, -5.0f), glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f));

  return true;
}

// Helper method for graceful shutdown
void shutDown()
{
  // Release the vertex array object
  glDeleteVertexArrays(1, &vao);

  // Release the vertex buffer
  glDeleteBuffers(1, &vbo);

  // Release the index buffer
  glDeleteBuffers(1, &ibo);

  // Release the shader program
  glDeleteProgram(shaderProgram);

  // Relase the window
  glfwDestroyWindow(mainWindow);

  // Close the GLFW library
  glfwTerminate();
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
    camera.SetTransformation(glm::vec3(0.0f, 0.0f, -5.0f), glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f));
}

void renderScene()
{
  // Clear the color buffer
  glClearColor(0.1f, 0.2f, 0.4f, 1.0f);
  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

  // Tell OpenGL we'd like to use the previously compiled shader program
  glUseProgram(shaderProgram);

  // Update the transformation & projection matrices
  glUniformMatrix4fv(0, 1, GL_FALSE, glm::value_ptr(camera.GetTransformation()));
  glUniformMatrix4fv(1, 1, GL_FALSE, glm::value_ptr(camera.GetProjection()));

  // Draw the scene geometry using index buffer instead of relying on the fact that 3 consecutive
  // vertices form a triangle. Bind the appropriate VAO, VBO, IBO and describe them
  glBindVertexArray(vao);
  glBindBuffer(GL_ARRAY_BUFFER, vbo);

  // Positions: 3 floats, stride = 5 * sizeof(float), offset = 0
  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(0));
  glEnableVertexAttribArray(0);
  // Texture coordinates: 2 floats, stride = 5 * sizeof(float), offset = 3
  glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(3 * sizeof(float)));
  glEnableVertexAttribArray(1);

  // The actual draw
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ibo);
  glDrawElements(GL_TRIANGLES, scene.GetCubeIBSize(), GL_UNSIGNED_INT, reinterpret_cast<void*>(0));

  // Unbind the shader program and other resources
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
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

  // Create the scene geometry
  createGeometry();

  // Enter the application main loop
  mainLoop();

  // Release used resources and exit
  shutDown();
  return 0;
}
