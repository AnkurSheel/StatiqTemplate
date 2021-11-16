#!/bin/bash
# npx tailwind build -c ./node/tailwind.config.js -i ./Bookland/input/assets/_site.css -o ././Bookland/output/assets/styles.css --watch & dotnet run --project ./Bookland -- preview
cd node
npx tailwind build -c ./tailwind.config.js -i ../src/Bookland/input/assets/_site.css -o ../src/Bookland/input/assets/styles.css --watch
