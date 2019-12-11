function maxFreeRect(width, height, rects) {
	let rectTree = new rectNode(0, 0, width, height)

	for (rect of rects) {
		rectTree.addObstacle({
			beginX: rect.left,
			beginY: rect.top,
			endX: rect.left + rect.width,
			endY: rect.top + rect.height,
		})
	}

	const toReturn = rectTree.getLargestRect()
	return {
		left: toReturn.beginX,
		top: toReturn.beginY,
		width: toReturn.endX - toReturn.beginX,
		height: toReturn.endY - toReturn.beginY
	}

}

// In nodejs, this is the way how export is performed.
// In browser, module has to be a global varibale object.
module.exports = {
	maxFreeRect
}


class rectNode {
	constructor(beginX, beginY, endX, endY) {
		this.beginX = beginX
		this.beginY = beginY
		this.endX = endX
		this.endY = endY
		this.childs = []
		this.size = (this.endX - this.beginX) * (this.endY - this.beginY)
	}

	addObstacle(obstacle) {
		if (!this.rectangleOverlaping(obstacle))
			return;

		if (this.childs.length == 0) {
			// create up to 4 childs
			// top
			if (this.beginY < obstacle.beginY)
				this.childs.push(new rectNode(this.beginX, this.beginY, this.endX, obstacle.beginY))

			// left
			if (this.beginX < obstacle.beginX)
				this.childs.push(new rectNode(this.beginX, this.beginY, obstacle.beginX, this.endY))

			// right
			if (this.endX > obstacle.endX)
				this.childs.push(new rectNode(obstacle.endX, this.beginY, this.endX, this.endY))

			// bottom
			if (this.endY > obstacle.endY)
				this.childs.push(new rectNode(this.beginX, obstacle.endY, this.endX, this.endY))


		} else {
			for (let child of this.childs) {
				child.addObstacle(obstacle)
			}
		}
	}

	getLargestRect() {
		if (this.childs.length == 0)
			return this

		let maxSize = 0
		let largestChildNode
		for (let child of this.childs) {
			const largestChild = child.getLargestRect()
			if (largestChild.size > maxSize) {
				maxSize = largestChild.size
				largestChildNode = largestChild
			}
		}
		return largestChildNode
	}

	/**
	 * return true if they overlap
	 */
	rectangleOverlaping(secondRectangle) {
		return !(
			this.beginX > secondRectangle.endX ||
			this.endX < secondRectangle.beginX ||
			this.beginY > secondRectangle.endY ||
			this.endY < secondRectangle.beginY
		)
	}
}