#!/bin/bash
docker build -t counter .
echo "http://localhost:5010/api/v1/swagger/index.html"
docker run -it --rm -p 5010:5000 --name counter_5010 counter
