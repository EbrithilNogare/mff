#include "Scene.h"
#include <vector>

#define STB_IMAGE_IMPLEMENTATION
#include <stb_image.h>

void Scene::LoadTextures()
{
  // Generate symbolic names for all samplers
  glGenSamplers(NumSamplers, _sampler);

  // Set texture filtering
  float maxAnistropy;
  glGetFloatv(GL_MAX_TEXTURE_MAX_ANISOTROPY, &maxAnistropy);
  glSamplerParameteri(_sampler[Anisotropic], GL_TEXTURE_MAG_FILTER, GL_LINEAR);
  glSamplerParameteri(_sampler[Anisotropic], GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
  glSamplerParameterf(_sampler[Anisotropic], GL_TEXTURE_MAX_ANISOTROPY, maxAnistropy);

  // Set texture addressing mode
  glSamplerParameteri(_sampler[Anisotropic], GL_TEXTURE_WRAP_S, GL_REPEAT);
  glSamplerParameteri(_sampler[Anisotropic], GL_TEXTURE_WRAP_T, GL_REPEAT);

  // Generate symbolic names for all textures
  glGenTextures(NumTextures, _tex);

  // --------------------------------------------------------------------------

  // Helper lamda for creating default textures
  auto generateDefaultTexture = [this](int index, unsigned char r, unsigned char g, unsigned char b)
  {
    // Create the texture object (first bind call for this name)
    glBindTexture(GL_TEXTURE_2D, _tex[index]);

    unsigned char data[] = { r, g, b };

    // Upload texture data: 2D texture, mip level 0, internal format RGB, width, height, border, input format RGB, type, data
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, 1, 1, 0, GL_RGB, GL_UNSIGNED_BYTE, data);

    // Unbind the texture
    glBindTexture(GL_TEXTURE_2D, 0);
  };

  generateDefaultTexture(White, 255, 255, 255);
  generateDefaultTexture(Grey, 127, 127, 127);
  generateDefaultTexture(Blue, 127, 127, 255);

  // Create the checkerboard texture
  CreateCheckerBoardTexture();

  // --------------------------------------------------------------------------

  // Helper lamda for loading textures
  auto loadTexture = [this](const char name[], int index)
  {
    // Load stored textures on the disk
    int width, height, numChannels;
    unsigned char *data = stbi_load(name, &width, &height, &numChannels, 0);

    if (!data)
    {
      printf("Failed to load texture: %s\n", name);
      return;
    }

    // Create the texture object (first bind call for this name)
    glBindTexture(GL_TEXTURE_2D, _tex[index]);

    // Upload texture data: 2D texture, mip level 0, internal format RGB, width, height, border, input format RGB, type, data
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
    glGenerateMipmap(GL_TEXTURE_2D);

    // Free the image data, we don't need them anymore
    stbi_image_free(data);

    // Unbind the texture
    glBindTexture(GL_TEXTURE_2D, 0);
  };

  loadTexture("data/Terracotta_Tiles_002_Base_Color.jpg", Diffuse);
  loadTexture("data/Terracotta_Tiles_002_Normal.jpg", Normal);
  loadTexture("data/Terracotta_Tiles_002_Roughness.jpg", Specular);
  loadTexture("data/Terracotta_Tiles_002_ambientOcclusion.jpg", Occlusion);
}

void Scene::CreateGeometry()
{
  // Generate symbolic names for VAO's for all models
  glGenVertexArrays(NumModels, _vao);
  // Generate symbolic names for VBO's for all models
  glGenBuffers(NumModels, _vbo);
  // Generate symbolic names for IBO's for all models
  glGenBuffers(NumModels, _ibo);

  // Create quad
  CreateQuad();

  // Create cube
  CreateCube();

  // Generate the instancing buffer but don't fill it with data
  // Note, that we're binding the buffer outside the VAO
  glGenBuffers(1, &_sbo);
  glBindBuffer(GL_SHADER_STORAGE_BUFFER, _sbo);
  glBufferData(GL_SHADER_STORAGE_BUFFER, MaxInstances * sizeof(InstanceData), nullptr, GL_STATIC_DRAW);
  glBindBufferBase(GL_SHADER_STORAGE_BUFFER, 0, _sbo);
}

void Scene::ReleaseResources()
{
  // Release vertex array objects
  glDeleteVertexArrays(NumModels, _vao);

  // Release vertex buffers
  glDeleteBuffers(NumModels, _vbo);

  // Release index buffers
  glDeleteBuffers(NumModels, _ibo);

  // Release instance buffers
  glDeleteBuffers(1, &_sbo);

  // Release textures
  glDeleteTextures(NumTextures, _tex);

  // Release samplers
  glDeleteSamplers(NumSamplers, _sampler);
}

