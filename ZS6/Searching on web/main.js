function search() {
  const text = document.getElementById("input").value;
  const result = data.messages
    .map(({ content, attachments }) => ({
      text: content.slice(2, content.lastIndexOf("**")),
      url: attachments[0].url,
    }))
    .filter((item) => {
      return item.text.includes(text);
    });

  if (result.length > 0) {
    document.getElementById("result").innerText = result[0].text;
    document.getElementById("img").src = "";
    document.getElementById("img").src = result[0].url;
  } else {
    document.getElementById("result").innerText = "nothing found";
    document.getElementById("img").src = "";
  }
}
