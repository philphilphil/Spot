#!/usr/bin/env bash
version=1.0
time=$(date +"%j%H%M")
go build -o builds/Spot -ldflags="-X 'main.BuildTime=$time' -X 'main.BuildVersion=$version'" . 
env GOOS=linux GOARCH=arm go build -o builds/Spot_arm -ldflags="-X 'main.BuildTime=$time' -X 'main.BuildVersion=$version'" . 