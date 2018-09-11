#lw2

Building:
```sh
sudo sh build.sh <version>
```

Run:
```sh
sudo sh <version>/run.sh
```

Stop:
```sh
sudo sh <version>/stop.sh
```

Force stop (Linux/MacOs terminal):
```sh
sudo lsof -i:5050
kill -9 <pid>
```