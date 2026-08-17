import adapter from '@sveltejs/adapter-node';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	kit: {
		adapter: adapter(),
		csp: {
			directives: {
				'default-src': ['self'],
				'script-src': ['self', 'https://static.cloudflareinsights.com'],
				'connect-src': ['self', 'https://cloudflareinsights.com'],
				'worker-src': ['self', 'blob:'],
				'img-src': ['*', 'data:'],
				'base-uri': ['self'],
				'form-action': ['self'],
				'frame-ancestors': ['none'],
				'object-src': ['none']
			}
		},
	},
};

export default config;