# Vývoj počítačových her

## Programování počítačových her

### vyvoj herních mechanik

- Cena x Benefit
- Zabavnost
- Intuitivnost

### herní návrhové vzory

- Singleton: jedna instance
- Factory: generator veci

### skriptování her

- Tweeny
- C#, python

## Architektura herních engine

### vrstvy architektur

### výpočetní modely

- Herni smyska
  - hracuv vstup
  - updatovat svet
    - posunout objekty
    - zkontrolovat kolize
    - upravit rychlosti a pozice
  - vyrenderovat svet
  - prehrat zvuky
  -

### entity-component system

Zpusob jak reprezentovat GameObjecty.
Objekt ma komponenty, kazda zarucujici nejakou funkcionalitu
Efektivnejsi pro pamet, protoze to je struct v zasobniku a ne classa na halde

- Entity: GameObject ID
- Komponenty: Data a komu patri
- Systemy: Spousti komponenty na entitach

### správa paměti

Je dulezite spravne vyuzivat cache
Tudiz je efektivnejsi mit data v zasobniku jako strukty, nez pohazene na halde

### příklady konkrétních instancí architektur

## Herní design

### kdo je herní designér

### osy herního designu

### herní žánry

### specifika herních platforem

### game design dokument

#### vlastnosti

#### struktura

#### UML diagramy pro popis herních mechanismů

#### herní prostor

#### postavy

#### specifikace dialogů

### historie herního trhu

## Vývojový cyklus počítačové hry

### fáze vývojového cyklu

### herní design řízený daty

### správa dat

### testování počítačových her

### vývojářské role

### herní analytiky

### vodopádový model a~agilní metodiky návrhu her

### obchodní modely komercializace her

## Narativita a~hry

### rozdíl mezi games of emergence a~games of progression

### chtěná a~nechtěná emergence

### environmentální storytelling

### procedurální rétorika

### ludonarativní disonance

# Počítačová grafika pro hry

## Pojmy

### Homogenní souřadnice

Homogenní souřadnice umožňují reprezentaci bodů a transformací v geometrickém prostoru s využitím matice vyšší dimenze.
Používají se pro sjednocení translací, rotací a škálování do jednoho matematického modelu.
Přidáním čtvrté souřadnice (w) do 3D prostoru umožňují snadnou aplikaci perspektivních projekcí.

Například bod [x, y, z] lze v homogenních souřadnicích zapsat jako [x, y, z, w], kde w ≠ 0.

### Afinní a projektivní transformace v rovině a v prostoru

Afinni \~ Ortogonalni

Afinní transformace zahrnují operace jako translace, rotace, škálování a šikmé posunutí, které zachovávají rovnoběžnost přímek.
Projektivní transformace rozšiřují afinní transformace o perspektivní zkreslení, což je klíčové pro modelování 3D scén.
Typický příklad projektivní transformace je pohledová projekce ve 3D grafice.

### Kvaterniony

Kvaterniony slouží k reprezentaci rotací ve 3D prostoru bez singularit spojených s Eulerovými úhly (gimbal lock).
Jsou čtyřrozměrným rozšířením komplexních čísel ve tvaru $q = w + xi + yj + zk$, kde $w$ je skalární část a $x, y, z$ jsou imaginární.
Jsou efektivní pro interpolaci rotací pomocí SLERP (Spherical Linear Interpolation).

## Křivky

### Spline funkce

Spline funkce jsou hladké křivky používané k aproximaci nebo interpolaci datových bodů.
Nevyhodou je, ze pokud se pohne 1 bodem, uz nebude spojita a musi se dorovnat vsechnny ostatni (kubicka varianta to nema).

![](img/spliny.png)

### Interpolace kubickými spliny

Retez spojenych krivek.
Kubické spliny jsou specifické typy spline funkcí, které interpolují mezi body tak, aby byla zajištěna spojitost první a druhé derivace.

### Bézierovy křivky

Bézierovy křivky jsou definované kontrolními body a Bernsteinovými polynomy.
Umožňují tvorbu hladkých křivek a jsou klíčové v grafickém designu.

