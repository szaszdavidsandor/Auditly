# Auditly

**Projekt neve:** Auditly  
**Projekt típusa:** Könyvelői ügyfélportál és feladatkezelő webalkalmazás  
**Célcsoport:** Kis- és közepes könyvelőirodák, valamint azok lakossági és vállalkozói ügyfelei (KATA, átalányadózó egyéni vállalkozók, Kft.-k).

---

## 1. A probléma, amit a szoftver megold

A könyvelők és ügyfelek közötti napi kommunikáció jelenleg legtöbbször kaotikus: az igazolások és számlák e-mailekben, Viberen vagy Messengeren keverednek el, az ügyfelek nem látják át az adófizetési határidőket, a könyvelők pedig folyamatosan ugyanazokra a kérdésekre válaszolnak.

Az Auditly egyetlen központi platformra tereli a teljes folyamatot:
- Élőben követhetővé teszi az adózási kereteket és a fizetendő adókat.
- Struktúrált, visszakereshető feladatkezelést nyújt e-mailezés helyett.
- Átlátható digitális irattárat biztosít mindkét fél számára.

---

## 2. Főbb funkciók

### Adózási Keretfigyelő (KATA & Átalányadó)
Megjeleníti az ügyfél éves bevételeit és a jogszabályi keretösszeget (pl. 18 millió Ft-os alanyi mentes keret). Egy vizuális állapotsáv segítségével figyelmeztet, ha a vállalkozó közeledik a keretátlépéshez.

### Kanban-alapú Ügyfélkérés Kezelő (Ticket rendszer)
Az ügyfelek nem e-mailt írnak, hanem feladatot hoznak létre (pl. "Bérszámfejtés módosítása", "Nullás igazolás igénylése"). A könyvelő státuszok szerint mozgatja a kártyákat: **Új**, **Folyamatban**, **Ügyfélre vár**, **Kész**.

### Adóbefizetések és EPC QR-kód generálás
A könyvelő rögzíti az aktuális havi NAV fizetendőket (pl. SZOCHO, KATA, ÁFA). A rendszer automatikusan EPC formátumú banki QR-kódot generál, amit az ügyfél a saját mobilbankjával beolvasva másodpercek alatt átutalhat.

### Digitális Irattár
Kategóriákra bontott (Bérlapok, Bevallások, Számlák) biztonságos fájltároló, ahová mindkét fél tölthet fel bizonylatokat és dokumentumokat.

### Gyors értesítések és belső kommunikáció
Minden feladat alatt külön hozzászólási lehetőség érhető el, így a kérdések és válaszok közvetlenül az adott ügy mellett maradnak.

---

## 3. Technológiai architektúra

A szoftver a modern szoftverfejlesztési elvárásoknak megfelelően három különálló rétegre épül:

- **Sánta Márk - Frontend:** Angular 19 (TypeScript, SCSS) - Interaktív, mobilbarát felhasználói felület.
- **Szász Dávid Sándor - Backend:** C# .NET 9 Web API - Biztonságos REST API, JWT alapú hitelesítés, üzleti logika és jogosultságkezelés.
- **Sánta Márk és Szász Dávid Sándor - Adatbázis & Storage:** PostgreSQL (Supabase Cloud) - Felhőben futó relációs adatbázis és fájltárhely a dokumentumok számára.

---

## 4. Felhasználói szerepkörök

* **Könyvelő (Accountant):** Látja az összes hozzá tartozó céget, kezeli a feladatokat, rögzíti a fizetendő adókat és feltölti a hivatalos dokumentumokat.
* **Ügyfél (Client):** Kizárólag a saját cégének adatait, keretösszegeit, fizetendőit és feladatait éri el. Új kéréseket hozhat létre és számlákat tölthet fel.
* **Adminisztrátor (System Admin):** A rendszer kezelője. Ő hozza létre és törli a könyvelői és ügyfél fiókokat, beállítja az éves adókereteket, és ha bármi technikai hiba vagy elakadás van a rendszerben, ő tudja javítani.
