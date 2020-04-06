// main.cpp
// author: Davud Napravnik

#include "main.h"

int main() {
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
	if (window == NULL)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
	glfwSetCursorPosCallback(window, mouse_callback);
	glfwSetScrollCallback(window, scroll_callback);

	glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}

	glEnable(GL_DEPTH_TEST);
	glCullFace(GL_FRONT);

	std::map<std::string, Shader> shaders;
	shaders.insert(std::pair<std::string, Shader>("shader", Shader(getLocalPath("shaders/shadow_mapping.vs"), getLocalPath("shaders/shadow_mapping.fs"))));
	shaders.insert(std::pair<std::string, Shader>("simpleDepthShader", Shader(getLocalPath("shaders/shadow_mapping_depth.vs"), getLocalPath("shaders/shadow_mapping_depth.fs"))));
	shaders.insert(std::pair<std::string, Shader>("simpleColorShader", Shader(getLocalPath("shaders/shadow_mapping_color.vs"), getLocalPath("shaders/shadow_mapping_color.fs"))));
	shaders.insert(std::pair<std::string, Shader>("debugDepthQuad", Shader(getLocalPath("shaders/debugDepthQuad.vs"), getLocalPath("shaders/debugDepthQuad.fs"))));
	shaders.insert(std::pair<std::string, Shader>("debugColorQuad", Shader(getLocalPath("shaders/debugColorQuad.vs"), getLocalPath("shaders/debugColorQuad.fs"))));

	
	shaders.at("shader").use();
	shaders.at("shader").setInt("diffuseTexture", 0);
	shaders.at("shader").setInt("shadowMap", 1);
	shaders.at("simpleColorShader").use();
	shaders.at("simpleColorShader").setInt("diffuseTexture", 0);
	shaders.at("debugDepthQuad").use();
	shaders.at("debugDepthQuad").setInt("depthMap", 0);
	shaders.at("debugColorQuad").use();
	shaders.at("debugColorQuad").setInt("colorMap", 0);


	Scene scene;
	Light light = Light(glm::vec3(-2.0f, 4.0f, -1.0f));

	while (!glfwWindowShouldClose(window))
	{
		processInput(window);

		light.setPosition(glm::vec3(cos(sin(glfwGetTime())*5)*10, 1, sin(sin(glfwGetTime())*5)*10)); // todo
		//light.setPosition(glm::vec3(0,0,0)); // todo

		showFPS(window);
		render(window, scene, light, shaders);
	}


	glfwTerminate();
	return 0;
}

void render(GLFWwindow* window, Scene scene, Light light, std::map<std::string, Shader> shaders)
{
	float currentFrame = (float)glfwGetTime();
	deltaTime = currentFrame - lastFrame;
	lastFrame = currentFrame;


	
	// 1. render depth of scene to texture (from light's perspective)
	// --------------------------------------------------------------
	// render scene from light's point of view
	
	shaders.at("simpleDepthShader").use();
	shaders.at("simpleDepthShader").setMat4("lightSpaceMatrix", light.lightSpaceMatrix);
	glViewport(0, 0, light.mapWidth, light.mapHeight);
	glBindFramebuffer(GL_FRAMEBUFFER, light.depthMapFBO);
	glClear(GL_DEPTH_BUFFER_BIT);
	scene.RenderSolid(shaders.at("simpleDepthShader"));
	
	shaders.at("simpleColorShader").use();
	shaders.at("simpleColorShader").setMat4("lightSpaceMatrix", light.lightSpaceMatrix);
	glViewport(0, 0, light.mapWidth, light.mapHeight);
	glBindFramebuffer(GL_FRAMEBUFFER, light.colorMapFBO);
	glClearColor(1,1,1,1);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	scene.RenderSolid(shaders.at("simpleColorShader"));
	glClear(GL_COLOR_BUFFER_BIT); // todo not working z-buffer
	scene.RenderTransparent(shaders.at("simpleColorShader"));


	glBindFramebuffer(GL_FRAMEBUFFER, 0);



	// 2. render scene as normal using the generated depth/shadow map  
	// --------------------------------------------------------------
	glViewport(0, 0, SCR_WIDTH, SCR_HEIGHT);
	glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	shaders.at("shader").use();
	glm::mat4 projection = glm::perspective(glm::radians(camera.Zoom), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 100.0f);
	glm::mat4 view = camera.GetViewMatrix();
	shaders.at("shader").setMat4("projection", projection);
	shaders.at("shader").setMat4("view", view);
	// set light uniforms
	shaders.at("shader").setVec3("viewPos", camera.Position);
	shaders.at("shader").setVec3("lightPos", light.position);
	shaders.at("shader").setMat4("lightSpaceMatrix", light.lightSpaceMatrix);
	glActiveTexture(GL_TEXTURE1);
	glBindTexture(GL_TEXTURE_2D, light.depthMap);
	scene.Render(shaders.at("shader"));
	//light.RenderHelper(shaders.at("shader"));

	shaders.at("debugDepthQuad").use();
	shaders.at("debugDepthQuad").setFloat("near_plane", light.near_plane);
	shaders.at("debugDepthQuad").setFloat("far_plane", light.far_plane);
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, light.depthMap);
	renderQuad(glm::vec2(0, .75), glm::vec2(1 / 4.0));

	shaders.at("debugColorQuad").use();
	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, light.colorMap);
	renderQuad(glm::vec2(0, .5), glm::vec2(1 / 4.0));
	
	glfwSwapBuffers(window);
	glfwPollEvents();
}