# Counter

Proyecto académico con el objetivo de crear una aplicación [.NET Core](https://dotnet.microsoft.com/download) que:
1. Consuma una base de datos.
2. Se pueda ejecutar localmente con [Visual Studio](https://visualstudio.microsoft.com/es/vs/) 2019 (se debe configurar qué servicio Redis debe consumir).
3. Se pueda ejecutar localmente con [Docker](https://www.docker.com/) (se debe configurar qué servicio Redis debe consumir).
4. Se pueda ejecutar localmente con [Docker-compose](https://docs.docker.com/compose/), levantando un servicio Redis para probar la aplicación.
5. Publique mensajes de *log* de la aplicación.

## Descripción de la aplicación

La aplicación es un servicio Web API, con la documentación expuesta utilizando [Swagger](https://swagger.io/):

![Swagger](./img/swagger.png)

Cómo probar la aplicación:

```bash
git clone https://github.com/UREURE/netcore-counter.git
cd netcore-counter
chmod 700 *.sh
docker-compose up --build
docker-compose rm -f
```

