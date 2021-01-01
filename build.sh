#!/usr/bin/env bash
version=1.0
#time=$(date +"%j%H%M")
lastCommitHash=$(git rev-parse --short HEAD)
go build -o bin/Spot_$version\_darwin -ldflags="-X 'main.CommitHash=$lastCommitHash' -X 'main.BuildVersion=$version'" . 
env GOOS=linux GOARCH=arm go build -o bin/Spot_$version\_arm -ldflags="-X 'main.CommitHash=$lastCommitHash' -X 'main.BuildVersion=$version'" . 
env GOOS=windows GOARCH=amd64 go build -o bin/Spot_$version\_win.exe -ldflags="-X 'main.CommitHash=$lastCommitHash' -X 'main.BuildVersion=$version'" . 