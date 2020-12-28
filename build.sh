#!/usr/bin/env bash
version=1.0
time=$(date +"%j%H%M")
go build -o builds/Spot_$version\_darwin -ldflags="-X 'main.BuildTime=$time' -X 'main.BuildVersion=$version'" . 
env GOOS=linux GOARCH=arm go build -o builds/Spot_$version\_arm -ldflags="-X 'main.BuildTime=$time' -X 'main.BuildVersion=$version'" . 
env GOOS=windows GOARCH=amd64 go build -o builds/Spot_$version\_win.exe -ldflags="-X 'main.BuildTime=$time' -X 'main.BuildVersion=$version'" . 