void Scene::CreateQuad()
{
  // Prepare temporary storage for VB and create unit single sided quad
  std::vector<Vertex> vb;
  vb.reserve(4);

  // Front face
  vb.push_back({ -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });
  vb.push_back({  0.5f, -0.5f, 0.0f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f });
  vb.push_back({  0.5f,  0.5f, 0.0f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f });
  vb.push_back({ -0.5f,  0.5f, 0.0f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f });

  // Fill in the index buffer
  std::vector<GLuint> ib;
  ib.reserve(6);
  
  // One triangle
  ib.push_back(0);
  ib.push_back(1);
  ib.push_back(2);

  // Other triangle
  ib.push_back(2);
  ib.push_back(3);
  ib.push_back(0);

  // Store the number of indices
  _numIndices[Quad] = ib.size();

  // Bind the cube VAO - this also creates it
  glBindVertexArray(_vao[Quad]);

  // Create and bind VBO for vertex storage and fill it with data
  glBindBuffer(GL_ARRAY_BUFFER, _vbo[Quad]);
  glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vb.size(), static_cast<void*>(&*vb.begin()), GL_STATIC_DRAW);

  // Positions: 3 floats, stride = sizeof(Vertex), offset = 0
  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(0));
  glEnableVertexAttribArray(0);
  // Normal: 3 floats, stride = sizeof(Vertex), offset = 3 * 4 B
  glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(3 * sizeof(float)));
  glEnableVertexAttribArray(1);
  // Tangent: 3 floats, stride = sizeof(Vertex), offset = 6 * 4 B
  glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(6 * sizeof(float)));
  glEnableVertexAttribArray(2);
  // Texture coordinates: 2 floats, stride = sizeof(Vertex), offset = 9 * 4 B
  glVertexAttribPointer(3, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(9 * sizeof(float)));
  glEnableVertexAttribArray(3);

  // Create and bind the index buffer and fill it with data
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _ibo[Quad]);
  glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLuint) * ib.size(), static_cast<void*>(&*ib.begin()), GL_STATIC_DRAW);

  // Unbind the VAO, we'll bind it prior rendering
  glBindVertexArray(0);
}

void Scene::CreateCube()
{
  // Prepare temporary storage for VB and create unit cube vertex buffer
  std::vector<Vertex> vb;
  vb.reserve(24);

  // Top face
  vb.push_back({ -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });
  vb.push_back({  0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f });
  vb.push_back({  0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f });
  vb.push_back({ -0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f });

  // Bottom face
  vb.push_back({  0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });
  vb.push_back({ -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f });
  vb.push_back({ -0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f });
  vb.push_back({  0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f });
  
  // Front face
  vb.push_back({ -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f });
  vb.push_back({  0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f });
  vb.push_back({  0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f });
  vb.push_back({ -0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f });

  // Back face
  vb.push_back({  0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f });
  vb.push_back({ -0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f });
  vb.push_back({ -0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f });
  vb.push_back({  0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f });

  // Left face
  vb.push_back({ -0.5f, -0.5f,  0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f });
  vb.push_back({ -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f });
  vb.push_back({ -0.5f,  0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f });
  vb.push_back({ -0.5f,  0.5f,  0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f });
  
  // Right face
  vb.push_back({  0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f });
  vb.push_back({  0.5f, -0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f });
  vb.push_back({  0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f });
  vb.push_back({  0.5f,  0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f });

  // Fill in the index buffer
  std::vector<GLuint> ib;
  ib.reserve(36);
  for (int face = 0; face < 6; ++face)
  {
    GLuint baseIndex = 4 * face;

    // One triangle
    ib.push_back(baseIndex);
    ib.push_back(baseIndex + 1);
    ib.push_back(baseIndex + 2);

    // Other triangle
    ib.push_back(baseIndex + 2);
    ib.push_back(baseIndex + 3);
    ib.push_back(baseIndex);
  }

  // Store the number of indices
  _numIndices[Cube] = ib.size();

  // Bind the cube VAO - this also creates it
  glBindVertexArray(_vao[Cube]);

  // Create and bind VBO for vertex storage and fill it with data
  glBindBuffer(GL_ARRAY_BUFFER, _vbo[Cube]);
  glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vb.size(), static_cast<void*>(&*vb.begin()), GL_STATIC_DRAW);

  // Positions: 3 floats, stride = sizeof(Vertex), offset = 0
  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(0));
  glEnableVertexAttribArray(0);
  // Normal: 3 floats, stride = sizeof(Vertex), offset = 3 * 4 B
  glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(3 * sizeof(float)));
  glEnableVertexAttribArray(1);
  // Tangent: 3 floats, stride = sizeof(Vertex), offset = 6 * 4 B
  glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(6 * sizeof(float)));
  glEnableVertexAttribArray(2);
  // Texture coordinates: 2 floats, stride = sizeof(Vertex), offset = 9 * 4 B
  glVertexAttribPointer(3, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<void*>(9 * sizeof(float)));
  glEnableVertexAttribArray(3);

  // Create and bind the index buffer and fill it with data
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, _ibo[Cube]);
  glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLuint) * ib.size(), static_cast<void*>(&*ib.begin()), GL_STATIC_DRAW);

  // Unbind the VAO, we'll bind it prior rendering
  glBindVertexArray(0);
}

void Scene::CreateCheckerBoardTexture()
{
  // Create the texture object (first bind call for this name)
  glBindTexture(GL_TEXTURE_2D, _tex[CheckerBoard]);

  // Generate texture RGB data
  const int texSize = 128;
  const int stride = 3;
  const int checkerSize = 16;
  float data[stride * texSize * texSize];
  for (int y = 0; y < texSize; ++y)
  {
    for (int x = 0; x < texSize; ++x)
    {
      bool odd = ((x / checkerSize + y / checkerSize) & 1) > 0;
      float r = odd ? 0.15f : 0.85f;
      float g = odd ? 0.15f : 0.75f;
      float b = odd ? 0.60f : 0.30f;

      int i = y * stride * texSize + x * stride;
      data[i] = r;
      data[i + 1] = g;
      data[i + 2] = b;
    }
  }

  // Upload texture data: 2D texture, mip level 0, internal format RGB, width, height, border, input format RGB, type, data
  glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, texSize, texSize, 0, GL_RGB, GL_FLOAT, data);
  glGenerateMipmap(GL_TEXTURE_2D);

  // Unbind the texture
  glBindTexture(GL_TEXTURE_2D, 0);
}
