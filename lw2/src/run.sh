#! /bin/bash

frontendPID=-1
backendPID=-1
exec dotnet Frontend/Frontend.dll --configuration Release --launch-profile Production & frontendPID=$!
exec dotnet Backend/Backend.dll --configuration Release --launch-profile Production & backendPID=$!

echo $frontendPID >> "pid"
echo $backendPID >> "pid"
