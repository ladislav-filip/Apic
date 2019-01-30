param (
    [string]$Project = "NewProjectTemplate"
)

[string]$SolutionFolder = [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path);

Get-ChildItem -recurse $SolutionFolder -include *.cs,*.csproj,*.sln,*.config,*.ps1,*.json,*.tsx,*.cshtml,*.props | where { $_ -is [System.IO.FileInfo] } | where { !$_.FullName.Contains("\packages\") } | where { !$_.FullName.Contains("\obj\") } | where { !$_.FullName.Contains("package.json") } | where { !$_.Name.Equals("rename.ps1") } |
Foreach-Object {
    Set-ItemProperty $_.FullName -name IsReadOnly -value $false
    [string]$Content = [IO.File]::ReadAllText($_.FullName)
    $Content = $Content.Replace('Apic', $Project)
    [IO.File]::WriteAllText($_.FullName, $Content, [System.Text.Encoding]::UTF8)
}

Get-ChildItem -recurse Apic*  | Rename-Item -NewName {$_.name -replace '^Apic',$Project}
# Rename-Item -path ([System.IO.Path]::Combine($SolutionFolder, 'Apic.sln')) -newName ($Project + '.sln')
