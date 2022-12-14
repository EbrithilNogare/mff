#pragma once

// main.h
// author: David Napravnik

#include <map>
#include <iostream>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include "stb_image.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "filesystem.h"
#include "shader.h"
#include "camera.h"
#include "model.h"
#include "scene.h"
#include "light.h"


// settings
unsigned int SCR_WIDTH = 1600;
unsigned int SCR_HEIGHT = 900;

// camera
Camera camera(glm::vec3(0.0f, 2.0f, -4.0f));
float lastX = (float)SCR_WIDTH / 2.0f;
float lastY = (float)SCR_HEIGHT / 2.0f;
bool firstMouse = true;

// timing
float deltaTime = 0.0f;
float lastFrame = 0.0f;

double lastTime = glfwGetTime();
unsigned int nbFrames = 0;

void render(GLFWwindow* window, Scene& scene, std::map<std::string, Shader>& shaders);

unsigned int quadVAO = 0;
unsigned int quadVBO;
void renderQuad(glm::vec2 position, glm::vec2 size)
{
	float quadVertices[] = {
		// vertices coords																		// texture coords
		position.x * 2.0f - 1.0f,					position.y * 2.0f - 1.0f + size.y * 2.0f,	0.0f,	0.0f, 1.0f,
		position.x * 2.0f - 1.0f,					position.y * 2.0f - 1.0f,					0.0f,	0.0f, 0.0f,
		position.x * 2.0f - 1.0f + size.x * 2.0f,	position.y * 2.0f - 1.0f + size.y * 2.0f,	0.0f,	1.0f, 1.0f,
		position.x * 2.0f - 1.0f + size.x * 2.0f,	position.y * 2.0f - 1.0f,					0.0f,	1.0f, 0.0f,
	};

	if (quadVAO == 0)
	{
		// setup plane VAO
		glGenVertexArrays(1, &quadVAO);
		glGenBuffers(1, &quadVBO);
		glBindVertexArray(quadVAO);

		glBindBuffer(GL_ARRAY_BUFFER, quadVBO);
		glBufferData(GL_ARRAY_BUFFER, sizeof(quadVertices), &quadVertices, GL_STATIC_DRAW);

		glEnableVertexAttribArray(0);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);

		glEnableVertexAttribArray(1);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
	}
	glBindVertexArray(quadVAO);

	glBufferData(GL_ARRAY_BUFFER, sizeof(quadVertices), &quadVertices, GL_STATIC_DRAW);

	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
	glBindVertexArray(0);
}

void processInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);

	if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
		camera.ProcessKeyboard(Camera::Camera_Movement::FORWARD, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
		camera.ProcessKeyboard(Camera::Camera_Movement::BACKWARD, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
		camera.ProcessKeyboard(Camera::Camera_Movement::LEFT, deltaTime);
	if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
		camera.ProcessKeyboard(Camera::Camera_Movement::RIGHT, deltaTime);

	//std::cout << "x: " << camera.Position.x << ", y: " << camera.Position.y << ", z: " << camera.Position.z << std::endl;
}

void processLightDebugInput(GLFWwindow* window, Light* light) {
	bool changed = false;
	if (glfwGetKey(window, GLFW_KEY_KP_ADD) == GLFW_PRESS) {
		light->far_plane += .1f;
		changed = true;
	}
	if (glfwGetKey(window, GLFW_KEY_KP_SUBTRACT) == GLFW_PRESS) {
		light->far_plane -= .1f;
		changed = true;
	}
	if (glfwGetKey(window, GLFW_KEY_KP_MULTIPLY) == GLFW_PRESS) {
		light->near_plane += .1f;
		changed = true;
	}
	if (glfwGetKey(window, GLFW_KEY_KP_DIVIDE) == GLFW_PRESS) {
		light->near_plane -= .1f;
		changed = true;
	}
	if(changed)
		std::cout << "lights focus changed to:\nnear_plane:" << light->near_plane << "\nfar_plane: " << light->far_plane << std::endl;
}


void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	SCR_WIDTH = width;
	SCR_HEIGHT = height;
	glViewport(0, 0, SCR_WIDTH, SCR_HEIGHT);
}

void mouse_callback(GLFWwindow* window, double xpos, double ypos)
{
	if (firstMouse)
	{
		lastX = (float)xpos;
		lastY = (float)ypos;
		firstMouse = false;
	}

	float xoffset = (float)xpos - lastX;
	float yoffset = lastY - (float)ypos;

	lastX = (float)xpos;
	lastY = (float)ypos;

	camera.ProcessMouseMovement(xoffset, yoffset);
}

void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
{
	camera.ProcessMouseScroll((float)yoffset);
}

void showFPS(GLFWwindow* pWindow)
{
	double currentTime = glfwGetTime();
	double delta = currentTime - lastTime;
	nbFrames++;
	if (delta >= 1.0) {
		double fps = double(nbFrames) / delta;
		std::stringstream ss;
		ss << "unusual shadow mapping" << " [" << fps << " fps]";
		glfwSetWindowTitle(pWindow, ss.str().c_str());

		nbFrames = 0;
		lastTime = currentTime;
	}
}