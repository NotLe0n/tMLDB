import type { AuthorData, AuthorHistoryData } from '$lib';
import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ params, fetch }) => {
	const creator_param = params.creator;
	if (!creator_param) {
		error(404, 'Creator not found');
	}

	const response = await fetch(`/api/creator_info/${creator_param}`);
	if (!response.ok) {
		error(response.status, 'Failed to load creator');
	}

	const creator: AuthorData = await response.json();

	async function getCreatorHistory(): Promise<AuthorHistoryData> {
		return await ((await fetch(`/api/creator_history/${creator.steam_id}`)).json())
	}

	return {
		creator,
		history: getCreatorHistory()
	};
};
