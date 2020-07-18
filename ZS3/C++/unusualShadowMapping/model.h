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

class Model
{
private:
	unsigned int VAO = 0;
	unsigned int VBO = 0;
	unsigned int texture = 0;
	glm::vec3 position;
	glm::vec3 rotation;
	glm::vec3 scale;
	std::vector<float> bufferData; // 3 vertices, 2 uvs, 3 normals
	void LoadModelData(std::string& pathToModel) {
		std::vector<std::array<float, 3>> vertices;
		std::vector<std::array<float, 2>> textureCoords;
		std::vector<std::array<float, 3>> normals;
		std::vector<std::array<int, 3>> faces;

		std::ifstream file(pathToModel);
		if (!file.is_open()) {
			std::cout << "file with model not found: " << pathToModel << std::endl;
			return;
		}

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

					int verticesIndex = std::stoi(facesSplited[0]) - 1;
					bufferData.push_back(vertices[verticesIndex][0]);
					bufferData.push_back(vertices[verticesIndex][1]);
					bufferData.push_back(vertices[verticesIndex][2]);

					int textureIndex = std::stoi(facesSplited[1]) - 1;
					bufferData.push_back(textureCoords[textureIndex][0]);
					bufferData.push_back(1-textureCoords[textureIndex][1]);

					int normalsIndex = std::stoi(facesSplited[2]) - 1;
					bufferData.push_back(normals[normalsIndex][0]);
					bufferData.push_back(normals[normalsIndex][1]);
					bufferData.push_back(normals[normalsIndex][2]);
				}
			}
			else {
				// unsupported model description
				// eg. material definition, comments
			}
		}
		file.close();
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
	void Draw(Shader& shader) {	
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
		glDrawArrays(GL_TRIANGLES, 0, (GLsizei)bufferData.size());
		glBindVertexArray(0);
	}	
	void setPosition(glm::vec3 position) {
		this->position = position;
	}
};
