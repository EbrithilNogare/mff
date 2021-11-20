import java.lang.reflect.Constructor;
import java.util.*;

import javax.print.event.PrintEvent;
import javax.security.auth.x500.X500Principal;

import engine.AgentBase;
import game.*;
import game.move.*;
import minimax.HeuristicGame;
import agents.Napoleon;

public class MyAgent extends AgentBase
{
    Random random = new Random(0);
    Napoleon napoleon = new Napoleon();
    Minimax<Game, WarlightAction> minimax;
    WarlightProblem problem;
    WarlightAction nextAction;
    
    @Override
    public void init(long timeoutMillis) {
        problem = new WarlightProblem();
        minimax = new Minimax<Game, WarlightAction>(problem, 1); // not actual depth, its dynamically changing
    }

    @Override
    public Region chooseRegion(Game game) {
        ArrayList<Region> choosable = game.getPickableRegions();
        return choosable.get(random.nextInt(choosable.size()));
    }

    @Override
    public List<PlaceArmies> placeArmies(Game state) {
        if(state.getRoundNumber() > 20 && state.getRoundNumber() < 90)
            minimax.mainLimit = 4;
        else
            minimax.mainLimit = 7;

        problem.me = state.currentPlayer();
        this.nextAction = minimax.action(state);

        return this.nextAction.placeArmies;
    }
    
    @Override
    public List<AttackTransfer> attackTransfer(Game state) {
        return this.nextAction.attackTransfer;   
    }
}

class WarlightProblem implements HeuristicGame<Game, WarlightAction>{
    Napoleon napoleon;
    public int me;

    WarlightProblem(){
        napoleon = new Napoleon();
    }

    @Override
    public Game initialState(int seed) {
        return null;
    }

    @Override
    public Game clone(Game state) {
        return state.clone();
    }

    @Override
    public int player(Game state) {
        return state.currentPlayer() == this.me ? 1 : 2;
    }

    @Override
    public List<WarlightAction> actions(Game state) {
        List<WarlightAction>toReturn = new ArrayList<WarlightAction>();

        if(state.currentPlayer() == this.me){
            List<Integer> alreadyAttacked = new ArrayList<>();
            for(Region region : state.regionsOwnedBy(state.currentPlayer())){
                if(state.getOwner(region) != this.me)
                    continue;
                for(Region neighbor : region.getNeighbors()){
                    if(alreadyAttacked.contains(neighbor.id) || state.getOwner(neighbor) == this.me)
                        continue;
                    alreadyAttacked.add(neighbor.id);
                }
            }

            for(Integer regionToAttack : alreadyAttacked){
                List<PlaceArmies> placeArmies = new ArrayList<>();
                List<AttackTransfer> attackTransfer = new ArrayList<>();
                for(Region regionToAttackNeighbor : state.getRegion(regionToAttack).getNeighbors()) {
                    if(state.getOwner(regionToAttackNeighbor) != state.currentPlayer())
                        continue;
                    if(attackTransfer.size() == 0){
                        placeArmies.add(new PlaceArmies(regionToAttackNeighbor, state.armiesPerTurn(state.currentPlayer())));
                        attackTransfer.add(new AttackTransfer(regionToAttackNeighbor, state.getRegion(regionToAttack), state.getArmies(regionToAttackNeighbor)-1 + state.armiesPerTurn(state.currentPlayer())));
                    } else {
                        if(state.getArmies(regionToAttackNeighbor)-1 > 0)
                            attackTransfer.add(new AttackTransfer(regionToAttackNeighbor, state.getRegion(regionToAttack), state.getArmies(regionToAttackNeighbor)-1));
                    }
                    
                    toReturn.add(
                        new WarlightAction(
                            placeArmies,
                            attackTransfer
                        )
                    );
                }
            }
        } 
        else
        toReturn.add(
            new WarlightAction(
                napoleon.placeArmies(state),
                napoleon.attackTransfer(state)
            )
        );

        return toReturn;
    }

    @Override
    public void apply(Game state, WarlightAction action) {
        state.placeArmies(action.placeArmies);
        state.attackTransfer(action.attackTransfer, true);
    }

    @Override
    public boolean isDone(Game state) {
        return state.isDone();
    }

    @Override
    public double outcome(Game state) {
        if(this.me == state.winningPlayer())
            return 1000000;
        else
            return -1000000;
    }

    @Override
    public double evaluate(Game state) {
        double score = 0;
        score += state.armiesPerTurn(this.me) - state.armiesPerTurn(this.me == 1 ? 2 : 1);
        score *= 10;
        score += state.numberRegionsOwned(this.me) - state.numberRegionsOwned(this.me == 1 ? 2 : 1);
        score *= 100;
        score += state.numberArmiesOwned(this.me) - state.numberArmiesOwned(this.me == 1 ? 2 : 1);
        return score;
    }
}

class WarlightAction{
    List<PlaceArmies> placeArmies;
    List<AttackTransfer> attackTransfer;
    WarlightAction(List<PlaceArmies> placeArmies, List<AttackTransfer> attackTransfer){
        this.placeArmies = placeArmies;
        this.attackTransfer = attackTransfer;
    }
}