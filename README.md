# Counter

Proyecto académico con el objetivo de crear una aplicación *.NET Core* que:

1. Consuma una base de datos.
2. Se pueda ejecutar localmente con [Visual Studio](https://visualstudio.microsoft.com/es/vs/) 2019 (se debe configurar qué servicio Redis externo debe consumir).
3. Se pueda ejecutar localmente con [Docker](https://www.docker.com/) (se debe configurar qué servicio Redis externo debe consumir).
4. Se pueda ejecutar localmente con [Docker-compose](https://docs.docker.com/compose/) levantando un servicio Redis y con la aplicación preparada para consumirlo.
5. Se pueda ejecutar en un clúster Kubernetes en [GKE](https://cloud.google.com/kubernetes-engine/?hl=es) levantando un servicio Redis, y con la aplicación preparada para consumirlo.
6. Publique mensajes de *log* de la aplicación en formato JSON por la salida estándar.

## Descripción de la aplicación

La aplicación es un servicio Web API simple, que permite utilizar un contador en un servicio Redis, con las siguientes operaciones:

* Leer el contador.
* Incrementar el contador, y devolver el valor del contador incrementado.

Además, tiene un método auxiliar para generar un error, y comprobar su publicación en el sistema de *Logs*.

## Funcionamiento de la aplicación

El API publicado por la aplicación está documentado con [Swagger](https://swagger.io/). Se puede consultar la documentación ejecutando la aplicación, y accediendo a la dirección *http://**host**:**puerto**/api/v1/swagger/index.html*:

![Swagger](./img/swagger.png)

La aplicación se puede configurar con el archivo [appsettings.json](./solution/src/Counter.Web/appsettings.json), para establecer, por ejemplo, la información de conexión con Redis. La configuración puede ser sobreescrita por las variables de entorno que se establezcan en el contexto en el que se ejecute.

## Requisitos

Para compilar la aplicación es necesario:

* [SKD de .NET Core](https://dotnet.microsoft.com/download) 2.2.

Para ejecutar la aplicación es necesario:

* [Runtime de .NET Core](https://dotnet.microsoft.com/download/dotnet-core/2.2) 2.2.

En los ejemplos se ha utilizado el sistema operativo (aunque no es un requisito esencial):

* [Debian 9](https://www.debian.org/index.es.html).

## Ejecutar localmente la aplicación

Para la ejecución con **Docker**, es necesario configurar la conexión con un servicio Redis pre-existente en el archivo [appsettings.json](./solution/src/Counter.Web/appsettings.json). Una vez configurada, se puede utilizar:

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter/solution
chmod 700 *.sh
./start.sh
```

Para la ejecución con **Docker-compose**, se levanta automáticamente un servicio Redis junto con el de la aplicación, configurando las variables de entorno en el archivo [.env](./solution/.env). Una vez configuradas las variables, se puede utilizar:

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter/solution
chmod 700 *.sh
./start-compose.sh
```

## Ejecutar la aplicación en Kubernetes GKE

Requisitos:

* Conectar con un clúster Kubernetes de GCP previamente creado en GKE. Por ejemplo, ejecutando:

```bash
gcloud container clusters get-credentials <nombre_clúster_GKE> --zone <zona_clúster_GKE> --project <nombre_proyecto_GCP>
```

* Tener instalado *kubectl*. Si no se tiene instalado, se puede instalar con:

```bash
sudo apt-get install kubectl -y
```

* Tener instalado un *Ingress Controller*. Si no se tiene instalado, se puede instalar el de *Nginx* con:

```bash
kubectl create clusterrolebinding cluster-admin-binding --clusterrole cluster-admin --user $(gcloud config get-value account)
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/static/mandatory.yaml
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/static/provider/cloud-generic.yaml
```

Para la ejecución de la aplicación en un clúster de Kubernetes en GKE, se pueden utilizar las imágenes subidas de esta [aplicación](https://cloud.docker.com/repository/registry-1.docker.io/ureure/netcore-counter), y de [Redis](https://hub.docker.com/_/redis), en [Docker Hub](https://hub.docker.com/). Por ejemplo, ejecutando:

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter/k8s
chmod 700 *.sh
./start-kubernetes.sh *poner_aquí_la_contraseña_que_se_desee_poner_para_el_servicio_Redis*
```

Una vez instalado, se puede ver en qué IP está expuesta la aplicación fuera del clúster con:

```bash
kubectl get ingress netcore-counter --namespace=netcore-counter
```

La IP en la que está expuesta la aplicación fuera del clúster está en el campo *ADRESS*. En este ejemplo, es *34.76.93.8*:

![IP Ingress](./img/ip-ingress.png)

**¡¡¡Atención!!!: La IP generada varía en cada ejercicio de despliegue en Kubernetes. Será necesario modificar el archivo [07_counter-ingress.yaml](./k8s/07_counter-ingress.yaml), reemplazando la IP del ejemplo con la obtenida en el paso anterior. Después de modificar la IP en el archivo, será necesario actualizar el objeto *Ingress* del clúster con**:

```bash
kubectl apply -f 07_counter-ingress.yaml --namespace=netcore-counter
```

Utilizando [nip.io](https://nip.io/) se puede acceder a la aplicación fácilmente fuera del clúster. En este ejemplo, está expuesta en "[http://netcore-counter.34.76.93.8.nip.io/api/v1/swagger/index.html"](http://netcore-counter.34.76.93.8.nip.io/api/v1/swagger/index.html):

![Swagger Kubernetes](./img/swagger-kubernetes.png)

En cada ejeercicio realizado, la dirección a utilizar será la resultante de reemplazar la IP del ejemplo con la obtenida anteriormente:

"[http://netcore-counter.<ADRESS>.nip.io/api/v1/swagger/index.html"](http://netcore-counter.<ADRESS>.nip.io/api/v1/swagger/index.html)
