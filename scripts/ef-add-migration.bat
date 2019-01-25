@echo off
cd..
cd src
set /p id="Enter migration name: "
dotnet ef migrations add %id% -p Apic.Data -s Apic.Web -v
pause