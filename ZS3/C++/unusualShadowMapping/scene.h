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
#include "json.hpp"
#include "model.h"
#include "shader.h"
#include "light.h"
#include "loadTexture.h"

# define M_PI           3.14159265358979323846  /* pi */

class Scene
{
private:
	std::vector<Model> models;
	std::vector<Model> transparentModels;
public:
	std::map<std::string, Light> lights;

	Scene(std::string pathToConfig) {
		std::ifstream configFile(pathToConfig);
		if (!configFile.is_open()) {
			std::cout << "config file not found at \"" << pathToConfig << "\"" << std::endl;
			return;
		}

		try {
			nlohmann::json configData;
			configFile >> configData;

			// load models
			for (auto& modelData : configData["models"])
			{
				std::string modelPath = FileSystem::getPath(modelData["model"]);
				std::string texturePath = FileSystem::getPath(modelData["texture"]);
				glm::vec3 position = glm::vec3(modelData["position"][0], modelData["position"][1], modelData["position"][2]);
				glm::vec3 rotation = glm::vec3(modelData["rotation"][0], modelData["rotation"][1], modelData["rotation"][2]);
				rotation *= M_PI;
				glm::vec3 scale = glm::vec3(modelData["scale"][0], modelData["scale"][1], modelData["scale"][2]);
				bool transparent = modelData["transparent"];
				unsigned int texture = loadTexture(std::string(texturePath).c_str());

				if (transparent)
					transparentModels.push_back(Model(modelPath, position, rotation, scale, texture));
				else
					models.push_back(Model(modelPath, position, rotation, scale, texture));
			}

			// load lights
			lights.insert(std::pair<std::string, Light>("sun", Light()));
			lights.at("sun").lookAt = glm::vec3(configData["sun"]["lookAt"][0], configData["sun"]["lookAt"][1], configData["sun"]["lookAt"][2]);
			lights.at("sun").near_plane = configData["sun"]["near_plane"];
			lights.at("sun").far_plane = configData["sun"]["far_plane"];
			lights.at("sun").fov = glm::radians((float)configData["sun"]["fov"]);
			lights.at("sun").setPosition(glm::vec3(configData["sun"]["position"][0], configData["sun"]["position"][1], configData["sun"]["position"][2]));

			lights.insert(std::pair<std::string, Light>("projection", Light()));
			lights.at("projection").lookAt = glm::vec3(configData["projection"]["lookAt"][0], configData["projection"]["lookAt"][1], configData["projection"]["lookAt"][2]);
			lights.at("projection").near_plane = configData["projection"]["near_plane"];
			lights.at("projection").far_plane = configData["projection"]["far_plane"];
			lights.at("projection").fov = glm::radians((float)configData["projection"]["fov"]);
			lights.at("projection").setPosition(glm::vec3(configData["projection"]["position"][0], configData["projection"]["position"][1], configData["projection"]["position"][2]));
		}
		catch (const std::exception & e) {
			std::cout << "error while reading json config:\n" << e.what() << std::endl;
		}
	}
	void Render(Shader& shader) {
		RenderSolid(shader);
		RenderTransparent(shader);
	}
	void RenderSolid(Shader& shader) {
		glDisable(GL_BLEND);
		for (Model model : models) {
			model.Draw(shader);
		}
	}
	void RenderTransparent(Shader& shader) {
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		for (Model model : transparentModels) {
			model.Draw(shader);
		}
	}
};
