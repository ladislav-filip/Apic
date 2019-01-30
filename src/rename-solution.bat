set /p id="Enter new project name: "
powershell -ExecutionPolicy Bypass -file "rename-solution.ps1" %id%