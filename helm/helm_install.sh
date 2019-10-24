#!/bin/bash
VERSION=${1:-v2.14.3}

./get_helm.sh --version $VERSION
kubectl create -f ./tiller-rbac-config.yaml
helm init --service-account tiller --history-max 200 --upgrade --wait
