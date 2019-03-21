Výsledky benchmarkù pro poèítání ve Fixed notaci
================================================

Porovnávaly se tyto datové typy:
* Q24_8
* Q16_16
* Q8_24
* Float
* Double

Porovnávali se tyto operace:
* Sèítání
* Odèítání
* Násobení
* Dìlení

Benchmarky byly spuštìny s defaultním configem (tedy bez žádného)
Platformou byl Intel s hardwarovou podporou dìlení a desetiných èísel

Tabulka výsledkù:

* Benchmarky.AddTests 
  * Q24_8Test           :  446.0400 ns 
  * Q16_16Test          :  454.3971 ns     
  * Q8_24Test           :  445.8681 ns
  * FloatTest           :  0.0097 ns         
  * DoubleTest          :  0.0953 ns    
* Benchmarky.SubtractTests          
  * Q24_8Test           :  442.8743 ns   
  * Q16_16Test          :  452.5564 ns
  * Q8_24Test           :  447.2059 ns      
  * FloatTest           :  0.0498 ns         
  * DoubleTest          :  0.0208 ns            
* Benchmarky.MultiplyTests                     
  * Q24_8Test           :  449.2814 ns       
  * Q16_16Test          :  448.9159 ns        
  * Q8_24Test           :  467.1417 ns           
  * FloatTest           :  0.0129 ns                  
  * DoubleTest          :  0.0413 ns                    
* Benchmarky.DivideTests
  * Q24_8Test           :  457.6201 ns                
  * Q16_16Test          :  475.9709 ns             
  * Q8_24Test           :  471.1466 ns         
  * FloatTest           :  0.0070 ns             
  * DoubleTest          :  0.2041 ns


 Z tìchto výsledkù vyplývá, že použití Fixed point typù je øádovì
 horší, než používat vestavìné floating point typy.

 Dále mùže øíct, že dìlení není nijak výraznì horší,
 než jiné operace na tomto typu procesoru.

 Dále vydíme, že aèkoliv benchmarky FixedPoint jsou stabilní,
 benchmarky u float a double jsou nestabilní,
 ba dokonce subjektivnì pochybuji o jejich správnosti.

