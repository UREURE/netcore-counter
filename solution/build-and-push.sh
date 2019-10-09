#!/bin/bash

ARG1=${1:-0.3-unstable}

docker build -t counter .
docker tag counter ureure/netcore-counter:$ARG1
docker push ureure/netcore-counter:$ARG1
