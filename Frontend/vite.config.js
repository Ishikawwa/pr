import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'node:path';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': 'http://localhost:5073',
    },
  },
  build: {
    outDir: resolve(import.meta.dirname, '../Presentation/WebApi/wwwroot'),
    emptyOutDir: true,
  },
});
