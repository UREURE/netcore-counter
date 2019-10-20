#!/bin/bash
ARG1=${1:-PasswordMuyMuyDificilDeAdivinar0=0}

kubectl apply -f 01_counter-namespace.yaml
kubectl create secret generic redis --from-literal=password=$ARG1 --namespace=netcore-counter
kubectl apply -f 02_redis-pvc.yaml --namespace=netcore-counter
kubectl apply -f 03_redis-deployment.yaml --namespace=netcore-counter
kubectl apply -f 04_redis-service.yaml --namespace=netcore-counter
kubectl apply -f 05_counter-deployment-redis.yaml --namespace=netcore-counter
kubectl apply -f 05_counter-deployment-next.yaml --namespace=netcore-counter
kubectl apply -f 05_counter-deployment-user.yaml --namespace=netcore-counter
kubectl apply -f 06_counter-service-redis.yaml --namespace=netcore-counter
kubectl apply -f 06_counter-service-next.yaml --namespace=netcore-counter
kubectl apply -f 06_counter-service-user.yaml --namespace=netcore-counter
kubectl apply -f 07_counter-ingress-user.yaml --namespace=netcore-counter
kubectl apply -f 08_counter-service-monitor-redis.yaml --namespace=netcore-counter
kubectl apply -f 08_counter-service-monitor-next.yaml --namespace=netcore-counter
kubectl apply -f 08_counter-service-monitor-user.yaml --namespace=netcore-counter
