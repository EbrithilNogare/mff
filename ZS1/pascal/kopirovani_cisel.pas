Program SpojSeznam;
type pointer = ^ Prvek;
    Prvek = record hodnota: longint;
    next: pointer;
end;
var aktualni, seznam, konec: pointer;
    i: byte;
    input: longint;
procedure insert(var konec: pointer; data: longint);
var aktualni, novy: pointer;
begin
    new(novy);
    aktualni := konec;
    aktualni^.next := novy;
    novy^.hodnota := data;
    novy^.next := nil;
    konec:=novy;
end;
procedure delete(var seznam: pointer; data: longint);
begin
    aktualni := seznam;
    while aktualni^.next^.hodnota <> data do begin
        aktualni := aktualni ^ .next;
    end;
    aktualni^ .next := aktualni^.next^.next;
end;

begin
    new(konec);
    seznam := konec;
    readln(input);
    while input <> 0  do begin
        insert(konec, input);
        readln(input);
    end;
    
    for i:=0 to 1 do begin
        aktualni := seznam^.next;
        while (aktualni <> nil) do begin
            writeln(aktualni^.hodnota);
        aktualni := aktualni^.next;
        end;
    end;
end.