const fs = require("fs");
const csv = require("csv-parser");
const statistics = require("math-statistics");

const inputDirectory = "./results/astarGrid";
const outputFilePath = "./cross.csv";
const headerLinesToSkip = 8;

const columns = {
  0: "level",
  1: "win/fail",
  2: "% travelled",
  3: "run time",
  4: "game ticks",
  5: "planning time",
  6: "total plannings",
  7: "nodes evaluated",
  8: "most backtracked nodes",
};

const levelsRegex = /astarGrid-([^-]+)/;
const searchStepsRegex = /searchSteps-(\d+.\d+)/;
const timeToFinishRegex = /TTFW-(\d+.\d+)/;
const RIGHT_WINDOW_BORDER_XRegex = /RIGHT_WINDOW_BORDER_X-(\d+.\d+)/;

let data = [];

const files = fs.readdirSync(inputDirectory);

files.forEach((file) => {
  const filePath = `${inputDirectory}/${file}`;
  const levels = file.match(levelsRegex)[1];
  const searchSteps = parseFloat(file.match(searchStepsRegex)[1]);
  const timeToFinish = parseFloat(file.match(timeToFinishRegex)[1]);
  //  const RIGHT_WINDOW_BORDER_X = parseFloat(
  //    file.match(RIGHT_WINDOW_BORDER_XRegex)[1]
  //  );

  if (!fs.existsSync(filePath)) {
    console.error("cant read file: " + file);
  }

  let parseData = [];
  let parseData2 = [];

  fs.readFileSync(filePath, "utf-8")
    .split("\n")
    .slice(headerLinesToSkip)
    .forEach((line) => {
      const values = line.split(",");
      if (values.length !== Object.keys(columns).length) return;

      parseData.push(parseFloat(values[2]));
      parseData2.push(parseFloat(values[8]));
    });

  data.push([
    levels,
    searchSteps,
    timeToFinish,
    statistics.mean(parseData),
    statistics.mean(parseData2),
  ]);
});

const outputData = [
  [
    "level",
    "searchSteps",
    "timeToFinish",
    "avg travelled",
    "most backtracked nodes",
  ],
  ...data,
];

const csvContent = outputData.map((line) => line.join(",")).join("\n");

fs.writeFileSync(outputFilePath, csvContent);
