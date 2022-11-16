Program kralCesta;
uses sysutils;
type
    pointer = ^Prvek;
    Prvek = record
            x: Integer;
            y: Integer;
            next: pointer;
        end;
        
var posledni,seznam: pointer;
    vh,i,j,k,l,x,y,n,kx,ky,cx,cy,sx,sy,tmp: Integer;
    sachovnice:array[1..8,1..8]of Integer;
    
procedure insert(var posledni:pointer;x,y:integer);
var novy:pointer;
begin
    new(novy);
    posledni^.next := novy;
    novy^.x := x;
    novy^.y := y;
    novy^.next := nil;
end;

procedure tah(x,y:integer);
var i,j:Integer;
begin
    if (x=cx)and(y=cy) then begin
        write(sachovnice[x,y]);
        seznam^.next:=nil;
    end;
    
    for i:=-1 to 1 do
        for j:=-1 to 1 do
            if((i<>0)or(j<>0))and(x+i<=8)and(x+i>=1)and(y+j<=8)and(y+j>=1)and
              (sachovnice[x+i,y+j]>sachovnice[x,y])then begin
                sachovnice[x+i,y+j] := sachovnice[x,y]+1;
                insert(posledni,x+i,y+j);
                posledni:=posledni^.next;
            end;
    
    
    

end;



begin    
    //writeln(TimeToStr(Time));
    new(seznam);
    posledni:=seznam;
    read(n);
    
    for i := 1 to 8 do begin
        for j := 1 to 8 do
            sachovnice[i,j]:=88;
    end;
    
    
    for i := 1 to n do begin 
        read(x);
        read(y);
        sachovnice[x,y]:=-1;
    end;
    
    read(sx);
    read(sy);
    read(cx);
    read(cy);
    
    sachovnice[sx,sy]:=0;
    
    
    {for i := 1 to 8 do begin
        for j := 1 to 8 do
            write(sachovnice[i,j]:3);
            writeln();
    end;
    }
    
    seznam^.x:=sx;
    seznam^.y:=sy;
    seznam^.next:=nil;
    
    while seznam <> nil do begin
        tah(seznam^.x,seznam^.y);
        seznam := seznam^.next;
    end;
    
    if (sachovnice[cx,cy] = 88) or (sachovnice[cx,cy] <= 0) then
        writeln(-1);
    
    {for i := 1 to 8 do begin
        for j := 1 to 8 do
            write(sachovnice[i,j]:3);
            writeln();
    end;
    }    
    
    
end.