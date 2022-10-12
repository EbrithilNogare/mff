// expects variable data with json of midjourney chat export

data = data.messages.map(({ content, attachments }) => ({
  text: content.slice(2, content.lastIndexOf("**")),
  url: attachments[0].url,
}));

function search() {
  const text = document.getElementById("input").value;
  const result = data.filter((item) => {
    return item.text.includes(text);
  });

  document.getElementById("info").innerText = "found: " + result.length;
  if (result.length > 0) {
    document.getElementById("result").innerText = result[0].text;
    document.getElementById("img").src = "";
    document.getElementById("img").src = result[0].url;
  } else {
    document.getElementById("result").innerText = "nothing found";
    document.getElementById("img").src = "";
  }
}
