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


class Scene
{
private:
	std::vector<Model> models;
public:
	Scene() {
		models.push_back(Model("../resources/models/scene.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(1.0f)));
	}
	void Render(Shader shader) {
		for (Model model : models) {
			model.Draw(shader);
		}
	}
};
