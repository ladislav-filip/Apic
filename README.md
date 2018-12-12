[![Build status](https://mholec.visualstudio.com/DEV/_apis/build/status/RestApiCz%20-%20Apic)](https://mholec.visualstudio.com/DEV/_build/latest?definitionId=54)
# ASP.NET Core RESTful Web Api

Repositář obsahuje zdrojové kódy solution, kterou je možné použít jako výchozí šablonu pro vývoj RESTového Web API postaveného na technologii ASP.NET Core.

**Repositář se skládá ze dvou částí:**

- **doc** - dokumentace, kompilovaná do podoby webu **[restapi.cz](https://www.restapi.cz)**
- **src** - zdrojové kódy výchozí solution

## Status

- **doc** - draft, neobsahuje nyní relevantní informace, testuje se workflow
- **src** - proof of concept


Současný stav projektu je ve fázi prototypu.

## Technologie

- ASP.NET Core 2.2
- EF Core 2.2
- Windsor Castle 4.2
- Swashbuckle 4.0
- MSTest, Moq, FluentAssertions

## Guidelines

Cílem solution **(src)** je poskytnout vývojářům výchozí šablonu, pro tvorbu RESTových Web API postavených na webovém frameworku ASP.NET Core a souvisejících technologiích (viz. výše). Architektura celého řešení zohledňuje dle priority od nejdůležitější:

- standardy, RFC v souvislosti s protokolem HTTP(S) a souvisejícími
- ustálené zvyklosti vycházející z architektonického stylu REST
- řešení vhodná pro obecná robustní API

Šablona solution má přínos zejména pro vývojáře, jejichž cílem je stavba robustního Web API, které má být obecné a dostupné pro širokou technickou veřejnost. Reflektuje především standardy a ustálené zvyklosti, jejichž dodržení je bezpodmínečné pro použitelnost.

