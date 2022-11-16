program vyskyty;
var i,n,max,index, vysledek1: integer;
var pole:array[1..1000] of integer;
begin
    readln(n);
    for i:=1 to n do read(pole[i]);
    max:=pole[1];
    
    for i:=1 to n do
    begin
        if pole[i]>max then
        begin
            max := pole[i];
        end;
    end;
    writeln(max);
    
    for i:=1 to n do
    begin
        if pole[i]=max then
        begin
            write(i,' ');
        end;
    end;

end.