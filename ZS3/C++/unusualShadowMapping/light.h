#pragma once

#include <string>
#include <fstream>
#include <sstream>
#include <iostream>
#include <vector>

#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include "filesystem.h"
#include "model.h"
#include "shader.h"
#include "loadTexture.h"

class Light
{
private:
	bool visible = true;
	unsigned int glow = loadTexture(std::string(FileSystem::getPath("resources/textures/sun.jpg")).c_str());
	Model sphere = Model(FileSystem::getPath("resources/models/sun.obj"), glm::vec3(0, 0, 0), glm::vec3(0.0f), glm::vec3(.1f), glow);
public:
	glm::vec3 position = glm::vec3(0, 0, 0);
	float lightTime = 0;
	unsigned int mapWidth = 2048;
	unsigned int mapHeight = 2048;
	float near_plane = 1.0f;
	float far_plane = 8.0f;
	glm::vec3 lookAt = glm::vec3(0, 0, 0);
	float fov = glm::radians(45.0f);
	GLuint depthMapFBO;
	GLuint depthMap;
	GLuint colorMapFBO;
	GLuint colorMap;
	GLuint depthMap2;

	glm::mat4 lightSpaceMatrix = glm::mat4(0);
	Light() {
		glGenTextures(1, &depthMap);
		glGenTextures(1, &depthMap2);
		glGenTextures(1, &colorMap);
		glGenFramebuffers(1, &depthMapFBO);
		glGenFramebuffers(1, &colorMapFBO);

		glBindTexture(GL_TEXTURE_2D, depthMap);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, mapWidth, mapHeight, 0, GL_DEPTH_COMPONENT, GL_FLOAT, NULL);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
		GLfloat borderColor[] = { 0.0, 0.0, 0.0, 1.0 };
		glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

		glBindTexture(GL_TEXTURE_2D, depthMap2);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, mapWidth, mapHeight, 0, GL_DEPTH_COMPONENT, GL_FLOAT, NULL);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
		glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

		glBindTexture(GL_TEXTURE_2D, colorMap);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, mapWidth, mapHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, NULL);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		

		glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
		glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);
		glDrawBuffer(GL_NONE);
		glReadBuffer(GL_NONE);

		GLenum status = glCheckFramebufferStatus(GL_FRAMEBUFFER);
		if (status != GL_FRAMEBUFFER_COMPLETE) {
			std::cout << "error while setting up framebuffer for light" << std::endl;
		}

		glBindFramebuffer(GL_FRAMEBUFFER, colorMapFBO);
		glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, colorMap, 0);
		glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap2, 0);
			   
		status = glCheckFramebufferStatus(GL_FRAMEBUFFER);
		if (status != GL_FRAMEBUFFER_COMPLETE) {
			std::cout << "error while setting up framebuffer for light" << std::endl;
		}

		glBindFramebuffer(GL_FRAMEBUFFER, 0);
	}
	void RenderHelper(Shader& shader) {
		if (visible) {
			sphere.setPosition(position);
			sphere.Draw(shader);
		}
	}
	void setPosition(glm::vec3 newPosition) {
		position = newPosition;

		glm::mat4 lightProjection, lightView;
		lightProjection = glm::perspective(fov, (GLfloat)mapWidth / (GLfloat)mapHeight, near_plane, far_plane);
		lightView = glm::lookAt(position, lookAt, glm::vec3(0.0, 1.0, 0.0));

		lightSpaceMatrix = lightProjection * lightView;
	}
	void addTick() {
		lightTime += .003f;
	}
};
