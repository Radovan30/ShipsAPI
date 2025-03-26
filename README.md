# ShipsAPI
# 🛳 Lodě (Battleship) – REST API v ASP.NET Core

Tento projekt je implementací klasické hry **Lodě** pro dva hráče pomocí C# a REST API. Hráči se střídají ve střílení na protivníkovu herní desku, dokud nejsou všechny lodě jednoho z hráčů potopeny.

---

## 🔧 Technologie

- ASP.NET Core Web API
- C# (.NET 8)
- Swagger (pro dokumentaci API)
- DTO modely pro přenos dat
- OOP přístup s využitím dědičnosti a enkapsulace

---

## 🗂 Struktura projektu

| Složka            | Popis |
|-------------------|-------|
| `Controllers/`    | Obsahuje `GameController.cs`, který definuje REST API endpointy |
| `Services/`       | Obsahuje `GameService.cs` a `IGameService.cs` – hlavní herní logika |
| `Models/`         | Obsahuje modely jako `Board`, `Cell`, `Player`, `Ship` a jejich podtřídy |
| `DTOs/`           | Obsahuje datové třídy `GameInitRequest`, `FireRequest` apod. |
| `Program.cs`      | Konfigurace služby a DI kontejneru |

---

## 🚀 Jak projekt spustit

1. Otevři řešení ve Visual Studiu 2022 nebo novějším
2. Ujisti se, že máš .NET 8 SDK
3. V `Program.cs` je `GameService` zaregistrován jako singleton:

```csharp
builder.Services.AddSingleton<IGameService, GameService>();
