import type { ModData, ModHistoryData } from '$lib';
import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ params, fetch }) => {
	const mod_param = params.mod;
	if (!mod_param) {
		error(404, 'Mod not found');
	}

	const response = await fetch(`/api/mod_info/${mod_param}`);
	if (!response.ok) {
		error(response.status, 'Failed to load mod');
	}

	const mod: ModData = await response.json();

	async function getModHistory(): Promise<ModHistoryData> {
		return await ((await fetch(`/api/mod_history/${mod.mod_id}`)).json())
	}

	async function getChildrenInfo(): Promise<ModData[]> {
		if (!mod.children) return []
		let children = []
		for (const childID of mod.children) {
			children.push(await ((await fetch(`/api/mod_info/${childID}`)).json()))
		}
		return children
	}

	return {
		mod,
		history: getModHistory(),
		children: getChildrenInfo()
	};
};
