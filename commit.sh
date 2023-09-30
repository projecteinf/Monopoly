#!/bin/bash
if [ $# -ne 1 ]
  then
    echo "No arguments supplied"
    exit 1
fi

git add .
git commit -m "$1" 
git add .
