#!/bin/bash

# Summary: 
#   This script will change all .jpg images collected in the Winter, Summer, Autumn and Spring folders 
#   to 80% quality and a width of 300 pixels while keeping original ratio. Files are renamed with 
#   increasing numbers per folder.
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
    convert $image -resize 300x -quality 80% $season/$i.jpg;
    rm $image;
    ((i++));
  done
done

echo "All jp(e)gs are converted to 300 pixels and 80% quality."
