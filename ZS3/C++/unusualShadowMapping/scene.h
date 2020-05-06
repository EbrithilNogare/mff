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

# define M_PI           3.14159265358979323846  /* pi */

unsigned int loadTexture(char const* path)
{
	unsigned int textureID;
	glGenTextures(1, &textureID);

	int width, height, nrComponents;
	unsigned char* data = stbi_load(path, &width, &height, &nrComponents, 0);
	if (data)
	{
		GLenum format;
		if (nrComponents == 1)
			format = GL_RED;
		else if (nrComponents == 3)
			format = GL_RGB;
		else if (nrComponents == 4)
			format = GL_RGBA;

		glBindTexture(GL_TEXTURE_2D, textureID);
		glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, format == GL_RGBA ? GL_CLAMP_TO_EDGE : GL_REPEAT); // for this tutorial: use GL_CLAMP_TO_EDGE to prevent semi-transparent borders. Due to interpolation it takes texels from next repeat 
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, format == GL_RGBA ? GL_CLAMP_TO_EDGE : GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

		stbi_image_free(data);
	}
	else
	{
		std::cout << "Texture failed to load at path: " << path << std::endl;
		stbi_image_free(data);
	}

	return textureID;
}

class Scene
{
private:
	std::vector<Model> models;
	std::vector<Model> transparentModels;
public:
	Scene() {
		unsigned int cathedralAO = loadTexture(std::string("../resources/textures/cathedralAO.png").c_str());
		unsigned int grass = loadTexture(std::string("../resources/textures/grass.jpg").c_str());
		unsigned int mosaic = loadTexture(std::string("../resources/textures/mosaic.png").c_str());
		
		unsigned int room = loadTexture(std::string("../resources/textures/room diffuse+ao.png").c_str());
		models.push_back(Model("../resources/models/room.obj", glm::vec3(0), glm::vec3(0, M_PI, 0), glm::vec3(1.0f), room));
		unsigned int droneTexture = loadTexture(std::string("../resources/textures/marble.png").c_str());
		models.push_back(Model("../resources/models/drone.obj", glm::vec3(0,1,0), glm::vec3(0, M_PI, 0), glm::vec3(1.0f), droneTexture));

		unsigned int projection = loadTexture(std::string("../resources/textures/mosaic.png").c_str());
		transparentModels.push_back(Model("../resources/models/projectionPlane.obj", glm::vec3(0), glm::vec3(0, M_PI, 0), glm::vec3(1.0f), projection));

		//models.push_back(Model("../resources/models/cathedralS.obj", glm::vec3(0), glm::vec3(0), glm::vec3(.1f), cathedralAO));
		//models.push_back(Model("../resources/models/grass.obj", glm::vec3(0), glm::vec3(0), glm::vec3(.1f), grass));
		
		//transparentModels.push_back(Model("../resources/models/cathedralT.obj", glm::vec3(0), glm::vec3(0), glm::vec3(.1f), mosaic));
	}
	void Render(Shader shader) {
		RenderSolid(shader);
		RenderTransparent(shader);
	}
	void RenderSolid(Shader shader) {
		glDisable(GL_BLEND);
		for (Model model : models) {
			model.Draw(shader);
		}
	}
	void RenderTransparent(Shader shader) {
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		for (Model model : transparentModels) {
			model.Draw(shader);
		}
	}
};
