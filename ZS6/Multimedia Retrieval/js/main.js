// expects variable data with json of midjourney chat export

const SEPARATOR_REGEXP = /[;, ]+/;

const inputDOM = document.getElementById("input");
let indexOfResult = 0;

if (data.messages) {
  data = data.messages.map(({ content, attachments }) => ({
    text: content
      .slice(2, content.lastIndexOf("**"))
      .split(SEPARATOR_REGEXP)
      .filter((item) => item[0] !== "<")
      .filter((item) => item[0] !== "-")
      .filter((item) => item[0] !== "—")
      .filter((item) => item.length < 32)
      .filter((item) => item !== "")
      .filter(onlyUnique),
    url: attachments[0]?.url,
  }));
  console.log("dataset convert done: ", data);
}

function onlyUnique(value, index, self) {
  return self.indexOf(value) === index;
}

function prev() {
  indexOfResult = Math.max(0, indexOfResult - 1);
  search();
}
function next() {
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
  imgDOM.src = result[indexOfResult].url.replace(
    "cdn.discordapp.com",
    "media.discordapp.net"
  ); // +"?width=700&height=700";

  imgRDOM.src =
    result[indexOfResult + 1].url.replace(
      "cdn.discordapp.com",
      "media.discordapp.net"
    ) + "?width=64&height=64";
  imgLDOM.src =
    result[indexOfResult - 1].url.replace(
      "cdn.discordapp.com",
      "media.discordapp.net"
    ) + "?width=64&height=64";
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
