// expects variable data with json of midjourney chat export

const SEPARATOR_REGEXP = /[;, ]+/;

const inputDOM = document.getElementById("input");
let indexOfResult = 0;

if (!data || data.messages) {
  alert("no data found");
}

let canvas,
  ctx,
  fillStyle = "black",
  flag = false,
  prevX = 0,
  currX = 0,
  prevY = 0,
  currY = 0,
  dot_flag = false;

init();

function onlyUnique(value, index, self) {
  return self.indexOf(value) === index;
}

function prev() {
  indexOfResult = Math.max(0, indexOfResult - 1);
  search();
}
function next() {
  const imgRDOM = document.getElementById("imgR");
  if (imgRDOM.src == null) return;
  indexOfResult++;
  search();
}

function newSearch() {
  indexOfResult = 0;
  search();
}

function search() {
  const input = inputDOM.value.split(SEPARATOR_REGEXP);
  const result = data
    .map((item) => {
      let found = 0;
      input.forEach((word) => {
        item.text.includes(word) ? (found += 1) : null;
      });
      return {
        ...item,
        match: found / Math.max(input.length, item.text.length),
      };
    })
    .filter((item) => item.match > 0)
    .sort((itemA, itemB) => {
      return itemB.match - itemA.match;
    });

  if (result.length === 0) return;

  document.getElementById("info").innerText =
    "match: " + (result[indexOfResult].match * 100).toFixed(2) + "%";
  document.getElementById("result").innerHTML = result[indexOfResult].text
    .map((item) =>
      input.includes(item) ? createChip(item, true) : createChip(item)
    )
    .join(" ");

  const imgDOM = document.getElementById("img");
  const imgLDOM = document.getElementById("imgL");
  const imgRDOM = document.getElementById("imgR");

  imgDOM.src = "";
  imgDOM.src =
    result[indexOfResult]?.url.replace(
      "cdn.discordapp.com",
      "media.discordapp.net"
    ) ?? ""; // +"?width=700&height=700";

  imgRDOM.src =
    result[indexOfResult + 1]?.url
      .replace("cdn.discordapp.com", "media.discordapp.net")
      .concat("?width=128&height=128") ?? "";
  imgLDOM.src =
    result[indexOfResult - 1]?.url
      .replace("cdn.discordapp.com", "media.discordapp.net")
      .concat("?width=128&height=128") ?? "";
}

inputDOM.addEventListener("keypress", function (event) {
  if (event.key === "Enter") {
    event.preventDefault();
    newSearch();
  }
});

function createChip(text, bold = false) {
  return `<div class="chip ${
    bold ? "green lighten-1" : "orange lighten-2"
  }">${text}</div>`;
}

function init() {
  canvas = document.getElementById("customDrawCanvas");
  ctx = canvas.getContext("2d");
  ctx.imageSmoothingQuality = "low";

  canvas.addEventListener(
    "mousemove",
    function (e) {
      findxy("move", e);
    },
    false
  );
  canvas.addEventListener(
    "mousedown",
    function (e) {
      findxy("down", e);
    },
    false
  );
  canvas.addEventListener(
    "mouseup",
    function (e) {
      findxy("up", e);
    },
    false
  );
  canvas.addEventListener(
    "mouseout",
    function (e) {
      findxy("out", e);
    },
    false
  );
}

function findxy(res, e) {
  if (res == "down") {
    prevX = currX;
    prevY = currY;
    currX = e.clientX - canvas.offsetLeft;
    currY = e.clientY - canvas.offsetTop;

    draw(currX, currY);

    flag = true;
    dot_flag = false;
  }
  if (res == "up" || res == "out") {
    flag = false;
  }
  if (res == "move") {
    if (flag) {
      prevX = currX;
      prevY = currY;
      currX = e.clientX - canvas.offsetLeft;
      currY = e.clientY - canvas.offsetTop;
      draw(currX, currY);
    }
  }
}

function canvasScale(x, y) {
  return [
    (x * canvas.width) / canvas.offsetWidth,
    (y * canvas.height) / canvas.offsetHeight,
  ];
}

function draw(x, y) {
  [x, y] = canvasScale(x, y);
  ctx.fillStyle = fillStyle;
  ctx.fillRect(x, y, 1, 1);
}

function setColor(color) {
  document.getElementById("selectedColorDOM").style.background = color;
  fillStyle = color;
}
