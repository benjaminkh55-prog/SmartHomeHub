📄 README.md (färdig att använda)
🏠 Smart Home Hub

Ett enkelt Smart Home system där man kan styra enheter, köra rutiner och få live uppdateringar via designmönster.

▶️ Hur man kör programmet
Öppna projektet i Visual Studio / VS Code
Kör Program.cs
Output visas direkt i konsolen
🧠 Designmönster
🗞️ Observer

Observer används för att skapa live uppdateringar när en enhet ändrar state.
När t.ex. en lampa sätts på, notifieras flera observers som Dashboard, AuditLogger och SystemLogger.
Detta är en en-till-många relation där alla subscribers automatiskt uppdateras.

🎛️ Command

Command används för att kapsla in actions som objekt, t.ex. TurnOnLampCommand.
Det gör att kommandon kan köas, loggas och köras senare.
Systemet stödjer även replay av senaste kommandon, vilket visar flexibiliteten.

🧠 Strategy

Strategy används för att byta systemets beteende via olika modes:

NormalMode → allt tillåtet
EcoMode → blockerar vissa actions
PartyMode → tillåter allt

Detta gör att logik kan ändras utan att ändra befintlig kod.

🧱 Facade

Facade (SmartHomeFacade) ger ett enkelt API för att styra hela systemet.
Programmet behöver inte känna till detaljer om devices, commands eller observers.
Det gör koden ren och lätt att använda.

🧩 Singleton

Logger är implementerad som en Singleton.
Det säkerställer att hela systemet använder samma instans för loggning.
Flera klasser kan logga utan att skapa nya objekt.

🏗️ Struktur

Projektet är uppdelat i ansvar:

Devices → Lamp, Thermostat, DoorLock
Commands → olika actions
Observers → Dashboard, Logger
Strategies → Modes
Facade → huvud-API

Detta gör systemet lätt att utöka.

📊 Klassdiagram 


Replaying last 2 commands...

[Dashboard] Thermostat set to 25°C
[Dashboard] DoorLock LOCKED

Action blocked by current mode

