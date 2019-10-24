#!/bin/bash
PASSWORD=${1:-PasswordMuyMuyDificilDeAdivinar0=0}
NAMESPACE=${2:-netcore-counter}

kubectl create secret generic redis --from-literal=password=$PASSWORD --namespace=$NAMESPACE
kubectl apply -f 02_redis-pvc.yaml --namespace=$NAMESPACE
kubectl apply -f 03_redis-deployment.yaml --namespace=$NAMESPACE
kubectl apply -f 04_redis-service.yaml --namespace=$NAMESPACE
kubectl apply -f 05_counter-deployment-redis.yaml --namespace=$NAMESPACE
kubectl apply -f 05_counter-deployment-next.yaml --namespace=$NAMESPACE
kubectl apply -f 05_counter-deployment-user.yaml --namespace=$NAMESPACE
kubectl apply -f 06_counter-service-redis.yaml --namespace=$NAMESPACE
kubectl apply -f 06_counter-service-next.yaml --namespace=$NAMESPACE
kubectl apply -f 06_counter-service-user.yaml --namespace=$NAMESPACE
