package arch;

import jason.asSyntax.Literal;
import jason.asSemantics.Message;
import env.WorldModel;
import jason.asSyntax.NumberTerm;
import jason.environment.grid.Location;

public class PrvniRadaArch extends LocalMinerArch {
	static Boolean sentToEnemy = true;

	@Override
    public void broadcast(Message m) throws Exception {
        String basename = getAgName().substring(0,getAgName().length()-1);
        for (int i=1; i <= 6; i++) {
            String oname = basename+i;
			System.out.println("m: " + m.getPropCont());
            if (!getAgName().equals(oname)) {
                Message msg = new Message(m);
                msg.setReceiver(oname);
                sendMsg(msg);
            }
        }


		Location depotLocation = super.model.getDepot();
		int depotX = depotLocation.x;
		int depotY = depotLocation.y;
		final String[] names = {"dummy", "miner"};
		
		if(sentToEnemy){
			sentToEnemy = false;
			for (int name=0; name < names.length; name++) {
				for (int i=1; i <= 6; i++) {
					String oname = names[name]+""+i;
					Message msg = new Message(m);
					msg.setReceiver(oname);
					for (int x = -2; x <= 2; x++) {
						for (int y = -2; y <= 2; y++) {
							msg.setPropCont(Literal.parseLiteral("cell("+(x+depotX)+","+(y+depotY)+",obstacle)"));
							sendMsg(msg);
						}
					}
				}
			}
		}
	}
}
