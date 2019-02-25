program BubbleSort;
var pocet,j,k,pomocne:longint;
var CISLA: array [1..20] of longint;
procedure BublinkoveTrideni;begin
    for j:=1 to pocet-1 do begin
        for k:=1 to pocet-j do begin
            if CISLA[k]>CISLA[k+1] then begin
                pomocne:=CISLA[k+1];
                CISLA[k+1]:=CISLA[k];
                CISLA[k]:=pomocne;
                writeln('Prohazuji prvky ',CISLA[k],' a ',CISLA[k+1],'.');
            end;
        end;
    end;
end;
begin
    pocet:= 0;
    read (j);
    while j>=0 do begin
        pocet:=pocet+1;
        CISLA[pocet]:=j;
        read(j);
    end;
    BublinkoveTrideni;
    for j:=1 to pocet do write(CISLA[j],' ');
end.