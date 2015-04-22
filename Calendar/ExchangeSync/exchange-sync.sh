#!/bin/bash

export PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/usr/games:/usr/local/games

git clone git@bitbucket.org:tobiasschulz/calendars.git calendars
./ExchangeSync.exe
mv output.ical calendars/exchange.ical
cd calendars
git pull
git add --all
git commit -m "$(date)" -a
git push
