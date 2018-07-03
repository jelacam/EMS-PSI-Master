P4 EMS-Team

Standardi i modeliranje EES	
- model u skladu sa CIM-om, obezbediti visekorisnicki pristup

--------------------------------------------------------------
SCADA
- koristiti statusne i analogne ulazne i izlazne tacke
- obezbediti (sa remote uredjaja) online informacije o trenutnom stanju/statusu 'tacaka/sistema'
- remote uredjaje simulirati protocol simulatorima
- obezbediti izdavanje komandi za izlazne tacke (sa simulacijom uspesnog realizovanja komandi - npr automatska promena vrednosti odgovarajucih ulaza)

--------------------------------------------------------------
Cloud	
- Koristiti microservice arhitekturu. Servisi treba da budu podeljeni na mikroservise i medjusobna komunikacija se odvija na App Service Fabric

--------------------------------------------------------------
Upravljanje i optimizacija
-Razviti algoritam za minimizaciju potrošnje goriva u generatorima. Algoriatam se bazira na metodi linearnog programiranja. Ulazi u optimizaciju su:
- Goriva i njihove cene
- Generatori sa tehničkim ograničenjem minimalne i maksimalne snage i sa datom linearnom krivom potrošnje (l/MWh)
- Ukupna snaga koju je treba proizvesti
- Podešenja kontrolabilnosti, neki generatori su upravljivi (konvencionalni, dizel, gas, na ugalj) a drugi nisu (na primer renewables)
Optimizacija se radi pomoću dva algoritma, 1) linearno programinje, 2) genetski algoritam. 
Algoritam treba da bude organizovan kao servis sa automatskim vremenskim okidanjem proračuna.

--------------------------------------------------------------
DMS sistem

- Sistem upravljanja energijom je sistem računarskih alata koji se koriste dispečeri mreža za prenos električne energije da nadziru, upravljaju i optimizuju performanse proizvodnje i prenosa električne energije. 
- U sistemu postoje vetro farme koje učestvuju u proizvodnji električne energija sa ostalim konvencionalnim (ugalj, gas, dizel) izvorima električne energije. 
- Obezbediti prikaz udela u proizvodnji vetro farme koji je izražen u % (u odnosu na ukupnu potrošnju sistema) i kW. Takođe potrebno je prikazati ukupnu količinu energije koja ja proizvedena za period od mesec dana, četiri meseca i godinu dana. 
- Potrebno je prikazati dijagrame proizvodnje za trenutni dan. 
- Na osnovu količine energije proizvedene u vetro farmi potrebno je proračunati: 
•        Dobit (pojedinačno ili po regionalnoj pripadnosti) izraženu u $. 
•        CO2 redukciju (smanjenje CO2 emisije izraženo u tonama). 
- Proizvodnja vetro farme direktno zavisi od intenziteta vetra.
