#!/bin/bash

# install latest version of docker the lazy way
sudo curl -sSL https://get.docker.com | sh

# make it so you don't need to sudo to run docker commands
sudo usermod -aG docker ubuntu

# install docker-compose
sudo curl -L https://github.com/docker/compose/releases/download/1.21.2/docker-compose-$(uname -s)-$(uname -m) -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# copy the dockerfile into /srv/docker 
# if you change this, change the systemd service file to match
# WorkingDirectory=[whatever you have below]
sudo mkdir /srv/docker
sudo curl -o /srv/docker/docker-compose.yml https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/docker-compose-server.yml

# copy in systemd unit file and register it so our compose file runs 
# on system restart
sudo curl -o /etc/systemd/system/docker-compose-app.service https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/docker-compose-app.service
sudo systemctl enable docker-compose-app

# copy in the nginx config file
sudo mkdir /nginx
sudo curl -o /nginx/nginx.conf https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/nginx/nginx-srv.conf

sudo mkdir -p /srv/docker/letsencrypt/src/production/production-site
sudo mkdir -p /srv/docker/letsencrypt/src/production/dh-param

# start up the application via docker-compose
sudo docker-compose -f /srv/docker/docker-compose.yml up -d

sudo curl -o cert-initialize.sh https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/letsencrypt/cert-initialize.sh
sudo chmod +X ./cert-initialize.sh
