#!/bin/bash
docker build -t counter .
docker tag counter ureure/netcore-counter:0.3-unstable
docker push ureure/netcore-counter:0.3-unstable
