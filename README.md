# Counter

Proyecto académico con el objetivo de crear una aplicación .NET Core que:
1. Consuma una base de datos.
2. Se pueda ejecutar localmente con [Visual Studio](https://visualstudio.microsoft.com/es/vs/) 2019 (se debe configurar qué servicio Redis debe consumir).
3. Se pueda ejecutar localmente con [Docker](https://www.docker.com/) (se debe configurar qué servicio Redis debe consumir).
4. Se pueda ejecutar localmente con [Docker-compose](https://docs.docker.com/compose/) levantando un servicio Redis y con la aplicación preparada para consumirlo.
4. Se pueda ejecutar en un clúster Kubernetes en [GKE](https://cloud.google.com/kubernetes-engine/?hl=es) levantando un servicio Redis y con la aplicación preparada para consumirlo.
6. Publique mensajes de *log* de la aplicación en formato JSON por la salida estándar.

## Descripción de la aplicación

La aplicación es un servicio Web API simple, que permite utilizar un contador en un servicio Redis, con las siguientes operaciones:
* Leer el contador.
* Incrementar el contador, y devolver el valor del contador incrementado.

Además, tiene un método auxiliar para generar un error, y comprobar su publicación en el sistema de *Logs*.

## Funcionamiento de la aplicación

El API publicado por la documentación está documentado con [Swagger](https://swagger.io/). Se puede consultar la documentación ejecutando la aplicación, y accediendo a la dirección *http://**host**:**puerto**/api/v1/swagger/index.html*:

![Swagger](./img/swagger.png)

La aplicación se debe configurar con el archivo [appsettings.json](./solution/src/Counter.Web/appsettings.json), para establecer, por ejemplo, la información de conexión con Redis. La configuración puede ser sobreescrita por las variables de entorno que se establezcan el el contexto en el que se ejecute.

## Requisitos

Para compilar la aplicación es necesario:
* [SKD de .NET Core](https://dotnet.microsoft.com/download) 2.2.

Para ejecutar la aplicación es necesario:
* [Runtime de .NET Core](https://dotnet.microsoft.com/download/dotnet-core/2.2) 2.2.

## Ejecutar localmente la aplicación

Para la ejecución con **Docker**, es necesario configurar un servidio Redis pre-existente en el archivo [appsettings.json](./solution/src/Counter.Web/appsettings.json):

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter/solution
chmod 700 *.sh
./start.sh
```

Para la ejecución con **Docker-compose** se levanta un servicio Redis junto con el de la aplicación, configurando las variables de entorno en [.env](./solution/.env):

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter/solution
chmod 700 *.sh
./start-compose.sh
```

## Ejecutar la aplicación en Kubernetes GKE

Requisitos:
1. Conectar con un clúster Kubernetes previamente creado en GKE:

```bash
gcloud container clusters get-credentials *nombre_clúster_GKE* --zone *zona_clúster_GKE* --project *nombre_proyecto_GCP*
```

2. Instalar *kubectl*.

```bash
gcloud components install kubectl
```

Para la ejecución de la aplicación en un clúster de Kubernetes en GKE se utilizan las imágenes subidas de esta aplicación, y de Redis, en [Docker Hub](https://cloud.docker.com/repository/registry-1.docker.io/ureure/netcore-counter), ejecutando:

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter/k8s
chmod 700 *.sh
./start-kubernetes.sh *poner_aquí_la_contraseña_que_se_desee_poner_para_el_servicio_Redis*
```
