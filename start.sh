#!/bin/bash
docker build -t counter .
docker run -d --name redis-bd -p 6379:6379 redis:5.0.3
echo "http://localhost:5010/api/v1/swagger/index.html"
docker run -it --rm -p 5010:5000 --name counter_5010 counter
docker container stop redis-bd
docker container rm redis-bd
