#include "Camera.h"
#include "Math.h"
#include <glm/gtc/matrix_transform.hpp>

Camera::Camera():
  _transformation(1.0f),
  _projection(1.0f),
  _movementSpeed(5.0f),
  _sensitivity(0.1f)
{}

void Camera::SetTransformation(const glm::vec3& eye, const glm::vec3& lookAt, const glm::vec3& up)
{
  _transformation = glm::lookAt(eye, lookAt, up);
  _invTransformation = fastMatrixInverse(_transformation);
}

void Camera::SetProjection(float fov, float aspect, float near, float far)
{
  // Make sure you convert from degrees to radians as glm uses radians from 0.9.6 version
  _projection = glm::perspective(glm::radians(fov), aspect, near, far);
}

void Camera::Move(MovementDirections direction, const glm::vec2& mouseMove, float dt)
{
  // Prepare the new transformation matrix
  glm::mat4x4 transform(_invTransformation[0], _invTransformation[1], _invTransformation[2], glm::vec4(0.0f, 0.0f, 0.0f, 0.0f));
  glm::vec4& aside = transform[0];
  glm::vec4& up = transform[1];
  glm::vec4& dir = transform[2];

  // Update the camera orientation
  float yaw = atan2(dir.z, dir.x);
  float pitch = asin(dir.y);

  yaw -= mouseMove.x * _sensitivity * dt;
  pitch -= mouseMove.y * _sensitivity * dt;
  // TODO: check pitch > PI_HALF

  dir.x = cos(pitch) * cos(yaw);
  dir.y = sin(pitch);
  dir.z = cos(pitch) * sin(yaw);
  dir = glm::vec4(glm::normalize(glm::vec3(dir)), 0.0f);

  // Move the camera position
  glm::vec4& position = transform[3];
  position = _invTransformation[3];
  if ((int)direction & (int)MovementDirections::Forward)
    position += dir * _movementSpeed * dt;

  if ((int)direction & (int)MovementDirections::Backward)
    position -= dir * _movementSpeed * dt;

  if ((int)direction & (int)MovementDirections::Left)
    position -= aside * _movementSpeed * dt;

  if ((int)direction & (int)MovementDirections::Right)
    position += aside * _movementSpeed * dt;

  if ((int)direction & (int)MovementDirections::Up)
    position += up * _movementSpeed * dt;

  if ((int)direction & (int)MovementDirections::Down)
    position -= up * _movementSpeed * dt;
   
  // Update the transformation matrix and recalculate the inverse transformation
  // Set the aside, up, and dir using orthonormalization with scene up vector to avoid funky camera
  _invTransformation[0] = glm::vec4(glm::normalize(glm::cross(glm::vec3(0.0f, 1.0f, 0.0f), glm::vec3(dir))), 0.0f);
  _invTransformation[1] = glm::vec4(glm::normalize(glm::cross(glm::vec3(dir), glm::vec3(_invTransformation[0]))), 0.0f);
  _invTransformation[2] = glm::vec4(glm::normalize(glm::vec3(dir)), 0.0f);
  _invTransformation[3] = position;
  _transformation = fastMatrixInverse(_invTransformation);
}
