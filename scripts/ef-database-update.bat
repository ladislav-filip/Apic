@echo off
cd..
cd src
dotnet ef database update -p Apic.Data -s Apic.Web -v
pause