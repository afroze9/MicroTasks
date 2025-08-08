import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: parseInt(process.env.PORT || "3000", 10),
    proxy: {
      // Proxy API requests to the backend service
      "/api/companies": {
        target:
          process.env.services__company__https__0 ||
          process.env.services__company__http__0,
        changeOrigin: true,
        secure: false,
      },
      "/api/projects": {
        target:
          process.env.services__project__https__0 ||
          process.env.services__project__http__0,
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
