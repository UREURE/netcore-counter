docker build -t counter .
docker run -it --rm -p 5010:5000 --name counter_5010 counter
