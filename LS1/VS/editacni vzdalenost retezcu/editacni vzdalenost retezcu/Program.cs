using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace editacni_vzdalenost_retezcu{
class Program{static void Main(string[]args){
string str1=Console.ReadLine(),str2=Console.ReadLine();int[,]pole=new int[str1.Length+1,str2.Length+1];for(int i=0;i<=str1.Length;i++){pole[i,0]=i;}
for(int i=0;i<=str2.Length;i++){pole[0,i]=i;}
for(int i=1;i<=str2.Length;i++){for(int j=1;j<=str1.Length;j++){int del=pole[j-1,i]+1;int plus=pole[j,i-1]+1;int repl=(str1[j-1]==str2[i-1])?0:1;int min=pole[j-1,i-1]+repl;pole[j,i]=Math.Min(del,Math.Min(plus,min));}}
Console.WriteLine(pole[str1.Length,str2.Length]);Console.ReadKey();}}}