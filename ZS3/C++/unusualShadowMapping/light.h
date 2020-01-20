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
	bool visible = true; // todo to false
	Model sphere = Model("../resources/models/ball.obj", glm::vec3(0, 0, 0), glm::vec3(1.0f), glm::vec3(.2f));
public:
	unsigned int mapWidth = 1024;
	unsigned int mapHeight = 1024;
	Light(glm::vec3 position) {
		sphere.setPosition(position);
	}
	void Render(Shader shader) {
		if (visible) {
			sphere.Draw(shader);
		}
	}
	void setPosition(glm::vec3 position) {
		sphere.setPosition(position);
	}
};
