// expects variable data with json of midjourney chat export

const SEPARATOR_REGEXP = /[;, ]+/;

const inputDOM = document.getElementById("input");

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
    })
    .slice(0, 100);

  console.log("result: ", result);

  if (result.length === 0) return;

  document.getElementById("info").innerText =
    "match: " + (result[0].match * 100).toFixed(2) + "%";
  document.getElementById("result").innerHTML = result[0].text
    .map((item) => (input.includes(item) ? `<b>${item}</b>` : item))
    .join(", ");

  const imgDOM = document.getElementById("img");

  imgDOM.src = "";
  imgDOM.src =
    result[0].url.replace("cdn.discordapp.com", "media.discordapp.net") +
    "?width=700&height=700";
}

inputDOM.addEventListener("keypress", function (event) {
  if (event.key === "Enter") {
    event.preventDefault();
    search();
  }
});
