package controllers.ghosts;

import controllers.EntityAction;

public final class GhostAction extends EntityAction {
	
	public GhostAction clone() {
		GhostAction result = new GhostAction();
		
		result.direction = direction;
		
		return result;
	}
		
}
