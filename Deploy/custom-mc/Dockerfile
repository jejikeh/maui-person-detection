FROM ubuntu:latest

RUN apt-get update && apt-get install -y wget && apt-get install -y curl
RUN wget https://dl.min.io/client/mc/release/linux-amd64/mc
RUN chmod +x mc

ENTRYPOINT [ "./mc" ]