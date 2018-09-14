#generate certificate from letsencrypt
sudo docker run -it --rm \
-v /docker-volumes/etc/letsencrypt:/etc/letsencrypt \
-v /docker-volumes/var/lib/letsencrypt:/var/lib/letsencrypt \
-v /srv/docker/letsencrypt/src/production/production-site:/data/letsencrypt \
-v "/docker-volumes/var/log/letsencrypt:/var/log/letsencrypt" \
certbot/certbot \
certonly --webroot \
--email brad@jolicoeurs.net --agree-tos --no-eff-email \
--webroot-path=/data/letsencrypt \
-d bradjolicoeur.com -d www.bradjolicoeur.com

#Create dh param
sudo openssl dhparam -outform PEM -out /srv/docker/letsencrypt/src/production/dh-param/dhparam-2048.pem 2048

#Check the dh param to make sure it is valid
sudo openssl dhparam -check -in /srv/docker/letsencrypt/src/production/dh-param/dhparam-2048.pem