# Node.js base image
FROM node:18

WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

RUN npm install

COPY . .

# React dev server port
EXPOSE 3000

# Start React development server
CMD ["npm", "start"]