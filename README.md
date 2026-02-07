<<<<<<< HEAD
# rosetic-automation-task
Rosetic QA assignment automation task
=======
# Rosetic Automation Task

This repository contains a small UI automation framework created as part of a technical assignment.

The goal is **not exhaustive test coverage**, but to demonstrate:
- Framework structure
- Tooling choices
- Clean, readable tests
- Reasonable automation scope for a marketing website

---

## 🧰 Tech Stack

- **C# (.NET 10)**
- **Microsoft Playwright**
- **NUnit**
- **GitHub Actions** (CI)

---

## 📁 Project Structure

automation_task/
├─ .github/workflows/ # CI pipeline
├─ src/
│ └─ Rosetic.AutomationTests/
│ ├─ Framework/ # Driver, config, test base
│ ├─ Pages/ # Page Objects
│ ├─ Tests/ # Smoke tests
│ ├─ appsettings.json
│ └─ Rosetic.AutomationTests.csproj


---

## Test Scope

The automated tests focus on **smoke testing of main functionality**:

- Homepage loads successfully
- “Join Waitlist” CTA navigates correctly
- Footer is visible and legal links work

The website is primarily a **corporate/marketing site**, without complex user input flows.  
For this reason, automation intentionally focuses on **critical navigation and visibility checks**.

Only a small number of tests are included to keep the framework:
- Stable
- Easy to reason about
- Easy to discuss during a technical interview

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
>>>>>>> e8b8f6d (Initial commit)
