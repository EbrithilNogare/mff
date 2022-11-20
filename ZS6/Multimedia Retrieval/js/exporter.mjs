import * as fs from "fs";
import { createCanvas, loadImage } from "canvas";

const SEPARATOR_REGEXP = /[;, ]+/;
const fileNameToParse = process.argv[2];
const SAVE_EVERY_N_STEPS = 100;

//parseData();
//downloadImages();
checkDataIntegrity();
//concatenateJSONs();

function concatenateJSONs() {
  return; // code not needed anumore
  let datas = [];
  readFile(
    "C:/Users/nogare/Downloads/midjourney export/dataset (1).json",
    (err, data) => {
      data = JSON.parse(data);
      console.log("found: ", data.length);
      datas = [...datas, ...data];
      console.log("size of datas: ", datas.length);

      readFile(
        "C:/Users/nogare/Downloads/midjourney export/dataset (2).json",
        (err, data) => {
          data = JSON.parse(data);
          console.log("found: ", data.length);
          datas = [...datas, ...data];
          console.log("size of datas: ", datas.length);

          readFile(
            "C:/Users/nogare/Downloads/midjourney export/dataset (3).json",
            (err, data) => {
              data = JSON.parse(data);
              console.log("found: ", data.length);
              datas = [...datas, ...data];
              console.log("size of datas: ", datas.length);

              readFile(
                "C:/Users/nogare/Downloads/midjourney export/dataset (4).json",
                (err, data) => {
                  data = JSON.parse(data);
                  console.log("found: ", data.length);
                  datas = [...datas, ...data];
                  console.log("size of datas: ", datas.length);

                  readFile(
                    "C:/Users/nogare/Downloads/midjourney export/dataset (5).json",
                    (err, data) => {
                      data = JSON.parse(data);
                      console.log("found: ", data.length);
                      datas = [...datas, ...data];
                      console.log("size of datas: ", datas.length);

                      readFile(
                        "C:/Users/nogare/Downloads/midjourney export/dataset (6).json",
                        (err, data) => {
                          data = JSON.parse(data);
                          console.log("found: ", data.length);
                          datas = [...datas, ...data];
                          console.log("size of datas: ", datas.length);

                          readFile(
                            "C:/Users/nogare/Downloads/midjourney export/dataset (7).json",
                            (err, data) => {
                              data = JSON.parse(data);
                              console.log("found: ", data.length);
                              datas = [...datas, ...data];
                              console.log("size of datas: ", datas.length);

                              readFile(
                                "C:/Users/nogare/Downloads/midjourney export/dataset (8).json",
                                (err, data) => {
                                  data = JSON.parse(data);
                                  console.log("found: ", data.length);
                                  datas = [...datas, ...data];
                                  console.log("size of datas: ", datas.length);

                                  readFile(
                                    "C:/Users/nogare/Downloads/midjourney export/dataset (10).json",
                                    (err, data) => {
                                      data = JSON.parse(data);
                                      console.log("found: ", data.length);
                                      datas = [...datas, ...data];
                                      console.log(
                                        "size of datas: ",
                                        datas.length
                                      );

                                      readFile(
                                        "C:/Users/nogare/Downloads/midjourney export/dataset (11).json",
                                        (err, data) => {
                                          data = JSON.parse(data);
                                          console.log("found: ", data.length);
                                          datas = [...datas, ...data];
                                          console.log(
                                            "size of datas: ",
                                            datas.length
                                          );

                                          readFile(
                                            "C:/Users/nogare/Downloads/midjourney export/dataset (12).json",
                                            (err, data) => {
                                              data = JSON.parse(data);
                                              console.log(
                                                "found: ",
                                                data.length
                                              );
                                              datas = [...datas, ...data];
                                              console.log(
                                                "size of datas: ",
                                                datas.length
                                              );

                                              readFile(
                                                "C:/Users/nogare/Downloads/midjourney export/dataset (13).json",
                                                (err, data) => {
                                                  data = JSON.parse(data);
                                                  console.log(
                                                    "found: ",
                                                    data.length
                                                  );
                                                  datas = [...datas, ...data];
                                                  console.log(
                                                    "size of datas: ",
                                                    datas.length
                                                  );

                                                  readFile(
                                                    "C:/Users/nogare/Downloads/midjourney export/dataset (14).json",
                                                    (err, data) => {
                                                      data = JSON.parse(data);
                                                      console.log(
                                                        "found: ",
                                                        data.length
                                                      );
                                                      datas = [
                                                        ...datas,
                                                        ...data,
                                                      ];
                                                      console.log(
                                                        "size of datas: ",
                                                        datas.length
                                                      );

                                                      toFile(
                                                        datas,
                                                        fileNameToParse
                                                      );
                                                    }
                                                  );
                                                }
                                              );
                                            }
                                          );
                                        }
                                      );
                                    }
                                  );
                                }
                              );
                            }
                          );
                        }
                      );
                    }
                  );
                }
              );
            }
          );
        }
      );
    }
  );
}

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

function checkDataIntegrity() {
  readFile(fileNameToParse, (err, data) => {
    try {
      data = JSON.parse(data);
    } catch (err) {
      console.log("something went wrong");
      console.log(err.message);
      return;
    }

    console.log("found " + data.length + " records");
    let toReturn = data
      .filter((item) => item.text?.length > 0)
      .filter((item) => item.url?.length > 0)
      .filter((item) => item.smallImg?.length === 8 * 8 * 3);
    console.log("preserving " + toReturn.length + " records");
    //toFile(toReturn, fileNameToParse);
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
