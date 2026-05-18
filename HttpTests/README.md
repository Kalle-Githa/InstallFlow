# HttpTests — manuella API-tester för InstallFlow

Två separata mappar — en per roll. Varje `.http`-fil är **självständig** och innehåller eget login (och, där det behövs, eget setup).

```
HttpTests/
├── Admin/         ← alla requests körs som admin
├── Tekniker/      ← alla requests körs som tekniker
└── http-client.env.json
```

## Hur du använder en fil

Öppna vilken `.http`-fil som helst, börja högst upp:

1. Klicka **Send Request** på "Logga in" → token cachas i den här filen
2. (Om filen behöver kund/uppdrag) → Send Request på "Skapa kund" och "Skapa uppdrag" som följer direkt efter login
3. Kör sedan vilken request du vill längre ner

**Filerna är fristående.** Du behöver inte köra något i en annan fil först. Allt setup som filen behöver finns högst upp i filen själv.

## Varför egen login per fil?

Visual Studios `.http`-stöd cachar named requests (`# @name login`) **bara inom samma fil**. Det går alltså inte att referera till `login.response...` från en annan fil. Lösningen: varje fil har sin egen `login`-request.

## Skillnaden mellan mapparna

**`Admin/`** — testar att alla CRUD funkar och valideringar är på plats.

**`Tekniker/`** — testar att teknikergrejer funkar och att admin-skyddade endpoints **blockeras** (403). Inbyggda negativtester för rollkontroll.

| Endpoint | Admin → | Tekniker → |
|---|---|---|
| GET allt | 200 | 200 |
| POST customer/assignment/job | 201 | 201 |
| PATCH customer/assignment/job | 200 | 200 |
| DELETE customer/job | 204 | **403** |
| POST kategori | 201 | **403** |
| PATCH/DELETE kategori | 200/204 | **403** |
| POST/PATCH produkt | 201/200 | 201/200 |
| DELETE produkt | 204 | **403** |
| POST user | 201 | **403** |
| GET /api/carts (alla) | 200 | **403** |
| GET /api/carts/me | 200 | 200 |

## Lokal vs Azure

`http-client.env.json` definierar miljöer. Uppe till höger i `.http`-editor finns en dropdown — välj `local` eller `azure`. Byt URL till din riktiga App Service-adress när du är redo.

## Snabbtips

- **Felmeddelande "Unable to evaluate expression 'login.response...'"** → kör login-blocket högst upp i samma fil först.
- **401 Unauthorized** → tokenen har gått ut (1h livslängd). Kör login igen.
- **Vill se vad en token innehåller?** Kopiera token-strängen från svaret och klistra in på [jwt.io](https://jwt.io).
