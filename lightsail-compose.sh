#!/bin/bash

# install latest version of docker the lazy way
curl -sSL https://get.docker.com | sh

# make it so you don't need to sudo to run docker commands
usermod -aG docker ubuntu

# install docker-compose
curl -L https://github.com/docker/compose/releases/download/1.21.2/docker-compose-$(uname -s)-$(uname -m) -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose

# copy the dockerfile into /srv/docker 
# if you change this, change the systemd service file to match
# WorkingDirectory=[whatever you have below]
mkdir /srv/docker
curl -o /srv/docker/docker-compose.yml https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/docker-compose-server.yml

# copy in systemd unit file and register it so our compose file runs 
# on system restart
curl -o /etc/systemd/system/docker-compose-app.service https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/docker-compose-app.service
systemctl enable docker-compose-app

# copy in the nginx config file
mkdir /nginx
curl -o /nginx/nginx.conf https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/nginx/nginx-srv.conf

mkdir -p /srv/docker/letsencrypt/src/production/production-site
mkdir -p /srv/docker/letsencrypt/src/production/dh-param
mkdir -p /docker-volumes/etc/letsencrypt/live/bradjolicoeur.com

# start up the application via docker-compose
docker-compose -f /srv/docker/docker-compose.yml up -d

curl -o cert-initialize.sh https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/letsencrypt/cert-initialize.sh
chmod +x ./cert-initialize.sh
