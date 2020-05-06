#pragma once

#include <string>
#include <array>
#include <fstream>
#include <sstream>
#include <iostream>
#include <vector>

#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>

#include "common.h"


unsigned int TextureFromFile(const char* path, const std::string& directory, bool gamma = false);

class Model
{
private:
	unsigned int VAO = 0;
	unsigned int VBO = 0;
	unsigned int texture = 0;
	glm::vec3 position;
	glm::vec3 rotation;
	glm::vec3 scale;
	//std::vector<float> bufferData;
	std::vector<float> bufferData; // 3 vertices, 2 uvs, 3 normals
	void LoadModelData(std::string pathToModel) {
		std::vector<std::array<float, 3>> vertices;
		std::vector<std::array<float, 2>> textureCoords;
		std::vector<std::array<float, 3>> normals;
		std::vector<std::array<int, 3>> faces;

		std::ifstream file(pathToModel);
		if (file.is_open()) {
			std::string line;
			while (getline(file, line)) {
				std::vector<std::string> splited = SplitString(line, ' ');			
				if (splited[0] == "v") {
					vertices.push_back(std::array<float, 3>{std::stof(splited[1]), std::stof(splited[2]), std::stof(splited[3])});
				}
				else if (splited[0] == "vt") {
					textureCoords.push_back(std::array<float, 2>{std::stof(splited[1]), std::stof(splited[2])});
				}
				else if (splited[0] == "vn") {
					normals.push_back(std::array<float, 3>{std::stof(splited[1]), std::stof(splited[2]), std::stof(splited[3])});
				}
				else if (splited[0] == "f") {					
					if (splited.size() != 4)
						std::cerr << "ERROR: model must have triangle faces!\nin: " << line << "\nfrom: " << pathToModel << std::endl;
					
					for (int i = 1; i <= 3; i++) {
						auto facesSplited = SplitString(splited[i], '/');

						bufferData.push_back(vertices[std::stoi(facesSplited[0])-1.0][0]);
						bufferData.push_back(vertices[std::stoi(facesSplited[0])-1.0][1]);
						bufferData.push_back(vertices[std::stoi(facesSplited[0])-1.0][2]);

						bufferData.push_back(textureCoords[std::stoi(facesSplited[1])-1.0][0]);
						bufferData.push_back(1-textureCoords[std::stoi(facesSplited[1])-1.0][1]);

						bufferData.push_back(normals[std::stoi(facesSplited[2])-1.0][0]);
						bufferData.push_back(normals[std::stoi(facesSplited[2])-1.0][1]);
						bufferData.push_back(normals[std::stoi(facesSplited[2])-1.0][2]);
					}
				}
				else {
					// skip it
				}
			}
			file.close();
		}		
	}
public:
	Model(std::string pathToModel, glm::vec3 position, glm::vec3 rotation, glm::vec3 scale, unsigned int texture = 0) {
		glGenVertexArrays(1, &VAO);
		glGenBuffers(1, &VAO);
		// fill buffer
		LoadModelData(pathToModel);
		glBindBuffer(GL_ARRAY_BUFFER, VAO);
		glBufferData(GL_ARRAY_BUFFER, bufferData.size() * sizeof(bufferData[0]), &bufferData[0], GL_STATIC_DRAW);
		// link vertex attributes
		glBindVertexArray(VAO);
		glEnableVertexAttribArray(0); // vertex
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(2); // normal
		glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(5 * sizeof(float)));
		glEnableVertexAttribArray(1); // texture coor
		glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
		glBindBuffer(GL_ARRAY_BUFFER, 0);
		glBindVertexArray(0);

		this->position = position;
		this->rotation = rotation;
		this->scale = scale;
		this->texture = texture;
	}
	void Draw(Shader shader) {	
		glm::mat4 model = glm::mat4(1.0f);
		model = glm::rotate(model, rotation.x, glm::vec3(1, 0, 0)); // x
		model = glm::rotate(model, rotation.y, glm::vec3(0, 1, 0)); // y
		model = glm::rotate(model, rotation.z, glm::vec3(0, 0, 1)); // z
		model = glm::translate(model, position);
		model = glm::scale(model, scale);
		shader.setMat4("model", model);

		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, texture);
		
		// render
		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLES, 0, bufferData.size());
		glBindVertexArray(0);
	}	
	void setPosition(glm::vec3 position) {
		this->position = position;
	}
};

unsigned int TextureFromFile(const char* path, const std::string& directory, bool gamma)
{
	std::string filename = std::string(path);
	filename = directory + '/' + filename;

	unsigned int textureID;
	glGenTextures(1, &textureID);

	int width, height, nrComponents;
	unsigned char* data = stbi_load(filename.c_str(), &width, &height, &nrComponents, 0);
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

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
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
