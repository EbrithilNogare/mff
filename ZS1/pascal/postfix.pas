Program Postfix;
//-----------------------------------Uses---------------------------------------
Uses sysutils;
//-----------------------------------const--------------------------------------
const debug = true;
//-----------------------------------type---------------------------------------
type
    pointer = ^Prvek;
    Prvek = record
            hodnota: longint;
            next: pointer;
        end;
type
    pointerS = ^PrvekS;
    PrvekS = record
            hodnota: String;
            next: pointerS;
        end;
        
var seznam: pointer;
    zadani,zadaniZ: pointerS;
    i,tt,td,err,voidI: longint;
    text,vstup: string;
    c:char;
//-----------------------------------procedure----------------------------------
procedure insertNumber(var seznam:pointer;data:longint);
var novy:pointer;
begin
    new(novy);
    novy^.next := seznam^.next;
    seznam^.next := novy;
    novy^.hodnota := data;
end;


procedure insertText(var zadani:pointerS;data:string);
var novy,aktualni:pointerS;
begin
    new(novy);
    aktualni:=zadani;
    while aktualni^.next <> nil do
        aktualni:=aktualni^.next;
    aktualni^.next := novy;
    novy^.next := nil;
    novy^.hodnota := data;
end;
//-----------------------------------Main---------------------------------------
begin
    if debug then writeln('cas spusteni: ',TimeToStr(Time));
    new(seznam);
    new(zadani);
    zadaniZ:=zadani^.next;
    
//-----------------------------------cteni zadani-------------------------------
    readln(vstup);
    text:=vstup;
    readln(vstup);
    while vstup <> '' do begin
        text := concat(text,' ',vstup);
        readln(vstup);
    end;
        if debug then writeln('zadane zadani: |',text,'|');
//-----------------------------------kontroly bordelu atd.----------------------
    
    for i:=1 to Length(text) do
        if not (text[i] in ['0','1','2','3','4','5','6','7','8','9','/','*','-','+'])then begin
            text[i]:= ' ';
            if debug then writeln('oprava nevalidnich znaku: |',text,'|');
        end;
    
    
    for i:=1 to Length(text) do
        if ((text[i] in ['0','1','2','3','4','5','6','7','8','9'])and
          (i+1<=length(text))and
          (text[i+1] in ['+','-','/','*'])) then begin
            Insert(' ',text,i+1);
            if debug then writeln('oprava nalepenych znamenek: |',text,'|');
        end;
        
    for i:=1 to Length(text) do
        if ((text[i] in ['+','-','/','*'])and
          (i+1<=length(text))and
          (text[i+1] in ['0','1','2','3','4','5','6','7','8','9'])) then begin
            Insert(' ',text,i+1);
            if debug then writeln('oprava nalepenych znamenek: |',text,'|');
        end;
    
    td:=1;
    tt:=Pos('  ',text);
    while tt > 0 do begin
        delete (text,tt,1);
        td:=1;
        tt:=Pos('  ',text);
        if debug then writeln('oprava duplicitnich mezer: |',text,'|');
    end;
    
    while not (text[Length(text)] in ['0','1','2','3','4','5','6','7','8','9','/','*','-','+']) do
        delete (text,Length(text),1);
        
        
//-----------------------------------vlozeni do spojoveho seznamu---------------
    
    td:=1;
    tt:=Pos(' ',text);
    while tt > 0 do begin
        if copy(text,td,tt-1) <> '' then
            insertText(zadani,copy(text,td,tt-1));
        delete (text,td,tt-td+1);
        td:=1;
        tt:=Pos(' ',text);
    end;
    insertText(zadani,text);
    
    zadaniZ:=zadani^.next;
    
    if debug then write('celkovy vyraz: ');
    
    while zadaniZ <> nil do begin
        if debug then write(zadaniZ^.hodnota,' ');
        zadaniZ := zadaniZ^.next;
    end;
    if debug then writeln();
    
//-----------------------------------pocitani-----------------------------------
    
    repeat
        zadani := zadani^.next;
        if debug then writeln('ctu zadani: ',zadani^.hodnota);
        val(zadani^.hodnota,voidI,err);
        if  err = 0 then
            insertNumber(seznam,StrToInt(zadani^.hodnota))
        else begin
            if debug then write('pocitam: ',seznam^.next^.next^.hodnota,zadani^.hodnota,seznam^.next^.hodnota);
            if seznam^.next^.next = nil then begin
                writeln('Chyba!');
                exit();
            end;
            
            case zadani^.hodnota of
                '+': seznam^.next^.next^.hodnota := seznam^.next^.hodnota + seznam^.next^.next^.hodnota;
                '-': seznam^.next^.next^.hodnota := seznam^.next^.next^.hodnota - seznam^.next^.hodnota;
                '*': seznam^.next^.next^.hodnota := seznam^.next^.hodnota * seznam^.next^.next^.hodnota;
                '/': 
                        if seznam^.next^.hodnota <> 0 then begin
                            seznam^.next^.next^.hodnota := seznam^.next^.next^.hodnota div seznam^.next^.hodnota;
                        end
                        else begin
                            writeln('Chyba!');
                            exit();
                        end;
                    
            end;
            if debug then writeln('=',seznam^.next^.next^.hodnota);
            seznam := seznam^.next;
        end;
            
        
        
    until  zadani^.next = nil;
    
    
//-----------------------------------zobrazeni vysledku---------------------------------------
    
    
    if seznam^.next^.next = nil then begin
        writeln(seznam^.next^.hodnota); end
    else
        writeln('Chyba!');
    
    
end.