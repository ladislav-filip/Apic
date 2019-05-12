#!/bin/sh

clear

echo "Název migrace:"
read ident

dotnet ef migrations add $ident --project ../src/Apic.Data --startup-project ../src/Apic.Web

read -n 1 -s