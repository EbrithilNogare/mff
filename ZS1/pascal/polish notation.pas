Program Polish;
Uses sysutils;
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
        
var aktualni,seznam: pointer;
    zadani: pointerS;
    vh,i,j,tt,td,err,voidI: longint;
    text: string;
    
procedure insertNumber(var seznam:pointer;data:longint);
var novy:pointer;
begin
    new(novy);
    novy^.next := seznam^.next;
    seznam^.next := novy;
    novy^.hodnota := data;
end;

procedure insertText(var zadani:pointerS;data:string);
var novy:pointerS;
begin
    new(novy);
    novy^.next := zadani^.next;
    zadani^.next := novy;
    novy^.hodnota := data;
end;

begin
    new(seznam);
    new(zadani);
    read(text);
    
    td:=1;
    tt:=Pos(' ',text);
    while tt > 0 do begin
        //write(copy(text,td,tt-1),'|');
        insertText(zadani,copy(text,td,tt-1));
        delete (text,td,tt-td+1);
        
        td:=1;
        tt:=Pos(' ',text);
    end;
    insertText(zadani,text);
    
    
    repeat
        zadani := zadani^.next;
        //writeln('zadani: ',zadani^.hodnota);
        val(zadani^.hodnota,voidI,err);
        if  err = 0 then
            insertNumber(seznam,StrToInt(zadani^.hodnota))
        else begin
            //writeln('pocitam: ',seznam^.next^.next^.hodnota,zadani^.hodnota,seznam^.next^.hodnota);
            if seznam^.next^.next = nil then begin
                writeln('CHYBA');
                exit();
            end;
            
            case zadani^.hodnota of
                '+': seznam^.next^.next^.hodnota := seznam^.next^.hodnota + seznam^.next^.next^.hodnota;
                '-': seznam^.next^.next^.hodnota := seznam^.next^.hodnota - seznam^.next^.next^.hodnota;
                '*': seznam^.next^.next^.hodnota := seznam^.next^.hodnota * seznam^.next^.next^.hodnota;
                '/': 
                        if seznam^.next^.next^.hodnota <> 0 then begin
                            seznam^.next^.next^.hodnota := seznam^.next^.hodnota div seznam^.next^.next^.hodnota;
                        end
                        else begin
                            writeln('CHYBA');
                            exit();
                        end;
                    
            end;
            //writeln('mezivysledek: ',seznam^.next^.next^.hodnota);
            seznam := seznam^.next;
        end;
            
        
        
    until  zadani^.next = nil;
    
    writeln(seznam^.next^.hodnota);
    
end.