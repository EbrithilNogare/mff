window.onload = ()=>{
	const game = new lloydsGame();
	game.create();
}



class lloydsGame{
	lloydsGame(){}

	create(){
		this.gameState = this.GenerateNewGameState();
	}
	
	GenerateNewGameState(){
		return[
			[ 3,  11,   2,   5],
			[ 1,  13,   6,   8],
			[ 4,   9,   0,  10],
			[14,  12,   7,  15]
		];
	}


}