Linearni bezierovka: Lerp(A, B, t)
Kvadraticka bezierovka: Lerp(Lerp(A, B, t), Lerp(B, C, t), t)
Kubicka bezierovka: Lerp mezi 2 kvadratickymi bezierovkami

![](img/bezierovky.png)

### Catmull-Rom spliny

Vychazi z Cubic Hermit spline.
2 body a 2 vektory (vstupni a vystupni)

![](img/catmulRom.png)

### B-spliny

B-spliny jsou zobecněním Bézierových křivek, umožňují lokální kontrolu nad tvarem křivky.
Jsou vhodné pro modelování složitých tvarů v CAD systémech.

![](img/bspliny.png)

## Textury

### Vzorkování a kvantování obrazu

Vzorkování (Sampling) je proces získávání hodnot z textury (prectneni pixelu).

Kvantování omezuje počet úrovní intenzity, což snižuje náročnost na paměť.
Treba z realnych cisel na cisla diskretni.

### Anti-aliasing

Anti-aliasing redukuje zubaté okraje (aliasing) v obrazu.
Aplikuje se pouze na pixelech, jez obsahuje hranu 2 trojuhelniku.

Metody:

- MSAA (Multi-Sample Anti-Aliasing) - vicero vzorku pro jeden pixel (4, 8, 16 vzorku)
- SSAA (Super-Sample Anti-Aliasing) - Vicero pixelu pro 1 pixel (4, 16)
- FXAA (Post-processing) - vpodstate rozmaze / vyprumeruje pixely, ktere maji vysoky kontrast vuci okolnim pixelum

### Textury

Textury se používají k přidání detailů na povrch objektů.
Mohou být 2D, 3D, nebo procedurální.
Příkladem je texturování dřeva nebo kamene.

### Změna kontrastu a jasu

Manipulace kontrastu a jasu mění vzhled textury.
Tyto úpravy lze provést pomocí lineární transformace intenzity pixelů.

### Kompozice poloprůhledných obrázků

Při kompozici poloprůhledných obrázků se používá alfa kanál, který určuje průhlednost.
Kombinace barev probíhá například pomocí operace „over“ v alfa kompozici.

## Reprezentace 3D scén

### Výpočet viditelnosti

Určuje, které části scény jsou viditelné z určitého pohledu.
Metody

- Z-buffering (Depth textura / buffer) - Rasteriazace
- BSP stromy (Binary space partitioning) - Raytracing

### Výpočet vržených stínů

Vržené stíny přidávají hloubku do sceny.

Metody:

- shadow maping (svetlo renderuje hloubku do textury)
- path tracing (co pixel, to paprsek do svetla)

### Měkké stíny

Měkké stíny simulují přirozené rozmazání stínů, které vzniká díky rozptylu světla.

Metody:

- Raytracing to ma implicitne (pokud svetlo neni bod)
- Shadow maping se podivaji na pixely okolo (pixel filtering)

### Rozptyl světla pod povrchem

subsurface scattering.
Rozptyl světla pod povrchem simuluje průchod světla materiály, jako je kůže nebo voda.

### Modely osvětlení a stínovací algoritmy

Modely osvětlení (Phong, Blinn-Phong) a stínování (Gouraud, Phong) definují, jak se světlo šíří a odráží na povrchu.

- leskla slozka (specular)
- matna slozka (diffuse)
  - odrazova slozka (mirror)

### Rekurzivní sledování paprsku

Raytracing.
Simulace světelných cest zahrnující odrazy a refrakce.
Používá se ve filmové grafice, je pomala, ale realisticka.

### Fyzikální model šíření světla

#### Radiometrie

Studium fyzikálních vlastností světla, jako je intenzita, jas a energetický tok.

#### Zobrazovací rovnice

Zobrazovací rovnice (rendering equation) je klíčovým konceptem v radiometrii a počítačové grafice.
Popisuje, jak světlo interaguje s povrchy ve scéně a jak je výsledný obraz vytvořen.
Rovnice bere v úvahu všechny možné cesty světla, které přispívají k osvětlení daného bodu na povrchu.

