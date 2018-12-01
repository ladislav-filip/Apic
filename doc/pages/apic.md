var.title:Projekt Apic - ukázka kompletního REST API
var.description:Kompletní ukázka REST API napsaného v .NETu
var.keywords:asp.net core

---
# Projekt Apic - kompletní ukázka REST API

![ss](assets/img/dotnet-talks.jpg)

Zde je nějaký další text

## GDPR

Tento článek bych zřejmě nepsal a hodiny života nezabil, kdyby neexistovalo **Obecné nařízené o ochraně osobních údajů**, zkráceně GDPR. Jedná se o nařízení, které má pomoci k ochraně našich osobních údajů a přesněji specifikuje mimo jiné jak má být s osobními údaji nakládáno. Nejběžnější uživatelé internetu (a těch je valná většina) ochranu svých osobních údajů neřeší a s GDPR se tedy většinou setkají na webech v podobě otravných proužků, které se dožadují pozornosti a souhlasu s použitím cookies. 

Kromě procesních změn uvnitř firem tedy GDPR donutilo mnoho firem, potažmo vývojářů **implementovat do webových aplikací některé nové funkce**. Řada z nich už měla být součástí webových aplikací v minulosti, jen na to každý kašlal, jelikož za takové kašlání nebyly tak vysoké finanční postihy jako v současnosti. 

**ASP.NET Core 2.1 obsahuje několik nových funkcí, které implementaci GDPR ve webových aplikacích usnadňují.**

## Cookie Consent

Po založení projektu v ASP.NET Core 2.1 se můžeme všimnout nové konfigurační sekce v souboru **Startup.cs** a metodě **ConfigureService()**.

	services.Configure<CookiePolicyOptions>(options =>
	{
		options.CheckConsentNeeded = context => true;
		options.MinimumSameSitePolicy = SameSiteMode.None;
	});


Samotné zapojení do pipeline probíhá v **Configure()** metodě zavoláním:

	app.UseCookiePolicy();

Typickým umístěním je před voláním **UseMvc()**.

### Views (HTML)

Pokud je nastavena hodnota **CheckConsentNeeded** na true, je funkcionalita Cookie Consent pro podporu GDPR aktivní. V praxi to znamená, že **po načtení webu se v horní části zobrazí proužek s žádostí o schválení Podmínek užití a ukládání cookies**. Vzhled (HTML) této žádosti je standardně uložen ve `~\Views\Shared\_CookieConsentPartial.cshtml.` Tlačítko **Learn more** ve své výchozí konfiguraci odkáže uživatele na další stránku `~\Views\Home\Privacy.cshtml`, kde jsou podrobné Podmínky užití. Pokud uživatel odsouhlasí podmínky, vytvoří se nová cookie:

![Cookie Consent](https://cdn.miroslavholec.cz/articles/cookie-consent/cookie-consent.png)

## Když vám to uživatel neodklikne

Pokud máte funkci Cookie Consent na svém webu aktivní (a to při založení výchozí šablony ASP.NET Core 2.1 máte) a uživatel neschválí pravidla užití a ukládání cookies, můžete si snadno naběhnout, podobně jako já. Například **kolekce TempData používá cookies** ve chvíli, kdy chcete udělat redirect mezi ASP.NET Core Pages nebo ASP.NET Core metodami. Čili pokud máte v controlleru toto:

	TempData["Message"] = "Zkontrolujte formulář";
	return View();

vše vám bude fungovat dle očekávání a ve Views můžete vyzvednout bez problému `TempData["Message"]`.

Ve chvíli, kdy však uděláte redirect, například v rámci paternu PRG (Post/Redirect/Get):

	TempData["Message"] = "Položka byla přidána.";
	return RedirectToAction("Index");

máte smůlu, protože **TempData se neuloží**. 

## Essential Cookies

**Některé cookies webová aplikace pro svůj život potřebuje a zároveň nijak neodporují směrnici GDPR.** Takové cookies potřebujeme ukládat bez ohledu na to, zda uživatel udělil souhlas či nikoliv. V naší hantýrce se tyto cookies nazývají jako essential a pokud takovou cookie chceme vyrobit, musíme ji oflagovat. Pokud máme dostupný context, může to vypadat takto:

	context.Response.Cookies.Append("WebsiteTheme", "Dark", new CookieOptions { IsEssential = true });

**TempData cookies ve výchozím nastavení nejsou essential.** Pokud TempData nepoužíváte pro uchování osobních nebo citlivých údajů, nabízí se tedy otázka, jak přepnout TempData cookies na essential. A naštěstí to jde celkem snadno, opět ve třídě **Startup.cs** a metodě **ConfigureServices()**:

	services.Configure<CookieTempDataProviderOptions>(options => { options.Cookie.IsEssential = true; });

## Závěrečné poznámky

V tomto článku jsem se zaměřil pouze na Cookie Consent v souvislosti s TempData, nicméně novinek je v této oblasti více. Již výchozí ASP.NET Core 2.1 šablona obsahuje po přihlášení v sekci Manage Account nový tab **Personal Data**, kde může uživatel stáhnout svá osobní data ve formátu JSON nebo svá data zcela odstranit (což znamená odstranění kompletního uživatelského účtu).