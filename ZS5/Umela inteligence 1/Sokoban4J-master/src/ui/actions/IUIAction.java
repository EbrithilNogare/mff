package ui.actions;

import ui.utils.TimeDelta;

public interface IUIAction {
	
	public void start();
	
	public void tick(TimeDelta time);
	
	public boolean isFinished();
	
	public void finish();
	

}
