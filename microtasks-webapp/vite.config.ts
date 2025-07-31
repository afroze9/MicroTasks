import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // @ts-expect-error(Vite doesn't recognize this env variable type)
    port: parseInt(process.env.PORT || "3000", 10),
    proxy: {
      // Proxy API requests to the backend service
      "/companies": {
        target:
          // @ts-expect-error(Vite doesn't recognize this env variable type)
          process.env.services__company__https__0 ||
          // @ts-expect-error(Vite doesn't recognize this env variable type)
          process.env.services__company__http__0,
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
