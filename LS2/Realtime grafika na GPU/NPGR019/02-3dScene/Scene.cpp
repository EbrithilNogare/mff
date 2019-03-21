#include "Scene.h"

void Scene::GetCube(std::vector<Vertex>& vb, std::vector<GLuint>& ib)
{
  // If the buffer was already created, just return the data
  if (vb.size() > 0)
  {
    vb = _cubeVB;
    ib = _cubeIB;
    return;
  }

  // Otherwise create the vertex buffer for a unit cube
  _cubeVB.reserve(24);

  // Top face
  _cubeVB.push_back({ -0.5f,  0.5f, -0.5f, 0.0f, 0.0f });
  _cubeVB.push_back({  0.5f,  0.5f, -0.5f, 1.0f, 0.0f });
  _cubeVB.push_back({  0.5f,  0.5f,  0.5f, 1.0f, 1.0f });
  _cubeVB.push_back({ -0.5f,  0.5f,  0.5f, 0.0f, 1.0f });

  // Bottom face
  _cubeVB.push_back({  0.5f, -0.5f, -0.5f, 0.0f, 0.0f });
  _cubeVB.push_back({ -0.5f, -0.5f, -0.5f, 1.0f, 0.0f });
  _cubeVB.push_back({ -0.5f, -0.5f,  0.5f, 1.0f, 1.0f });
  _cubeVB.push_back({  0.5f, -0.5f,  0.5f, 0.0f, 1.0f });
  
  // Front face
  _cubeVB.push_back({ -0.5f, -0.5f, -0.5f, 0.0f, 0.0f });
  _cubeVB.push_back({  0.5f, -0.5f, -0.5f, 1.0f, 0.0f });
  _cubeVB.push_back({  0.5f,  0.5f, -0.5f, 1.0f, 1.0f });
  _cubeVB.push_back({ -0.5f,  0.5f, -0.5f, 0.0f, 1.0f });

  // Back face
  _cubeVB.push_back({  0.5f, -0.5f,  0.5f, 0.0f, 0.0f });
  _cubeVB.push_back({ -0.5f, -0.5f,  0.5f, 1.0f, 0.0f });
  _cubeVB.push_back({ -0.5f,  0.5f,  0.5f, 1.0f, 1.0f });
  _cubeVB.push_back({  0.5f,  0.5f,  0.5f, 0.0f, 1.0f });

  // Left face
  _cubeVB.push_back({ -0.5f, -0.5f,  0.5f, 0.0f, 0.0f });
  _cubeVB.push_back({ -0.5f, -0.5f, -0.5f, 1.0f, 0.0f });
  _cubeVB.push_back({ -0.5f,  0.5f, -0.5f, 1.0f, 1.0f });
  _cubeVB.push_back({ -0.5f,  0.5f,  0.5f, 0.0f, 1.0f });
  
  // Right face
  _cubeVB.push_back({  0.5f, -0.5f, -0.5f, 0.0f, 0.0f });
  _cubeVB.push_back({  0.5f, -0.5f,  0.5f, 1.0f, 0.0f });
  _cubeVB.push_back({  0.5f,  0.5f,  0.5f, 1.0f, 1.0f });
  _cubeVB.push_back({  0.5f,  0.5f, -0.5f, 0.0f, 1.0f });
  
  // Fill in the index buffer
  ib.reserve(36);
  for (int face = 0; face < 6; ++face)
  {
    GLuint baseIndex = 4 * face;

    // One triangle
    _cubeIB.push_back(baseIndex);
    _cubeIB.push_back(baseIndex + 1);
    _cubeIB.push_back(baseIndex + 2);

    // Other triangle
    _cubeIB.push_back(baseIndex + 2);
    _cubeIB.push_back(baseIndex + 3);
    _cubeIB.push_back(baseIndex);
  }

  vb = _cubeVB;
  ib = _cubeIB;
}
