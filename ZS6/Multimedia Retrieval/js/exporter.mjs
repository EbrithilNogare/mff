import * as fs from "fs";
import { createCanvas, loadImage } from "canvas";

const SEPARATOR_REGEXP = /[;, ]+/;
const fileNameToParse = process.argv[2];
const SAVE_EVERY_N_STEPS = 100;

parseData();
downloadImages();

function parseData() {
  readFile(fileNameToParse, (err, data) => {
    data = JSON.parse(data);
    if (!data.messages) {
      return;
    }
    const toReturn = data.messages.map(({ content, attachments }) => ({
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
      smallImg: null,
    }));
    toFile(toReturn, fileNameToParse);
  });
}

function onlyUnique(value, index, self) {
  return self.indexOf(value) === index;
}

function downloadImages() {
  readFile(fileNameToParse, async (err, data) => {
    data = JSON.parse(data);
    if (data.messages) {
      return;
    }

    const imgSize = 8;
    const canvas = createCanvas(imgSize, imgSize);

    for (let i = 0; i < data.length; i++) {
      console.log(`smallImg progress: ${i}/${data.length}`);
      if (data[i].smallImg != null) {
        continue;
      }
      if (!data[i].url) {
        continue;
      }
      try {
        const element = data[i];
        element.smallImg = await getSmallImg(canvas, element.url, data);
        if (i % SAVE_EVERY_N_STEPS === 0) {
          toFile(data, fileNameToParse);
        }
      } catch (err) {
        console.log("error in img", i);
        continue;
      }
    }
    toFile(data, fileNameToParse);
  });
}

async function getSmallImg(canvas, url, data) {
  const newUrl =
    url.replace("cdn.discordapp.com", "media.discordapp.net") +
    `?width=${canvas.width}&height=${canvas.height}`;
  const imgData = await loadImage(newUrl).then((image) => {
    const context = canvas.getContext("2d");
    context.drawImage(image, 0, 0, canvas.width, canvas.height);
    return Object.values(
      context.getImageData(0, 0, canvas.width, canvas.height).data
    ).filter((item, index) => index % 4 !== 3);
  });
  return imgData;
}

function toFile(data, fileName) {
  if (data.length === 0) return;

  fs.writeFile(`${fileName}`, JSON.stringify(data), (err) => {
    if (err) {
      console.error(err);
    }
  });
}

function readFile(filename, fallback) {
  fs.readFile(filename, "utf8", fallback);
}
