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

            if(realCount > constraint.count || realCount + missingVars.size() < constraint.count){
                for (Integer toReturnItem : toReturn) {
                    csp.value[toReturnItem] = null;
                    csp.unchecked.clear();
                }
                return null;
            }

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

        int valIndex = -1;
        int maxVal = 0;
        for(int i = 0; i < csp.value.length; i++)
            if(csp.value[i] == null && maxVal < csp.varConstraints.get(i).size()){
                valIndex = i;
                maxVal = csp.varConstraints.get(i).size();
            }
        
        
        if(valIndex == -1)
            return new ArrayList<Integer>();

        for(int guessValue = 0; guessValue < 2; guessValue++){
            csp.set(valIndex, guessValue == 0 ? false : true);
            
            List<Integer> resultFCH = forwardCheck(csp);
            if(resultFCH == null){
                csp.value = previousValue.clone();
                csp.unchecked.clear();
                continue;
            }

            List<Integer> resultCSP = solve(csp);
            if(resultCSP == null){
                csp.value = previousValue.clone();
                csp.unchecked.clear();
                continue;
            }

            resultFCH.addAll(resultCSP);
            resultFCH.add(valIndex);

            return resultFCH;
        }

        return null;
    }

    // Infer a value for a single variable if possible using a proof by contradiction.
    // If any variable is inferred, return it; otherwise return -1.
    public int inferVar(BooleanCSP csp) {
        for (int i = 0; i < csp.value.length; i++) {
            if(csp.value[i] != null)
                continue;

            csp.set(i, true);
            List<Integer> resultCSP = solve(csp);
            if(resultCSP == null){
                csp.set(i, false);
                return i;
            }


            for(int CSPItem : resultCSP)
                csp.value[CSPItem] = null;
            

            csp.set(i, false);
            resultCSP = solve(csp);
            if(resultCSP == null){
                csp.set(i, true);
                return i;
            }


            for(int CSPItem : resultCSP)
                csp.value[CSPItem] = null;
            csp.value[i] = null;
        }

        return -1;
    }
}
