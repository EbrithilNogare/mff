#pragma once

#include <string>
#include <fstream>
#include <sstream>
#include <iostream>
#include <vector>

#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include "model.h"
#include "shader.h"


class Light
{
private:
	bool visible = false; // todo to false
	Model sphere = Model("../resources/models/sphere.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(.2f));
public:
	glm::vec3 position;
	unsigned int mapWidth = 1024;
	unsigned int mapHeight = 1024;
	float near_plane = 5.0f;
	float far_plane = 20.0f;
	unsigned int depthMapFBO;
	unsigned int depthMap;
	unsigned int colorMapFBO;
	unsigned int colorMap;
	glm::mat4 lightSpaceMatrix = glm::mat4(0);
	Light(glm::vec3 initPosition) {
		position = initPosition;

		float borderColor[] = { 0.0, 0.0, 0.0, 1.0 };

		glGenTextures(1, &depthMap);
		glBindTexture(GL_TEXTURE_2D, depthMap);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, mapWidth, mapHeight, 0, GL_DEPTH_COMPONENT, GL_FLOAT, NULL);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
		glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

		glGenFramebuffers(1, &depthMapFBO);
		glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
		glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);


		glGenTextures(1, &colorMap);
		glBindTexture(GL_TEXTURE_2D, colorMap);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, mapWidth, mapHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, NULL);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
		glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

		glGenFramebuffers(1, &colorMapFBO);
		glBindFramebuffer(GL_FRAMEBUFFER, colorMapFBO);
		glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, colorMap, 0);


		glDrawBuffer(GL_NONE);
		glReadBuffer(GL_NONE);
		glBindFramebuffer(GL_FRAMEBUFFER, 0);
	}
	void RenderHelper(Shader shader) {
		if (visible) {
			sphere.setPosition(position);
			sphere.Draw(shader);
		}
	}
	void setPosition(glm::vec3 newPosition) {
		position = newPosition;

		glm::mat4 lightProjection, lightView;
		lightProjection = glm::perspective(glm::radians(45.0f), (GLfloat)mapWidth / (GLfloat)mapHeight, near_plane, far_plane);

		lightView = glm::lookAt(position, glm::vec3(0.0f), glm::vec3(0.0, 1.0, 0.0));
		lightSpaceMatrix = lightProjection * lightView;
	}
};
