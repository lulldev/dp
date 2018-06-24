#! /bin/bash

frontendPID=-1
backendPID=-1
dotnet Frontend/Frontend.dll --configuration Release --launch-profile Production & frontendPID=$!
dotnet Backend/Backend.dll --configuration Release --launch-profile Production & backendPID=$!

echo $frontendPID >> "pid"
echo $backendPID >> "pid"
