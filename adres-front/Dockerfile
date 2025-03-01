# Etapa 1: Construcción de la app Angular
FROM node:22 AS build

# Directorio de trabajo en el contenedor
WORKDIR /app

# Copiar el resto del código de la aplicación Angular
COPY . /app

# Copiar el package.json y package-lock.json (si existe) y luego instalar las dependencias
RUN npm install

# Ejecutar el comando de build de Angular para generar los archivos de producción
RUN npm run build --prod

# Etapa 2: Servir la app con Nginx
FROM nginx:alpine

# Copiar los archivos generados por Angular desde la etapa de construcción
# Asegúrate de que la ruta sea correcta según el nombre de tu app Angular
COPY --from=build /app/dist/adres/browser /usr/share/nginx/html

# Exponer el puerto 80 para acceder a la app a través de Nginx
EXPOSE 80
