import type { RequestHandler } from './$types';
import { BACKEND_URL } from '$env/static/private';

export const GET: RequestHandler = ({ params, url }) => {
    return fetch(`${BACKEND_URL}/${params.path}${url.search}`);
};