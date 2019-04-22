function loadTextResource(url, callback) {
	var request = new XMLHttpRequest();
	request.open('GET', url + '?please-dont-cache=' + Math.random(), true);
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
		faces: [],
		texturecoords: [],
		normals: []
	};
	const texturecoords = [];
	const normals = [];

	obj = obj.split("\n");
	for (let i = 0; i < obj.length; i++) {
		const element = obj[i];
		const splited=element.split(" ");
		switch(splited[0]){
			case "v":
			model.vertices.push(parseFloat(splited[1]), parseFloat(splited[2]), parseFloat(splited[3]));
				break;
			case "vt":
				texturecoords.push([parseFloat(splited[1]), parseFloat(splited[2])]);
				break;
			case "vn":
				normals.push([parseFloat(splited[1]), parseFloat(splited[2]), parseFloat(splited[3])]);
				break;
			case "f":
				if(splited.length>4) console.error("model must have triangle faces!", `line number${i}`);
				for (let i = 1; i <= 3; i++) {	
					const index = splited[i].split("/");			
					model.texturecoords[(index[0]-1)*2+0] = texturecoords[index[1]-1][0];
					model.texturecoords[(index[0]-1)*2+1] = texturecoords[index[1]-1][1];
					
					model.normals[(index[0]-1)*3+0] = normals[index[2]-1][0];
					model.normals[(index[0]-1)*3+1] = normals[index[2]-1][1];
					model.normals[(index[0]-1)*3+2] = normals[index[2]-1][2];
				}
				model.faces.push(splited[1].split("/")[0]-1, splited[2].split("/")[0]-1, splited[3].split("/")[0]-1);
				break;
			case "#": 			//comment
			case "o": 			//obejct name
			case "usemtl": 		//material used
			case "mtllib": 		//material file
			case "s": 			//shading groups or off
			case "": 			//file end
				break; 			//just ignore them
			default:
				console.warn(`not implemented type in obj file: ${splited[0]}`);
				break;
		}
		
	}
	return model;
}