Matematicky je zobrazovací rovnice vyjádřena jako:

$$L_o(x, \omega_o) = L_e(x, \omega_o) + \int_{\Omega} f_r(x, \omega_i, \omega_o) L_i(x, \omega_i) (\omega_i \cdot n) d\omega_i$$

neboli:
vysledna barva = emitovane svetlo + svetlo z odrazu (= rekurzivni BDRF $*$ svetla $*$ cosine law)

### Algoritmus sledování cest

Path tracing je pokročilá metoda simulující globální osvětlení.
Pro kazdy pixel se vysle nekolik paprsku.
Kazdy paprsek se odrazi po scene, dokud nenarazi na svetlo, nebo neskonci po X krocich.

### Předpočítané globální osvětlení

Techniky jako lightmaps ukládají výsledky osvětlení pro rychlejší vykreslování.
napr. WOWko.
Svetlo se zapece pri kompilaci hry, nebo za runtimu a pak se jakoby cache hodnota pouziva.
Umoznuje pokrocile stiny a barvy, ale je to vse staticke.

### Výpočet globálního osvětlení v reálném čase

Real-time GI zahrnuje techniky jako voxel cone tracing nebo ray tracing na GPU.

### Stínování založené na sférických harmonických funkcích

Radiosita vuci environment mape.
Je to vlastne diffuzni slozka environment mapy.

### Předpočítaný přenos radiance

Predpocitani env mapy pres sfericke harmonicke funkce do paaraametru rovnice.
Staci ji mit jako rovnici a parametry, textura by byla moc velka a ochylka je < 3%.

![](img/radiosityBake.png)

## Animace postav

### Skinning

Skinning váže geometrii na kostru.
Lineární blend skinning je běžný algoritmus používaný ve hrách.

### Rigging

Rigging zahrnuje tvorbu kostry a kontrolních mechanismů pro animaci postavy.

### Morphing

Morphing interpoluje mezi různými tvary nebo animacemi.

## Architektura grafického akcelerátoru

### Předávání dat do GPU

Data jako vertex buffery a textury jsou přenášena z CPU do GPU pomocí API jako OpenGL nebo DirectX.

### Textury a GPU buffery

Textury a buffery jsou optimalizovány pro rychlý přístup během vykreslování.

### Programování GPU shaderů

Shadery jsou malé programy běžící na GPU, které ovlivňují vzhled scény.

## Jazyky

### OpenGL

OpenGL je grafická API umožňující programování 2D a 3D grafiky.

### GLSL

GLSL je jazyk pro psaní shaderů v OpenGL.

### CUDA

CUDA je platforma pro paralelní programování na GPU, využívající výpočetní schopnosti grafické karty.

### OpenCL

OpenCL je standard pro paralelní výpočty na různých typech hardwaru.

## Komprese

### Principy komprese rastrové 2D grafiky

Redukce velikosti obrazových dat odstraněním redundantních informací.
Bezeztrátová komprese zachovává původní obraz, ztrátová redukuje kvalitu.

### Standard JPEG

JPEG je ztrátový kompresní formát, který využívá DCT (diskrétní kosinovou transformaci) a quantizaci (v zavislosti na urovni DCT) na submatici 8x8.

![](img/DCT.png)

### Komprese videosignálu

Využívá prostorovou i časovou redundanci pro efektivní ukládání videa.
Vyuziva DCT, pohybove vektory a quantizaci (v zavislosti na urovni DCT).
Sniky jsou 3 typu:

- I = plny snimek
- P = rozdil mezi poslednim I nebo P snimkem
- B = rozdil snimku pred a snimku po
  v pomeru napr. 2:5:12

Příkladem je H.264 nebo H.265.

# Umělá inteligence pro počítačové hry

## Architektura autonomního agenta

### Percepce

