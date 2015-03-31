#!/bin/sh

# Source: https://bitbucket.org/mindbar/install-mono/src

LIBGDI_TAG=2.10.8
MONO_TAG=mono-3.2.1
XSP_TAG=3.0.11
MONO_SRV=mono-default

sudo apt-get -y install python-software-properties
sudo add-apt-repository ppa:nginx/stable
sudo apt-get update && sudo apt-get -y install build-essential git autoconf libtool automake gettext nginx libglib2.0-dev libjpeg-dev libpng12-dev libgif-dev libexif-dev libx11-dev libxrender-dev libfreetype6-dev libfontconfig1-dev

mkdir monobuild
cd monobuild

git clone https://github.com/mono/libgdiplus.git
cd libgdiplus
git checkout $LIBGDI_TAG
./autogen.sh --prefix=/usr/local
make && sudo make install
cd ..

git clone https://github.com/mono/mono.git
cd mono
git checkout $MONO_TAG
./autogen.sh --prefix=/usr/local
make get-monolite-latest && make EXTERNAL_MCS=${PWD}/mcs/class/lib/monolite/gmcs.exe && sudo make install
cd ..

git clone https://github.com/mono/xsp.git
cd xsp
git checkout $XSP_TAG
./autogen.sh --prefix=/usr/local
make && sudo make install
cd ..
cd ..

# creating default site
mkdir www
git clone https://mindbar@bitbucket.org/mindbar/mono-mvc4-default.git www
sudo mkdir /usr/local/etc/mono/fcgi
sudo mkdir /usr/local/etc/mono/fcgi/apps-available
sudo mkdir /usr/local/etc/mono/fcgi/apps-enabled
sudo touch /usr/local/etc/mono/fcgi/apps-available/default
echo "/:`pwd`/www" | sudo tee -a /usr/local/etc/mono/fcgi/apps-available/default
sudo ln -s /usr/local/etc/mono/fcgi/apps-available/default /usr/local/etc/mono/fcgi/apps-enabled/default
wget https://bitbucket.org/mindbar/install-mono/raw/master/monoserve
sudo cp monoserve /etc/init.d/monoserve
sudo chmod +x /etc/init.d/monoserve
sudo update-rc.d monoserve defaults
sudo /etc/init.d/monoserve start
rm monoserve

# configure nginx
# disable default configuration
sudo rm /etc/nginx/sites-enabled/default

echo "# mono config" | sudo tee -a /etc/nginx/fastcgi_params
echo "fastcgi_param  PATH_INFO          \"\";" | sudo tee -a /etc/nginx/fastcgi_params
echo "fastcgi_param  SCRIPT_FILENAME    \$document_root\$fastcgi_script_name;" | sudo tee -a /etc/nginx/fastcgi_params
# new mono default server
echo "server {" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "         listen   80;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "         server_name  localhost;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "         location / {" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "                 root `pwd`/www/;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "                 index index.html index.htm default.aspx Default.aspx;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "                 fastcgi_index Home;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "                 fastcgi_pass 127.0.0.1:9001;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "                 include /etc/nginx/fastcgi_params;" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "         }" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
echo "}" | sudo tee -a /etc/nginx/sites-available/$MONO_SRV
# enable it
sudo ln -s /etc/nginx/sites-available/$MONO_SRV /etc/nginx/sites-enabled/$MONO_SRV
# restart nginx
sudo /etc/init.d/nginx restart

echo "\n\n\n\n\nMy job is done here. Try wget localhost && cat index.html to check site"
