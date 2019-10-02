docker build -t counter .
docker run -it --rm -p 5000:5010 --name counter_5010 counter