Percepce představuje schopnost agenta získávat informace o prostředí.
Agent využívá senzory (virtuální nebo fyzické) ke sledování okolního světa.
Například v počítačových hrách může agent "vidět" objekty v dosahu viditelnosti nebo slyšet zvuky generované hráčem.
Tyto informace agent využívá k rozhodování a adaptaci na změny prostředí.

### Mechanismus výběru akcí

Mechanismus výběru akcí určuje, jak agent reaguje na vnímané podněty.
Může jít o jednoduché přístupy, jako jsou předdefinované if-then pravidla, nebo složitější algoritmy zahrnující stromy rozhodování či neuronové sítě.
Například NPC v RPG hrách může reagovat na hráčovo přiblížení útokem, útěkem nebo zahájením dialogu.

### Paměť

Paměť umožňuje agentovi uchovávat informace o předchozích stavech nebo událostech.
Napr. informace o jiz sebranych zbranich, nebo dlouhodobe cile.

## Psychologické inspirace

Psychologické inspirace vycházejí z modelování lidského nebo zvířecího chování.
Tyto přístupy zahrnují simulaci emocí, potřeb nebo motivačních modelů, například pomocí Maslowovy pyramidy.
Herní postava může například vykazovat chování závislé na hladině "strachu", "hladu" nebo "odvahy".

## Metody pro řízení agentů

### Symbolické a konekcionistické reaktivní plánování

Symbolické plánování využívá logické reprezentace a algoritmy.
Konekcionistické (sub-symbolic) se opírá o neuronové sítě, automaty a if-then pravidla.

Symbolické přístupy jsou přesnější, zatímco konekcionistické jsou flexibilnější v dynamických prostředích.

### Hybridní přístupy

Planovai cast pouziva symbolicke planovani pro vytvareni planu.
Reaktivni cast rychle reaguje, bez vetsich vypoctu ci planovani.

Například může agent využívat logiku pro dlouhodobé plánování a neuronové sítě pro rychlé reakce.

### Prostor rozhodování

Prostor rozhodování popisuje všechny možné akce, které může agent provést.
Optimalizace tohoto prostoru je klíčová pro rychlé a efektivní rozhodování.

### If-then pravidla

Tato pravidla definují jednoduché reakce na konkrétní podmínky.
Například "Pokud hráč zaútočí, agent se brání".

### Skriptování

Skriptování umožňuje tvůrcům her ručně definovat chování agentů pomocí programovacích skriptů.

### Sekvenční konečný automat

Sekvenční konečné automaty reprezentují chování agenta jako množinu stavů a přechodů mezi nimi.
Typický příklad je hlídkující stráž v stealth hrách.

### Stromy chování

Stromy chování představují hierarchické struktury, které organizují rozhodování agenta.
Jsou snadno čitelné a flexibilní pro komplexní chování.

## Problém hledání cesty

### Lokální navigační pravidla

Řeší lokální pohyb agenta v prostředí
Metody:

- Reynoldsovy steeringy
- VO (Velocity Obstacle)
- RVO
- Context Steering

Například obcházení překážek nebo spolupráce více agentů.

### Hledání cesty

Algoritmy:

- A\*
- JPS+
- goal bounding
- RRT
- RRT\*
- LPA\*
- MPAA\*
- bidirectional search

### Reprezentace prostoru

Prostor může být reprezentován pomocí geometrie (trojúhelníkové sítě), viditelnosti (viditelné grafy) nebo mřížek.
Správná volba ovlivňuje efektivitu navigace.

## Komunikace a znalosti v multiagentních systémech

### Ontologie

Ontologie popisují společnou strukturu znalostí, kterou agenti sdílejí.
Například definují, co je nepřítel, zbraň nebo úkryt.

### Řečové akty

Řečové akty jsou modely komunikace mezi agenty, které zahrnují požadavky, nabídky, příkazy nebo dotazy.

### FIPA-ACL

Standardní jazyk pro komunikaci agentů umožňující interoperabilitu mezi různými systémy.

### Protokoly

Komunikační protokoly definují, jak agenti spolupracují.
Například protokol aukce pro alokaci zdrojů.

