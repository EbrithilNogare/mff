// expects variable data with json of midjourney chat export

const SEPARATOR_REGEXP = /[;, ]+/;

const inputDOM = document.getElementById("input");
let indexOfResult = 0;

// data = {
//     text: [string],
//     url: string,
//     smallImg: [8*8*3=192, int]
// }

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
  dot_flag = false,
  findByImg = false;

init();

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

    const inputPicture = findByImg
        ? Object.values(
            ctx.getImageData(0, 0, canvas.width, canvas.height).data
        ).filter((item, index) => index % 4 !== 3)
        : [];

    const synonyms = [];

    input.forEach((word) => {
        fetch('https://api.dictionaryapi.dev/api/v2/entries/en/' + word)
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
                return Promise.reject(response);
            })
            .then((syndata) => {
                parseSynonyms(syndata, synonyms)
            })
            .finally(function () {
                const result = data
                    .map((item) => {
                        let foundScore = 0;
                        input.forEach((word) => {
                            item.text.includes(word) ? (foundScore += 1) : null;
                        });
                        foundScore /= Math.max(Math.max(input.length, item.text.length), 1);

                        let synonymScore = 0;
                        synonyms.forEach((synonym) => {
                            item.text.includes(synonym) ? (synonymScore += 1) : null;
                        });
                        synonymScore /= Math.max(Math.max(input.length, item.text.length), 1);
                        foundScore = (foundScore + synonymScore);

                        if (findByImg) {
                            imgScore = item.smallImg
                                .map((a, i) => 1 - Math.sqrt(Math.abs(a - inputPicture[i])) / 16)
                                .reduce((b, c) => b + c, 0);
                            imgScore /= inputPicture.length;
                            foundScore = (foundScore + imgScore) / 2;
                        }

                        return {
                            ...item,
                            match: foundScore,
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
                document.getElementById("synonyms").innerHTML =
                    "synonyms: " + synonyms
                        .map((item) =>
                            result[indexOfResult].text.includes(item) ? createChip(item, true, true) : createChip(item, false, true)
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
            });
    });
}

inputDOM.addEventListener("keypress", function (event) {
  if (event.key === "Enter") {
    event.preventDefault();
    newSearch();
  }
});

function parseSynonyms(syndata, synonyms) {
    syndata.forEach((result) => { // slo by nahradit .reduce, nebo nejakou obyc flattern funkci...
        for (key in result) {
            if (key == 'meanings') {
                result[key].forEach((meaning) => {
                    for (var i = 0; i < meaning.synonyms.length; i++) {
                        if (synonyms.indexOf(meaning.synonyms[i]) == -1) synonyms.push(meaning.synonyms[i]); // aby nebyly duplicity
                    }
                })
            }
        }
    })
}

function createChip(text, bold = false, syn = false) {
  return `<div class="chip ${
    bold ? "green lighten-1" : "orange lighten-2"
      }">${text}
${syn ? '</div>' : '<i class="close material-icons">close</i></div>'}`;
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
    currX = e.pageX - canvas.offsetLeft;
    currY = e.pageY - canvas.offsetTop;

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
      currX = e.pageX - canvas.offsetLeft;
      currY = e.pageY - canvas.offsetTop;
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
