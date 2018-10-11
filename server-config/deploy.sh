cd /srv/docker
docker-compose --log-level ERROR pull website
docker-compose --log-level ERROR down
docker-compose --log-level ERROR up -d