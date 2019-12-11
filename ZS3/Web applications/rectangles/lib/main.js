//import { maxFreeRect } from '../solution/solution.js';

$(function(){
	const zoomFactor = 2;
	$("#imgArea")
		.css("width", zoomFactor*window._width + "px")
		.css("height", zoomFactor*window._height + "px");
	
	let hue = 0;
	window._initialData.forEach(rect => {
		const div = $("<div class='rect'></div>");
		hue = hue + 47 % 360;
		div.css("color", `hsl(${hue}, 85%, 50%)`).css("background-color", `hsl(${hue}, 85%, 90%)`)
			.css("top", zoomFactor*rect.top + "px")
			.css("left", zoomFactor*rect.left + "px")
			.css("width", zoomFactor*(rect.width-2) + "px")
			.css("height", zoomFactor*(rect.height-2) + "px");
		$("#imgArea").append(div);
	});

	const freeRect = module.exports.maxFreeRect(window._width, window._height, window._initialData);

	const div = $("<div class='result'>result</div>");
	div.css("top", zoomFactor*freeRect.top + "px")
		.css("left", zoomFactor*freeRect.left + "px")
		.css("width", zoomFactor*(freeRect.width-2) + "px")
		.css("height", zoomFactor*(freeRect.height-2) + "px")
		.css("line-height", zoomFactor*(freeRect.height-2) + "px");
	$("#imgArea").append(div);
});