## Distribuované řešení problémů

### Kooperace

Agenti spolupracují na dosažení společného cíle, například na rozdělení úkolů při stavbě herního světa.

### Nashova ekvilibria

Rovnovážný stav, kdy žádný agent nemá motivaci měnit své rozhodnutí.

### Paretova efektivita

Stav, kdy nelze zlepšit situaci jednoho agenta bez zhoršení situace jiného.

### Alokace zdrojů

Distribuce omezených zdrojů mezi agenty.

### Aukce

Mechanismus, kde agenti soutěží o zdroje na základě nabídky a poptávky.

## Metody pro učení agentů

### Zpětnovazební učení

Agenti se učí na základě zpětné vazby z prostředí, například pomocí metody reinforcement learning.

### Základní formy učení zvířat

Inspirace přichází z učení zvířat, jako je klasické podmiňování nebo observační učení.

## Procedurální modelování stavového prostoru (forward model) a jeho prohledávání

### A\*

```
while fronta  is  not empty
	odeber hlavu
	pridej deti hlavy do fronty tak, ze:
		pokud tam neni, jakozto vzdalenost + heuristika
		pokud tam je, tak jen upravit odhad vzdalenosti
		a pokud se prida / zmeni, upravit akci ktera do nej vede
	pokud timeout (omezeny cas a stale nemame nejlepsi cestu)
		vrat zacatek fronty
```

### ABCD

Alpha Beta Considering Durations

Rekurzivni depth-first iterativni prohledavani pro simultalni tahy s apha-beta pruningem.

### MCTS

Monte-carlo tree search

- Select
- Expand
- Simulate
- Backpropagate

### UCB

Upper confidence bounds

UCB = Exploitace + c \* Explorace

Musime se rozhodnout, jestli vybrat zarucenou vyhru, nebo prozkoumat horsi cestu (ktera mozna povede k vetsi vyhre).

### PGS

Portfolio Greedy Search

- Aplikuj defaultni skript na vsechny jednotky
- Najdi nejvyhodnejsi skript pro vsechny jednotky spolecne
- Najdi nejpravdepodobnejsi nepriteluv skript pro vsechny jednotky spolecne
- Dokud mame zdroje
  - Najdi lepsi skript pro nepratelskou jednotku
  - Najdi lepsi skript pro svou jednotku

Oproti minmaxu (kteremu se podoba) nemusi najit nejvhodnejsi reseni

Napr. mam jednotku a chci kazdymu vojaku priradit ukol (skript z portfolia), tak abych zabil co nejvice nepratel

### Portfolio skriptů

- Attack Closest
- Attack Weakest
- Kiting (Move away)
- Attack Value
- No OverkKill Attack Value

## Klasifikace metod procedurálního generování

Procedurální generování lze klasifikovat podle generovaného obsahu, jako jsou terény, objekty nebo hudba.

## Přístupy pro generování

### Terénu

Generování realistických nebo stylizovaných krajin pomocí šumových funkcí.

### Vizuálních efektů

Procedurální vytváření efektů, jako jsou kouř nebo exploze.

### Hudby

Generování melodie nebo rytmu na základě algoritmů.

### Předmětů

Automatické vytváření unikátních zbraní nebo předmětů.

### Bludišť a dungeonů

Algoritmy pro vytváření komplexních herních map.

## Šumové funkce

### Perlin

Generuje plynulé a realistické šumy.

### Simplex

Efektivnější varianta Perlinova šumu.

### Worley

Šum vhodný pro generování buněčných struktur.

## Celulární automaty

### L-systémy

Modelování růstu rostlin.

### Grafové

Reprezentace vztahů mezi prvky.

### Tvarové gramatiky

Definují pravidla pro generování struktur.

## Answer set programming

Technika pro logické modelování a řešení problémů.

## Algoritmus kolapsu vlnové funkce

Procedurální generování na základě omezení podobných pravidlům sodoku.

## Metody smíšené iniciativy

Kombinují lidský a algoritmický přístup pro tvorbu obsahu, například herních map nebo questů.
