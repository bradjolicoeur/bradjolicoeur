#take down the containers so we can reconfigure for production
docker-compose -f /srv/docker/docker-compose.yml down

#pull down the production configuration
sudo curl -o /nginx/nginx.conf https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/nginx/nginx-srv.conf

#pull down the production docker compose
sudo curl -o /srv/docker/docker-compose.yml https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/docker-compose-server.yml


#bring containers back up in production configuration
docker-compose -f /srv/docker/docker-compose.yml up -d --force-recreate
