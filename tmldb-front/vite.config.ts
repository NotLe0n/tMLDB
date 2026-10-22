import adapter from '@sveltejs/adapter-node';
import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [
		sveltekit({
			adapter: adapter(),
			csp: {
				directives: {
					'default-src': ['none'],
					'script-src': ['self', 'https://static.cloudflareinsights.com'],
					'connect-src': ['self', 'https://cloudflareinsights.com'],
					'worker-src': ['self', 'blob:'],
					'style-src': ['self', 'unsafe-inline'],
					'font-src': ['self'],
					'img-src': ['http:', 'https:', 'data:'],
					'base-uri': ['self'],
					'form-action': ['self'],
					'frame-ancestors': ['none']
				}
			}
		})
	]
});
