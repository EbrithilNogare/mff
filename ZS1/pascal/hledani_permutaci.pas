program hledani_permutaci;
Uses math;
var n,k,i,j,l,temp,permutace: longint;
var pole:array[1..13] of longint;
var setrizeno:array[1..13] of longint;
var fak:array[0..12] of longint;
begin
    read(n);
    read(k);
    for i:=1 to n do read(pole[i]);
    for i:=1 to n do setrizeno[i] := i;
    fak[0]:=1;
    for i:=1 to n-1 do fak[i] := fak[i-1]*i;
    permutace:=1;
    
    for i:=1 to n do
    begin
        temp := 0;
        for j:=i downto 1 do
        begin
            if pole[i] >= pole[j] then
                temp := temp + 1;
            
        end;
        permutace := permutace + (pole[i]-temp) * (fak[n-i]);
    end;
    
    permutace := permutace + k-1;
    
    for i := 1 to n do
    begin
        j := Floor(permutace / fak[n - i])+1;
        permutace := permutace mod fak[n - i];
        write(setrizeno[j],' ');
        for l := j to n-1 do 
        begin
            setrizeno[l] := setrizeno[l+1];       
        end; 
    end;
end.