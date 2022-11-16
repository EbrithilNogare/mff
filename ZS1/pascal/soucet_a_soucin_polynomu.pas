Program soucet_a_soucin_polynomu;
var in1, in3,i,j,vr: integer;
var in2, in4:array[1..20] of integer;
var vrp:array[1..40] of integer;

begin
read(in1);
for i:=20-in1 to 20 do read(in2[i]);
read(in3);
for i:=20-in3 to 20 do read(in4[i]);

writeln(in1 + 1);
for i:=1 to 20 do begin
    vr := in4[i]+in2[i];
    if((vr > 0)) then
        write(vr,' ');
end;

writeln();
writeln(in3 + 1);

for i:=20-in1 to 20 do begin
    for j:=20-in3 to 20 do begin
        vrp[i+j]:=vrp[i+j]+in2[i]*in4[j];
    end;
end;

for i:=1 to 40 do begin
    if((vrp[i] > 0))then
        write(vrp[i],' ');
end;
end.