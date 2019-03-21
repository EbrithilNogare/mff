#pragma once

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

static const float PI = 3.1415926535897932384626433832795f;
static const float PI_HALF = 1.5707963267948966192313216916398f;

// Fast transformation matrix inversion, assumes affine transform
inline glm::mat4x4 fastMatrixInverse(const glm::mat4x4& matrix)
{
  glm::mat3x3 inv = glm::transpose(glm::mat3x3(matrix));
  return glm::mat4x4(glm::vec4(inv[0], 0.0f),
                     glm::vec4(inv[1], 0.0f),
                     glm::vec4(inv[2], 0.0f),
                     glm::vec4(-inv * glm::vec3(matrix[3]), 1.0f));
}

inline float getRandom(float min, float max)
{
  return min + static_cast<float>(rand()) / (static_cast <float>(RAND_MAX / (max - min)));
}
