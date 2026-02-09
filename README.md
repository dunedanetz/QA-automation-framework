# This repository contains a sample automation framework created as part of a technical assignment

This repository contains a small UI automation framework created as part of a technical assignment.

The goal is **not to include a great deal of tests**, but to demonstrate:
- Framework structure
- Tooling used
- Simple, clean, readable tests

## Disclaimer:

AI tools were used as a productivity aid for C# and NUnit syntax. 
All design decisions, structure, scope, and test logic were intentionally kept simple 
and aligned with my current level of hands-on automation experience.

---

## Tech Stack

- **C# (.NET 10)**
- **Microsoft Playwright**
- **NUnit**
- **GitHub Actions** CI Pipeline - run the tests on every GitHub push

---

## Project Structure

```text
automation_task/
├─ .github/workflows/       # CI pipeline
├─ src/
│ └─ AutomationTests/
│ ├─ Framework/             # Driver, config, test base
│ ├─ Pages/                 # Page Objects
│ ├─ Tests/                 # Smoke tests
│ ├─ appsettings.json
│ └─ AutomationTests.csproj
```
---

## Test Scope

The automated tests focus on **smoke testing of main functionality**:

- Homepage loads successfully
- “Join Waitlist” CTA navigates correctly
- Footer is visible and legal links work

The website is primarily a **corporate/marketing site**, without complex user input flows.  
For this reason, automation focuses on **navigation and visibility checks**.

---

## Running Tests Locally

Prerequisites:
- .NET SDK 10+
- Playwright browsers installed

Install browsers:
```bash
dotnet build
playwright install
```

Run tests:
```bash
dotnet test
```