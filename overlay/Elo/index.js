import { CountUp } from "./deps/countUp.min.js";

let socket = new ReconnectingWebSocket("ws://127.0.0.1:24601/json");

socket.onopen = () => console.log("Socket Connected!");

let killsEloDelta = document.getElementById("killsEloDelta");
let gamesEloDelta = document.getElementById("gamesEloDelta");

let killsEloCountUp = new CountUp("killsElo", 0, 0, 0, 0.5, {
  useEasing: true,
  useGrouping: true,
  separator: " ",
  decimal: ".",
});
let gamesEloCountUp = new CountUp("gamesElo", 0, 0, 0, 0.5, {
  useEasing: true,
  useGrouping: true,
  separator: " ",
  decimal: ".",
});
let killsEloDeltaCountUp = new CountUp("killsEloDelta", 0, 0, 0, 0.5, {
  useEasing: true,
  useGrouping: true,
  separator: " ",
  decimal: ".",
});
let gamesEloDeltaCountUp = new CountUp("gamesEloDelta", 0, 0, 0, 0.5, {
  useEasing: true,
  useGrouping: true,
  separator: " ",
  decimal: ".",
});

socket.onmessage = (event) => {
  let data = JSON.parse(event.data);

  if (data.playerStatsArray[data.localPlayerIndex].killsEloDelta < 0) {
    killsEloDelta.classList.remove("positive");
  } else {
    killsEloDelta.classList.add("positive");
  }

  if (data.playerStatsArray[data.localPlayerIndex].gamesEloDelta < 0) {
    gamesEloDelta.classList.remove("positive");
  } else {
    gamesEloDelta.classList.add("positive");
  }

  killsEloCountUp.update(data.playerStatsArray[data.localPlayerIndex].killsElo);

  killsEloDeltaCountUp.update(
    data.playerStatsArray[data.localPlayerIndex].killsEloDelta
  );

  gamesEloCountUp.update(data.playerStatsArray[data.localPlayerIndex].gamesElo);

  gamesEloDeltaCountUp.update(
    data.playerStatsArray[data.localPlayerIndex].gamesEloDelta
  );
};
