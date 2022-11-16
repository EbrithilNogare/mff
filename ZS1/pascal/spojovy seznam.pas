Program SpojSeznam;
type
    pointer = ^Prvek;
    Prvek = record
            hodnota: Integer;
            next: pointer;
        end;
        
var aktualni,seznam: pointer;
    vh: Integer;
    
procedure insert(var seznam:pointer;data:integer);
var aktualni,novy:pointer;
begin
    new(novy);
    aktualni := seznam;
    while aktualni^.next <> nil do
    begin
        aktualni := aktualni^.next;
    end;
    aktualni^.next := novy;
    novy^.hodnota := data;
    novy^.next := nil;
end;

procedure delete(var seznam:pointer;data:integer);
begin
    aktualni := seznam;
    while aktualni^.next^.hodnota <> data do
    begin
        aktualni := aktualni^.next;
    end;
    aktualni^.next := aktualni^.next^.next;
end;

    
begin
    new(seznam);
    for vh := 1 to 11 do begin
        insert(seznam,vh);
    end;
        delete(seznam,10);
    
    aktualni := seznam;
    aktualni := aktualni^.next;
    while (aktualni <> nil) do begin
        write(aktualni^.hodnota, ' ');
        aktualni := aktualni^.next;
    end;
end.