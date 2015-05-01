#!/bin/bash

# Summary: 
#   This script will change all .jpg and .jpeg images collected in the Winter, Summer, Autumn and Spring 
#   folders to 95% quality and a width of 500 pixels while keeping original ratio. Files are renamed with 
#   increasing numbers per folder. Images were obtained from http://pixabay.com/ (small format).
#   Note: original images will be removed.
#
# Dependency: 
#   ImageMagick.
#
# Usage:
#   Place script in root of images folder and run with "bash convert-images.sh".

seasons=("Spring" "Summer" "Autumn" "Winter");

for season in "${seasons[@]}"; do
  i=1;
  for image in $season/*.jp*; do
    convert $image -resize 500x -quality 95% $season/$i.jpg;
    rm $image;
    ((i++));
  done
done

echo "All jp(e)gs are converted to 500 pixels and 95% quality."