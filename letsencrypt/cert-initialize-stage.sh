sudo docker run -it --rm \
-v /docker-volumes/etc/letsencrypt:/etc/letsencrypt \
-v /docker-volumes/var/lib/letsencrypt:/var/lib/letsencrypt \
-v /srv/docker/letsencrypt/src/production/production-site:/data/letsencrypt \
-v "/docker-volumes/var/log/letsencrypt:/var/log/letsencrypt" \
certbot/certbot \
certonly --webroot \
--register-unsafely-without-email --agree-tos \
--webroot-path=/data/letsencrypt \
--staging \
-d bradjolicoeur.com -d www.bradjolicoeur.com

sudo openssl dhparam -outform PEM -out /srv/docker/letsencrypt/src/production/dh-param/dhparam-2048.pem 2048

# sudo openssl dhparam -check -in /srv/docker/letsencrypt/src/production/dh-param/dhparam-2048.pem