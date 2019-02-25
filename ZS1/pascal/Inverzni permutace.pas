Program HelloWorld(output);
var a,b,c,n,i: integer;
var pole:array[1..100] of integer;
begin
    //writeln('1 4 2 6 5 3');
    
    read(n);
    for i:=1 to n do read(pole[i]);
    
    for a:=1 to n do
    begin
        for b:=1 to n do
        begin
            
            if(pole[b] = a)then
            write(b,' ');
            
        end;
    end;
    
    
    
    
    
    
    
    
end.