import "./in.js";
import * as fs from "fs";

readFile("in.json",(data)=>{
  data = JSON.parse(data);
  if (data.messages) {
    toReturn = data.messages.map(({ content, attachments }) => ({
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
      smallImg: [],
    }));
    await startSmallImgGetter(toReturn);
    toFile(toReturn, "out.js");
  }
})

async function startSmallImgGetter(data) {
  const imgSize = 8;
  const canvas = document.createElement("canvas");
  canvas.width = imgSize;
  canvas.height = imgSize;
  for (let i = 0; i < data.length; i++) {
    const element = data[i];
    element.smallImg = await getSmallImg(canvas, element.url, data);
    console.log(`smallImg progress: ${i}/${data.length}`);
  }
}

function getSmallImg(canvas, url, data) {
  return new Promise((resolve, reject) => {
    let img = new window.Image();
    img.crossOrigin = "*";
    img.onload = () => {
      const context = canvas.getContext("2d");
      context.drawImage(img, 0, 0, canvas.width, canvas.height);
      img = null;
      resolve(
        context
          .getImageData(0, 0, canvas.width, canvas.height)
          .data.filter((item, index) => index % 4 !== 3)
      );
    };
    img.onerror = () => {
      reject([]);
    };
    img.src =
      url.replace("cdn.discordapp.com", "media.discordapp.net") +
      `?width=${canvas.width}&height=${canvas.height}`;
  });
}

function toFile(data, fileName) {
  if (data.length === 0) return;

  fs.writeFile(`./${fileName}`, "data = " + JSON.stringify(data), (err) => {
    if (err) {
      console.error(err);
    }
  });
}

function readFile(filename, fallback) {
  fs.readFile(filename, "utf8", fallback);
}
