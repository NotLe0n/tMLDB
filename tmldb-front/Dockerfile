# Base stage for dependencies
FROM node:20-alpine AS base
WORKDIR /app

# Dependencies stage
FROM base AS deps
COPY package*.json ./
RUN npm ci

# Build stage
FROM base AS build
COPY --from=deps /app/node_modules ./node_modules
COPY . .
ARG BACKEND_URL
RUN BACKEND_URL=$BACKEND_URL npm run build

# Production stage
FROM base AS production
ENV NODE_ENV=production

COPY --from=build /app/build ./build
COPY --from=build /app/package*.json ./
COPY --from=build /app/node_modules ./node_modules

EXPOSE 3000
CMD ["node", "build"]
