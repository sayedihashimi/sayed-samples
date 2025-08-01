import react from '@vitejs/plugin-react'
import { defineConfig, loadEnv } from 'vite'

export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), "");
    return {
        plugins: [react()],
        server: {
            // port: parseInt(process.env.PORT ?? "5173"),
            proxy: {
                '/api': {
                    target:
                        env.services__apiservice__https__0 ||
                        env.services__apiservice__http__0,
                    changeOrigin: true,
                    secure: false,
                    rewrite: (path) => path.replace(/^\/api/, "")
                }
            }
        }
    }
})
