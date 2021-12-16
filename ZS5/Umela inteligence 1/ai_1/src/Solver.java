import java.util.*;

import csp.*;

public class Solver {
    // Perform forward checking on any unchecked constraints in the given CSP.
    // Return a list of variables (if any) whose values were inferred.  If a contradiction
    // is found, return null.
    public List<Integer> forwardCheck(BooleanCSP csp) {
        List<Integer> toReturn = new ArrayList<Integer>();

        while(!csp.unchecked.isEmpty()){
            Constraint constraint = csp.unchecked.remove();
            int realCount = 0;
            List<Integer> missingVars = new ArrayList<Integer>();

            for (int var : constraint.vars) {
                if(csp.value[var] == null){
                    missingVars.add(var);    
                    continue;
                }
                realCount += csp.value[var] ? 1 : 0;
            }

            if(realCount > constraint.count)
                return null;

            if(realCount + missingVars.size() < constraint.count)
                return null;
            
            if(missingVars.size() == 0)
                continue;

            if(realCount == constraint.count){
                for(int missingVar : missingVars){
                    toReturn.add(missingVar);
                    csp.set(missingVar, false);
                }
                continue;
            }

            if(realCount + missingVars.size() == constraint.count){
                for(int missingVar : missingVars){
                    toReturn.add(missingVar);
                    csp.set(missingVar, true);
                }
                continue;
            }

        }

        return toReturn;
    }

    // Find a solution to the given CSP using backtracking.  The solution will not include
    // values for variables that do not belong to any constraints.
    // Return a list of variables whose values were inferred.  If no solution is found,
    // return null.
    public List<Integer> solve(BooleanCSP csp) {
        Boolean[] previousValue = csp.value.clone();
        List<Integer> toReturn = new ArrayList<Integer>();

        int valIndex;
        for(valIndex = 0; valIndex < csp.value.length; valIndex++)
            if(csp.value[valIndex] == null)
                break;
        
        if(valIndex == csp.value.length)
            return toReturn;

        for(int guessValue = 0; guessValue < 2; guessValue++){
            csp.set(valIndex, guessValue == 0 ? false : true);
            
            List<Integer> resultFCH = forwardCheck(csp);
            if(resultFCH == null){
                csp.value = previousValue;
                csp.unchecked.clear();
                continue;
            }

            List<Integer> resultCSP = solve(csp);
            if(resultCSP == null){
                csp.value = previousValue;
                csp.unchecked.clear();
                continue;
            }

            toReturn.addAll(resultFCH);
            toReturn.addAll(resultCSP);

            return toReturn;
        }

        return null;
    }

    // Infer a value for a single variable if possible using a proof by contradiction.
    // If any variable is inferred, return it; otherwise return -1.
    public int inferVar(BooleanCSP csp) {
        Boolean[] previousValue = csp.value.clone();
        List<Integer> resultFCH = forwardCheck(csp);



        return resultFCH.size() == 0 ? -1 : resultFCH.get(0);
    }
}
