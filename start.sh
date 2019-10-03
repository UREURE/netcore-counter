#!/bin/bash
docker build -t counter .
docker network create practica
docker run -d --name redis-bd --network practica redis:5.0.3
echo "http://localhost:5010/api/v1/swagger/index.html"
docker run -it --rm -p 5010:5000 --network practica --name counter_5010 counter
