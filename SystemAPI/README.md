# System API
Dette er systemets hoved, hvor alt kommunikation går igennem.

## Controllers
API'et indeholder "controllers" til at administere diverse modeller og interegere med systemet. Det er disse der indeholder metoderne, som kan kaldes.

### CRUD for modellerne
>"CRUD" står for Create, Read, Update og Delete, de fire standard metoder til at administere.

Blandt disse controllers har vi vores CRUD-controllers, som administere modellerne efter en vis standard.

Da de følger CRUD, har de alle en metode til at hente en liste af deres data, samt en metode til at oprette, redigere og slette dem.

Derudover har de alle også en metode til at hente en enkelt.