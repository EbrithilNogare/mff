function loadTextResource(url, callback) {
	var request = new XMLHttpRequest();
	request.open('GET', url + '?no-cache=' + Math.random(), true);
	request.onload = function () {
		if (request.status < 200 || request.status > 299) {
			callback('Error: HTTP Status ' + request.status + ' on resource ' + url);
		} else {
			callback(null, request.responseText);
		}
	};
	request.send();
};

function loadImage (url, callback) {
	var image = new Image();
	image.onload = function () {
		callback(null, image);
	};
	image.src = url;
};

function objToJSON(obj){
	const model = {
		vertices: [],
		texturecoords: [],
		normals: []
	};
	const vertices = [];
	const texturecoords = [];
	const normals = [];
	let faceCounter = 0;

	obj = obj.split("\n");
	for (let i = 0; i < obj.length; i++) {
		const element = obj[i];
		const splited=element.split(" ");
		switch(splited[0]){
			case "v":
				vertices.push([parseFloat(splited[1]), parseFloat(splited[2]), parseFloat(splited[3])]);
				break;
			case "vt":
				texturecoords.push([parseFloat(splited[1]), parseFloat(splited[2])]);
				break;
			case "vn":
				normals.push([parseFloat(splited[1]), parseFloat(splited[2]), parseFloat(splited[3])]);
				break;
			case "f":
				if(splited.length>4) throw `model must have triangle faces! \nline number${i}`;
				for (let i = 1; i <= 3; i++) {	
					const index = splited[i].split("/");	
					
					model.vertices[(faceCounter)*3+0] = vertices[index[0]-1][0];
					model.vertices[(faceCounter)*3+1] = vertices[index[0]-1][1];
					model.vertices[(faceCounter)*3+2] = vertices[index[0]-1][2];
					
					model.texturecoords[(faceCounter)*2+0] = texturecoords[index[1]-1][0];
					model.texturecoords[(faceCounter)*2+1] = texturecoords[index[1]-1][1];
					
					model.normals[(faceCounter)*3+0] = normals[index[2]-1][0];
					model.normals[(faceCounter)*3+1] = normals[index[2]-1][1];
					model.normals[(faceCounter)*3+2] = normals[index[2]-1][2];

					faceCounter++;
				}
				break;
			case "#": 			//comment
			case "o": 			//obejct name
			case "usemtl": 		//material used
			case "mtllib": 		//material file
			case "s": 			//shading groups or off
			case "": 			//file end
				break; 			//just ignore them
			default:
				throw `not implemented type in obj file: ${splited[0]}`;
				break;
		}
		
	}
	return model;
}
