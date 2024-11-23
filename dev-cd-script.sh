#!/bin/bash

REPO_DIR="/root/penomy/dev/Main/PenomyApi/"
DOCKER_COMPOSE_DIR=".."
GIT_URL="https://github.com/Jackpieking/PenomyApi.git"
CSPROJ_FILES_TO_SYNC="csrpoj_to_sync.txt"
CSPROJ_DIRS="sync_csproj"

# create penomy api for dev environment folder
echo "Creating dir for repo..."
mkdir -p $REPO_DIR

# cd to penomy api folder
echo "Cd to repo dir..."
cd $REPO_DIR

# Set the Git repository URL you want to clone

# Check if the .git directory exists
if [ ! -d "$REPO_DIR/.git" ]; then
  echo "Not an existing git repository. Cloning the repository..."
  git clone "$GIT_URL" "$REPO_DIR"

  echo "Switching to dev branch..."
  git switch -c dev

  echo "Pull the latest changes from dev branch..."
  git pull origin dev
else
  echo "The repository is already cloned, switching to dev branch...,"
  git switch dev

  echo "Pull the latest changes from dev branch..."
  git pull origin dev
fi

# Generate folder containing relative path from current folder
# to all .csproj files
echo "Find and creating a list of csproj need to sync..."
find . -name '*.csproj' > $CSPROJ_FILES_TO_SYNC

# Create and genrated all_csproj folder
# containing all .csproj files in src
echo "Syncing csproj files to $csproj_dirs..."
mkdir -p $CSPROJ_DIRS && rsync -a --files-from=$CSPROJ_FILES_TO_SYNC --delete --include='*/' --include="*.csproj" --exclude="*" ./ "${CSPROJ_DIRS}/"

# Cd to docker compose dir
echo "Cd to docker compose dir..."
cd $DOCKER_COMPOSE_DIR

# Build docker compose
docker compose up -d --build

# Cleaning
rm -rf ./sync_csproj/