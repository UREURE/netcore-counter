#!/bin/bash
ARG1=${1:-PasswordMuyMuyDificilDeAdivinar0=0}

kubectl apply -f 01_counter-namespace.yaml
kubectl create secret generic redis --from-literal=password=$ARG1 --namespace=netcore-counter
kubectl apply -f 02_redis-pvc.yaml --namespace=netcore-counter
kubectl apply -f 03_redis-deployment.yaml --namespace=netcore-counter
kubectl apply -f 04_redis-service.yaml --namespace=netcore-counter
kubectl apply -f 05_counter-deployment.yaml --namespace=netcore-counter
kubectl apply -f 06_counter-service.yaml --namespace=netcore-counter
kubectl apply -f 07_counter-ingress.yaml --namespace=netcore-counter
