package jia;

import java.util.Collections;
import java.util.List;
import java.util.ArrayList;
import jason.environment.grid.Location;
import jason.JasonException;
import jason.asSemantics.DefaultInternalAction;
import jason.asSemantics.TransitionSystem;
import jason.asSemantics.Unifier;
import jason.asSyntax.NumberTermImpl;
import jason.asSyntax.NumberTerm;
import jason.asSyntax.Term;

import env.WorldModel;
import arch.LocalWorldModel;
import arch.MinerArch;
import java.util.Random;
import java.util.Iterator;

public class prvniRadaSpecificPosition extends DefaultInternalAction {

    private Random random = new Random();

    @Override
    public Object execute(final TransitionSystem ts, final Unifier un, final Term[] args) throws Exception {
        try {
            final int maxIter = 3;
            LocalWorldModel model = ((MinerArch)ts.getUserAgArch()).getModel();
            //model.isFreeOfObstacle(x,y)
            int currentX = (int)((NumberTerm)args[2]).solve();
            int currentY = (int)((NumberTerm)args[3]).solve();
            
            int destinationX = (int)((NumberTerm)args[4]).solve();
            int destinationY = (int)((NumberTerm)args[5]).solve();

            //System.out.println("Agent at " + currentX + "_" + currentY);
            //for (int x = 0; x < 50; x++) {
            //    for (int y = 0; y < 50; y++) {
            //        //if(!model.inGrid(x,y)){
            //        //    continue;
            //        //} else if(model.hasObject(WorldModel.GOLD, x, y)) {
            //        //    System.out.print("G");
            //        //} else if(model.hasObject(WorldModel.OBSTACLE, x, y)) {
            //        //    System.out.print("O");
            //        //} else if(model.hasObject(WorldModel.ENEMY, x, y)) {
            //        //    System.out.print("E");
            //        //} else {
            //        //    System.out.print("_");
            //        //}
            //        System.out.print(model.getVisited(new Location(x,y))>0 ? "#" : "_");
            //    }
            //    System.out.println();
            //}
            //System.out.println("#####################################################");


            return new Iterator<Unifier>() {
                int i = 0;

                public boolean hasNext() {
                    return i < maxIter && ts.getUserAgArch().isRunning();
                }
                public Unifier next() {
                    Unifier c = un.clone();
                    Location l = model.getNearLeastVisited(currentX, currentY);
                    c.unifies(args[0], new NumberTermImpl(l.x));
                    c.unifies(args[1], new NumberTermImpl(l.y));
                    i++;
                    return c;
                }

                public void remove() {}
            };

        } catch (ArrayIndexOutOfBoundsException e) {
            throw new JasonException("The internal action 'random' has not received the required argument.");
        } catch (Exception e) {
            throw new JasonException("Error in internal action 'random': " + e, e);
        }
    }
}
