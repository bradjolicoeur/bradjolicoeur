#take down the containers so we can reconfigure for production
docker-compose -f /srv/docker/docker-compose.yml down

#pull down the production configuration
sudo curl -o /nginx/nginx.conf https://raw.githubusercontent.com/bradjolicoeur/bradjolicoeur/master/nginx/nginx-srv.conf

#bring containers back up in production configuration
docker-compose up -d --force-recreate
