#pragma once

#include <glad/glad.h>
#include <vector>

// Vertex structure for buffers
struct Vertex
{
  // Position
  float x, y, z;
  // Texture coordinates
  float u, v;
};

// Class for handling geometry creation and holding scene info
class Scene
{
public:

  // Creates/returns cube vertices
  void GetCube(std::vector<Vertex>& vb, std::vector<GLuint>& ib);
  // Return number of vertices in the cube vertex buffer
  GLuint GetCubeVBSize() const { return (GLuint)_cubeIB.size(); }
  // Return number of vertices in the cube index buffer
  GLuint GetCubeIBSize() const { return (GLuint)_cubeIB.size(); }

private:
  // Cube vertex buffer
  std::vector<Vertex> _cubeVB;
  // Cube index buffer
  std::vector<GLuint> _cubeIB;
};

