# Guía de Ejecución
Este proyecto usa Docker Compose para configurar y ejecutar un contenedor de la aplicación y un contenedor de base de datos PostgreSQL. A continuación, se detallan los pasos para ejecutar el proyecto, conectarse a la base de datos y acceder a la aplicación.

## Requisitos Previos
Antes de comenzar, asegúrate de tener instalados los siguientes programas:

- Docker
- Docker Compose
-  DBeaver (para conectarse a la base de datos PostgreSQL)

## Pasos de Ejecución

1. Abra una terminal y navegue a la raiz del directorio donde se encuentra el archivo docker-compose.yml.

2. Ejecute el siguiente comando para construir y levantar los contenedores del frontend, el backend y la database:

```docker-compose up -d ```

3. Conectese a la base de datos PostgreSQL con DBeaver, ingresando la siguiente información de conexión:

```
Host: localhost
Puerto: 5432
Base de Datos: adres
Usuario: postgres
Contraseña: 1234
```

4. Ejecute los siguientes scripts en la base de datos:
```
CREATE TABLE adquisicion (
	"Id" uuid NOT NULL,
	"Presupuesto" numeric NOT NULL,
	"Unidad" text NOT NULL,
	"TipoBienServicio" text NOT NULL,
	"Cantidad" int8 NOT NULL,
	"ValorUnitario" numeric NOT NULL,
	"ValorTotal" numeric NOT NULL,
	"Fecha" date NOT NULL,
	"Proveedor" text NOT NULL,
	"Documentacion" text NOT NULL,
	"Activo" bool NOT NULL,
	CONSTRAINT "PK_adquisicion" PRIMARY KEY ("Id")
);

CREATE TABLE historico (
	"Id" uuid NOT NULL,
	"AdquisicionId" uuid NOT NULL,
	"TipoCambio" int4 NOT NULL,
	"DetalleCambio" text NOT NULL,
	"FechaCreacion" timestamptz NOT NULL,
	CONSTRAINT "PK_historico" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_historico_adquisicion_AdquisicionId" FOREIGN KEY ("AdquisicionId") REFERENCES adquisicion("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_historico_AdquisicionId" ON public.historico USING btree ("AdquisicionId");
```

1. Abra el navegador web y acceda a la aplicación por la ruta http://localhost:4200.

2. Para detener la aplicación, es necesario detener los contenedores.
```docker-compose down```