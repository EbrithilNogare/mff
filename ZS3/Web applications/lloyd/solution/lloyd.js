onload = () => {
	const game = new lloydsGame();
	for(let i = 1; i < 16; i++)
		document.getElementById("game-tile-" + i).addEventListener("click", () => { game.tileClicked(i - 1) });
}

class lloydsGame {
	constructor() {
		this.GenerateNewGameState()
	}

	GenerateNewGameState() {
		this.gameState = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

		for (let i in this.gameState) {
			const rnd = Math.floor(Math.random() * 16);
			[this.gameState[i], this.gameState[rnd]] = [this.gameState[rnd], this.gameState[i]]
		}

		if (!this.isSolvable(this.gameState))
			[this.gameState[0], this.gameState[1]] = [this.gameState[1], this.gameState[0]]

		for (let i = 0; i < 15; i++)
			this.setTilePosition(i)
	}

	isSolvable(puzzle) {
		let parity = 0
		for (let ii = 0; ii < puzzle.length; ii++)
			for (let jj = ii + 1; jj < puzzle.length; jj++)
				if (puzzle.indexOf(ii) > puzzle.indexOf(jj) && puzzle.indexOf(ii) != 15)
					parity++

		return !!(Math.floor(puzzle[15] / 4) & 1 ^ parity & 1)
	}

	tileClicked(index) {
		if (
			(this.gameState[index] == this.gameState[15] + 1 && this.gameState[15] % 4 != 3) ||
			(this.gameState[index] == this.gameState[15] - 1 && this.gameState[15] % 4 != 0) ||
			(this.gameState[index] == this.gameState[15] + 4 && Math.floor(this.gameState[15] / 4) != 3) ||
			(this.gameState[index] == this.gameState[15] - 4 && Math.floor(this.gameState[15] / 4) != 0)
		)
			this.swapTiles(index)
	}

	setTilePosition(index) {
		const tileDOM = document.getElementById("game-tile-" + (index + 1))
		tileDOM.style.left = `calc(20vmin * ${this.gameState[index]%4})`
		tileDOM.style.top = `calc(20vmin * ${Math.floor(this.gameState[index]/4)})`
	}

	swapTiles(index) {
		[this.gameState[index], this.gameState[15]] = [this.gameState[15], this.gameState[index]]
		this.setTilePosition(index)
		this.checkWin()
	}

	checkWin() {
		for (let index in this.gameState)
			if (index != this.gameState[index])
				return;

		alert("you won, setting up new game")
		this.GenerateNewGameState()
	}
}
