sudo mkdir -p /srv/docker/letsencrypt-docker-nginx/src/letsencrypt/letsencrypt-site
sudo curl -o /srv/docker/letsencrypt-docker-nginx/src/letsencrypt/docker-compose.yml https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/letsencrypt/docker-compose.yml

sudo mkdir -p /srv/docker/letsencrypt-docker-nginx/src/letsencrypt/letsencrypt-site/
sudo curl -o /srv/docker/letsencrypt-docker-nginx/src/letsencrypt/letsencrypt-site/index.html https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/letsencrypt/index.html

cd /srv/docker/letsencrypt-docker-nginx/src/letsencrypt
sudo docker-compose -f /srv/docker/letsencrypt-docker-nginx/src/letsencrypt/docker-compose.yml up -d

# sudo docker run -it --rm \
# -v /srv/docker-volumes/etc/letsencrypt:/etc/letsencrypt \
# -v /srv/docker-volumes/var/lib/letsencrypt:/var/lib/letsencrypt \
# -v /srv/docker/letsencrypt-docker-nginx/src/letsencrypt/letsencrypt-site:/data/letsencrypt \
# -v "/srv/docker-volumes/var/log/letsencrypt:/var/log/letsencrypt" \
# certbot/certbot \
# certonly --webroot \
# --register-unsafely-without-email --agree-tos \
# --webroot-path=/data/letsencrypt \
# --staging \
# -d bradjolicoeur.com -d www.bradjolicoeur.com