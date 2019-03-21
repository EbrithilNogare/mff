#include <cstdio>
#include <glad/glad.h>
#include <GLFW/glfw3.h>

// ----------------------------------------------------------------------------

// Vertex shader source
static const char* vsSource[] = { R"(
#version 440 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;
out vec3 vColor;

void main()
{
  vColor = color.rgb;
  gl_Position = vec4(position.xyz, 1.0f);
}
)", "" };

// Fragment shader source
static const char* fsSource[] = { R"(
#version 440 core

in vec3 vColor;
out vec4 color;

void main()
{
  color = vec4(vColor.rgb, 1.0f);
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

// Max buffer length
static const unsigned int MAX_BUFFER_LENGTH = 256;
// Main window handle
GLFWwindow* mainWindow = nullptr;
// Shader program handle
GLuint shaderProgram = 0;
// Vertex Array Object handle (i.e., vertex buffer)
GLuint vertexArrayObject = 0;
// Buffer object data
GLuint buffer;

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
  glGenVertexArrays(1, &vertexArrayObject);
  glBindVertexArray(vertexArrayObject);

  // Normally you'd fill it with geometry, we'll do that next time
  float vertices[] = {-0.25f, -0.25f, 0.5f,
                        1.0f,  0.0f,  0.0f,
                       0.25f, -0.25f, 0.5f,
                        0.0f,  1.0f,  0.0f,
                       0.25f,  0.25f, 0.5f,
                        0.0f,  0.0f,  1.0f,
                      -0.25f,  0.25f, 0.5f,
                        1.0f,  0.0f,  0.0f,
                      -0.25f, -0.25f, 0.5f,
                        0.0f,  1.0f,  0.0f,
                       0.25f,  0.25f, 0.5f,
                        0.0f,  0.0f,  1.0f};

  // Generate the memory storage and fill it with data
  glGenBuffers(1, &buffer);
  glBindBuffer(GL_ARRAY_BUFFER, buffer);
  glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 2 * 3 * 6, vertices, GL_STATIC_DRAW);

  // Tell OpenGL where to find the data:
  // Positions: 3 floats, stride = 6 (6 floats/vertex), offset = 0
  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), reinterpret_cast<void*>(0));
  glEnableVertexAttribArray(0);
  // Colors: 3 floats, stride = 6 (6 floats/vertex), offset = 3
  glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), reinterpret_cast<void*>(3 * sizeof(float)));
  glEnableVertexAttribArray(1);
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
  glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

  // Create the window
  mainWindow = glfwCreateWindow((int)WindowParams::Width, (int)WindowParams::Height, "NPGR019 - Lab 01", nullptr, nullptr);
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

  // Register a window resize callback
  glfwSetFramebufferSizeCallback(mainWindow, resizeCallback);

  // Set the OpenGL viewport
  glViewport(0, 0, (int)WindowParams::Width, (int)WindowParams::Height);

  return true;
}

// Helper method for graceful shutdown
void shutDown()
{
  // Release the vertex buffer
  glDeleteVertexArrays(1, &vertexArrayObject);

  // Release the shader program
  glDeleteProgram(shaderProgram);

  // Relase the window
  glfwDestroyWindow(mainWindow);

  // Close the GLFW library
  glfwTerminate();
}

// Helper method for handling input events
void processInput()
{
  // Notify the window that user wants to exit the application
  if (glfwGetKey(mainWindow, GLFW_KEY_ESCAPE) == GLFW_PRESS)
    glfwSetWindowShouldClose(mainWindow, true);
}

void renderScene()
{
  // Clear the color buffer
  glClearColor(0.1f, 0.2f, 0.4f, 1.0f);
  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

  // Tell OpenGL we'd like to use the previously compiled shader program
  glUseProgram(shaderProgram);

  // Draw the scene geometry - just tell OpenGL we're drawing at this point
  glPointSize(10.0f);
  glDrawArrays(GL_TRIANGLES, 0, 6);

  // Unbind the shader program
  glUseProgram(0);
}

// Helper method for implementing the application main loop
void mainLoop()
{
  while (!glfwWindowShouldClose(mainWindow))
  {
    // Poll the events like keyboard, mouse, etc.
    glfwPollEvents();

    // Process keyboard input
    processInput();

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
