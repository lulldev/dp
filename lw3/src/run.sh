#! /bin/bash

frontendPID=-1
backendPID=-1
textListenerPID=-1
dotnet Frontend/Frontend.dll --configuration Release --launch-profile Production & frontendPID=$!
dotnet Backend/Backend.dll --configuration Release --launch-profile Production & backendPID=$!
dotnet TextListener/TextListener.dll --configuration Release --launch-profile Production & textListenerPID=$!

cat /dev/null > "pid"
echo $frontendPID >> "pid"
echo $backendPID >> "pid"
echo $textListenerPID >> "pid"
