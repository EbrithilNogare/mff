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
public:
	Scene() {
		unsigned int wall = loadTexture(std::string("../resources/textures/wall.png").c_str());
		unsigned int ColorGrid = loadTexture(std::string("../resources/textures/ColorGrid.png").c_str());
		unsigned int wood = loadTexture(std::string("../resources/textures/wood.jpg").c_str());

		models.push_back(Model("../resources/models/floor.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(1.0f), wood));
		models.push_back(Model("../resources/models/room.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(1.0f), wall));
		models.push_back(Model("../resources/models/sphere.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(1.0f), ColorGrid));
		models.push_back(Model("../resources/models/suzanne.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(1.0f), ColorGrid));
	}
	void Render(Shader shader) {
		for (Model model : models) {
			model.Draw(shader);
		}
	}
};
