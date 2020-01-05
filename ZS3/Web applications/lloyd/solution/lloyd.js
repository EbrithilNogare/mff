window.onload = ()=>{
	changeLook();
	const game = new lloydsGame();
	for(let i = 1; i < 16; i++)
		document.getElementById("game-tile-"+i).addEventListener("click", (e)=>{game.tileClicked(i-1)});
}


class lloydsGame{
	constructor(){
		this.GenerateNewGameState();
	}

	newGame(){
		this.GenerateNewGameState();
	}
	
	GenerateNewGameState(){
		this.gameState = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]; // init
		
		for(let i in this.gameState){ 				//mix
			const rnd = Math.floor(Math.random() * 16); 
			[this.gameState[i], this.gameState[rnd]] = [this.gameState[rnd], this.gameState[i]];
		}
		
		if(!this.isSolvable(this.gameState)) 		// check solvability
			[this.gameState[0], this.gameState[1]] = [this.gameState[1], this.gameState[0]];
		
		for(let i = 0; i < 15; i++) 				// draw
			this.setTilePosition(i);
	}

	isSolvable(puzzle){
		let parity = 0;

		for (let ii = 0; ii < puzzle.length; ii++)
			for (let jj = ii + 1; jj < puzzle.length; jj++)
				if (puzzle.indexOf(ii) > puzzle.indexOf(jj) && puzzle.indexOf(ii) != 15)
					parity++

		return !!(Math.floor(puzzle[15]/4) & 1 ^ parity & 1)
	}

	tileClicked(index){
		if(
			(this.gameState[index] == this.gameState[15]+1 && this.gameState[15] % 4 != 3) ||
			(this.gameState[index] == this.gameState[15]-1 && this.gameState[15] % 4 != 0) ||
			(this.gameState[index] == this.gameState[15]+4 && Math.floor(this.gameState[15] / 4) != 3) ||
			(this.gameState[index] == this.gameState[15]-4 && Math.floor(this.gameState[15] / 4) != 0)
		)
			this.swapTiles(index);
	}

	setTilePosition(index){
		document.getElementById("game-tile-"+(index+1)).style=`
			left: calc(20vmin * ${this.gameState[index]%4}); 
			top: calc(20vmin * ${Math.floor(this.gameState[index]/4)}); 
		`;
	}

	swapTiles(index){
		[this.gameState[index], this.gameState[15]] = [this.gameState[15], this.gameState[index]];
		this.setTilePosition(index);
		this.checkWin();
	}

	checkWin(){
		let win=true;
		for(let index in this.gameState)
			if(index != this.gameState[index])
				win=false;
		
		if(win)
			this.showWinMessage();
	}

	showWinMessage(){
		if(document.getElementsByClassName("winMessage").length!=0) return;
		const gameBoardDOM = document.getElementById("game-board");
		const lastTileDOM = document.createElement('div');

		lastTileDOM.className = "winMessage";
		lastTileDOM.appendChild(document.createTextNode("Win")); 
		lastTileDOM.addEventListener("click", (e)=>{e.target.parentNode.removeChild(e.target); this.newGame();});

		gameBoardDOM.appendChild(lastTileDOM);
	}

}


function changeLook(){
	const headDOM = document.getElementsByTagName("head")[0];
	const bodyDOM = document.getElementsByTagName("body")[0];
	const gameBoardDOM = document.getElementById("game-board");
	const newStyleDOM = document.createElement('style');
	const gameTiles = document.getElementsByClassName("game-tile");

	newStyleDOM.type = 'text/css';
	newStyleDOM.innerHTML = `
		.newGameTile { 
			width: calc(20vmin - 20px);
			height: calc(20vmin - 20px);
			position: absolute;
			border-radius: 0.5vmin;
			background-color: #111;
			font-size: 12vmin;
			text-align: center;
			font-family: cursive;
			color: #fff;
			text-shadow: 0 0 20px #d800ff;
			line-height: calc(20vmin - 20px);
			cursor: pointer;
			box-shadow: 5px 5px 5px 0px #000;	
			transition: margin 0.3s, width 0.3s, height 0.3s, box-shadow 0.3s, line-height 0.3s, font-size 0.3s, top 1s, left 1s;
		}
		.newGameTile:hover {
			margin: -9px;
			width: calc(20vmin - 2px);
			height: calc(20vmin - 2px);
			box-shadow: 5px 5px 22px 0px #000;	
			text-shadow: 0 0 20px #00e7ff;
			line-height: calc(20vmin - 2px);
			font-size: 13vmin;
		}
		.winMessage{
			position: absolute;
			top: 10vmin;
			left: 10vmin;
			width: 60vmin;
			height: 60vmin;

			border-radius: 0.5vmin;
			background-color: #050;
			font-size: 20vmin;
			text-align: center;
			color: #fff;
			line-height: 60vmin;
			cursor: pointer;
			box-shadow: 5px 5px 5px 0px #000;
		}
	`;
	headDOM.appendChild(newStyleDOM);

	while(gameTiles.length!=0)
		gameTiles[0].className = 'newGameTile';


	gameBoardDOM.style = "border: 0;";
	bodyDOM.style = `
		background: #222;
		color: #fff;
	`;
}
