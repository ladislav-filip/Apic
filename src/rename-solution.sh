#!/bin/sh

clear

echo "Název projektu, např.: Apic:"
read ident

pwsh rename-solution.ps1 $ident 

read -n 1 -s