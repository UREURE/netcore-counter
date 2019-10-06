#!/bin/bash

./get_helm.sh
kubectl create -f ./tiller-rbac-config.yaml
helm init --service-account tiller --history-max 200 --upgrade
