#pragma once

#include <glad/glad.h>
#include <glm/glm.hpp>

// Vertex structure for buffers
struct Vertex
{
  // Position
  float x, y, z;
  // Normal
  float nx, ny, nz;
  // Tangent
  float tx, ty, tz;
  // Texture coordinates
  float u, v;
};

// Structure for holding instance data
struct InstanceData
{
  // Transformation matrix
  glm::mat4x4 model;
  // Texture repeat factor (vec4 to include padding)
  glm::vec4 texRepeat;
};

// Class for handling geometry creation and holding scene info
class Scene
{
public:
  enum Model {
    Quad, Cube, NumModels
  };

  enum Textures {
    White, Grey, Blue, CheckerBoard, Diffuse, Normal, Specular, Occlusion, NumTextures
  };

  enum Samplers {
    Anisotropic, NumSamplers
  };

  // Maximum number of allowed instances - SSBO can be up to 128 MB!
  static const unsigned int MaxInstances = 1024;

  // Create scene geometry
  void CreateGeometry();
  // Load textures
  void LoadTextures();
  // Called upon shutdown to release resources
  void ReleaseResources();
  // Get the specified VAO
  GLuint GetVAO(int i) { return _vao[i]; }
  // Get the number of indices stored in the specified index buffer
  GLsizei GetNumIndices(int i) { return _numIndices[i]; }
  // Get the Shader Storage Buffer
  GLuint GetSBO() { return _sbo; }
  // Get the specified texture
  GLuint GetTexture(int i) { return _tex[i]; }
  // Get the specified texture
  GLuint GetSampler(int i) { return _sampler[i]; }

private:
  // One Vertex Array Object per model
  GLuint _vao[NumModels];
  // One Vertex Buffer per model
  GLuint _vbo[NumModels];
  // One Index Buffer per model
  GLuint _ibo[NumModels];
  // Size of Index Buffer per model
  GLsizei _numIndices[NumModels];
  // Single Storage Buffer for all models used for instance data
  GLuint _sbo;
  // Textures
  GLuint _tex[NumTextures];
  // Texture samplers
  GLuint _sampler[NumSamplers];
  
  // Helper function for creating quad model
  void CreateQuad();
  // Helper function for creating cube model
  void CreateCube();
  // Helper function for creating checkerboard texture
  void CreateCheckerBoardTexture();
};

